using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WebApiClient.ApiClient;
using Assets.Scripts.Models;

public class ObjectManager : MonoBehaviour
{
    public GameObject UISideMenu;
    public List<GameObject> prefabObjects;
    public List<GameObject> placedObjects = new List<GameObject>();
    public ApiClient apiClient;
    public float worldWidth = 10f;  // Wordt straks overschreven
    public float worldHeight = 10f;

    public Object2D selectedObject; 

    private string environmentApiUrl = "environments2d";
    private string objectsApiUrl = "objects";

    private async void Start()
    {
        apiClient.Initialize("https://avansict2232490.azurewebsites.net/api/");
        apiClient.SetAccessToken(PlayerPrefs.GetString("UserToken"));
        await LoadObjects();

       
    
}

    public async Task LoadObjects()
    {
        string envId = PlayerPrefs.GetString("SelectedEnvironmentID", "");
        if (string.IsNullOrEmpty(envId))
        {
            Debug.LogWarning("No environment selected.");
            return;
        }

        try
        {
            var response = await apiClient.GetAsync<EnvironmentResponse>($"{environmentApiUrl}/getwithobjects/{envId}");
            if (response?.objects == null)
            {
                Debug.LogWarning("No objects found in environment.");
                return;
            }
            var visualizer = FindFirstObjectByType<WorldVisualizer>();
            if (visualizer != null)
            {
                visualizer.SetWorldSize(response.width, response.height);
            }

            foreach (var objData in response.objects)
            {
                if (objData.prefabID < 0 || objData.prefabID >= prefabObjects.Count)
                    continue;

                GameObject newObj = Instantiate(prefabObjects[objData.prefabID]);
                newObj.transform.position = new Vector3(objData.positionX, objData.positionY, 0);
                newObj.transform.localScale = new Vector3(objData.scaleX, objData.scaleY, 1);
                newObj.transform.rotation = Quaternion.Euler(0, 0, objData.rotationZ);

                var script = newObj.GetComponent<Object2D>();
                if (script != null)
                {
                    script.AssignObjectData(objData);
                    script.objectManager = this;
                    script.apiClient = apiClient;
                }
                placedObjects.Add(newObj);
            }

            Debug.Log($"✅ Loaded {response.objects.Count} objects.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Error loading objects: {ex.Message}");
        }
    }

    // 🔥 Nieuw: Object plaatsen vanuit knop!
    public void PlaceNewObject(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= prefabObjects.Count)
        {
            Debug.LogWarning("❌ Invalid prefab index.");
            return;
        }

        GameObject newObj = Instantiate(prefabObjects[prefabIndex]);
        newObj.transform.localScale = new Vector3(3, 3, 2); // Reset scale

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 1f;
        newObj.transform.position = mouseWorldPosition;

        var script = newObj.GetComponent<Object2D>();
        if (script != null)
        {
            script.prefabID = prefabIndex;
            script.objectManager = this;
            script.apiClient = apiClient;
            script.isDragging = true; // ✅ HIER: direct na plaatsen dragging starten
        }

        placedObjects.Add(newObj);
        SelectObject(script);

        if (UISideMenu != null)
            UISideMenu.SetActive(false);
    }




    // 🔥 Nieuw: Geselecteerd object opslaan
    public void SaveSelectedObject()
    {
        if (selectedObject == null)
        {
            Debug.LogWarning("❌ No object selected to save.");
            return;
        }
        SaveObject(selectedObject.gameObject, selectedObject.prefabID);
    }

    // 🔥 Nieuw: Geselecteerd object verwijderen
    public void DeleteSelectedObject()
    {
        if (selectedObject == null)
        {
            Debug.LogWarning("❌ No object selected to delete.");
            return;
        }
        DeleteObject(selectedObject.gameObject);
        selectedObject = null;
    }

    public void SelectObject(Object2D obj)
    {
        selectedObject = obj;
        HighlightSelectedObject();
    }

    private void HighlightSelectedObject()
    {
        foreach (var obj in placedObjects)
        {
            var renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = (obj.GetComponent<Object2D>() == selectedObject) ? Color.green : Color.white;
            }
        }
    }

    public async void SaveObject(GameObject obj, int prefabId)
    {
        if (obj == null || apiClient == null) return;

        string envId = PlayerPrefs.GetString("SelectedEnvironmentID", "");
        if (string.IsNullOrEmpty(envId))
        {
            Debug.LogWarning("EnvironmentID missing when trying to save object.");
            return;
        }

        var script = obj.GetComponent<Object2D>();
        string id = script != null && !string.IsNullOrEmpty(script.id) ? script.id : System.Guid.NewGuid().ToString();

        var objectData = new GameObjectData
        {
            id = id,
            environmentID = envId,
            prefabID = prefabId,
            positionX = obj.transform.position.x,
            positionY = obj.transform.position.y,
            scaleX = obj.transform.localScale.x,
            scaleY = obj.transform.localScale.y,
            rotationZ = obj.transform.rotation.eulerAngles.z,
            sortingLayer = 0
        };

        try
        {
            if (string.IsNullOrEmpty(script.id))
            {
                await apiClient.PostAsync<GameObjectData, object>($"{objectsApiUrl}/SaveObject/{envId}", objectData);
                script.id = objectData.id;
            }
            else
            {
                {
                    // Bestaand object updaten
                    await apiClient.PutAsync<object>($"{objectsApiUrl}/{id}", objectData);
                }
                //await apiClient.PutAsync($"{objectsApiUrl}/objects/{id}", objectData);
            }

            if (!placedObjects.Contains(obj))
            {
                placedObjects.Add(obj);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Failed to save object: {ex.Message}");
        }
    }


    public async void DeleteObject(GameObject obj)
    {
        if (obj == null) return;
        var script = obj.GetComponent<Object2D>();
        if (script == null || string.IsNullOrEmpty(script.id)) return;

        try
        {
            await apiClient.DeleteAsync($"{objectsApiUrl}/{script.id}");
            placedObjects.Remove(obj);
            Destroy(obj);
        }
        catch (System.Exception ex)
        {
        
        }
    }
    public async void Reload()
    {
        foreach (var obj in placedObjects)
        {
            Destroy(obj);
        }

        placedObjects.Clear(); // ✅ heel belangrijk!

        await LoadObjects();
    }


    public void ShowMenu()
    {
        if (UISideMenu != null)
        {
            UISideMenu.SetActive(!UISideMenu.activeSelf);
        }
        else
        {
            Debug.LogWarning("UISideMenu is not assigned!");
        }
    }
}
