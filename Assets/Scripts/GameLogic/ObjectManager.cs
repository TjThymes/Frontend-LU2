using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using WebApiClient.ApiClient;

public class ObjectManager : MonoBehaviour
{
    public GameObject UISideMenu;
    public List<GameObject> prefabObjects;
    public List<GameObject> placedObjects = new List<GameObject>();
    public ApiClient apiClient;

    private string environmentApiUrl = "environments2d";
    private string objectsApiUrl = "objects";

    private void Start()
    {
        LoadObjects();
    }

    public async void LoadObjects()
    {
        string envId = PlayerPrefs.GetString("SelectedEnvironmentID", "");
        if (string.IsNullOrEmpty(envId)) return;

        var response = await apiClient.GetAsync<EnvironmentResponse>($"{environmentApiUrl}/getwithobjects/{envId}");

        if (response.objects == null) return;

        foreach (var objData in response.objects)
        {
            if (objData.prefabID < 0 || objData.prefabID >= prefabObjects.Count)
                continue;

            GameObject newObj = Instantiate(prefabObjects[objData.prefabID]);
            newObj.transform.position = new Vector3(objData.positionX, objData.positionY, 0);
            newObj.transform.localScale = new Vector3(objData.scaleX, objData.scaleY, 1);
            newObj.transform.rotation = Quaternion.Euler(0, 0, objData.rotationZ);

            Object2D script = newObj.GetComponent<Object2D>();
            if (script != null)
            {
                script.AssignObjectData(objData);
                script.objectManager = this;
            }
            placedObjects.Add(newObj);
        }
    }

    public async void SaveObject(GameObject obj, int prefabId)
    {
        var envId = PlayerPrefs.GetString("SelectedEnvironmentID", "");
        if (string.IsNullOrEmpty(envId)) return;

        var objectData = new GameObjectData
        {
            id = System.Guid.NewGuid().ToString(),
            environmentID = envId,
            prefabID = prefabId,
            positionX = obj.transform.position.x,
            positionY = obj.transform.position.y,
            scaleX = obj.transform.localScale.x,
            scaleY = obj.transform.localScale.y,
            rotationZ = obj.transform.rotation.eulerAngles.z,
            sortingLayer = 0
        };

        await apiClient.PostAsync<GameObjectData, object>($"{objectsApiUrl}/saveobject/{envId}", objectData);

        Object2D script = obj.GetComponent<Object2D>();
        if (script != null)
        {
            script.id = objectData.id;
        }
    }
}
