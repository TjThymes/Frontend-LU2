using UnityEngine;
using Assets.Scripts.Models;
using WebApiClient.ApiClient;
using NUnit.Framework.Internal.Execution;

public class Object2D : MonoBehaviour
{
    public ObjectManager objectManager;
    public ApiClient apiClient;
    public bool isDragging = false;
    public int prefabID;
    public string id;

    private bool isMouseOver = false; 

    private void Update()
    {
        if (isDragging)
        {
            Vector3 pos = GetMousePosition();
            pos.z = pos.z + 1;
            transform.position = pos; // 

        }



        if (isMouseOver)
        {
            //  Delete met D
            if (Input.GetKeyDown(KeyCode.D))
            {
                objectManager.DeleteObject(this.gameObject);
            }

            //  Roteren met R
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject(15f); // 15 graden rechtsom
            }

            //  Shift + R = Linksom draaien
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                RotateObject(-15f); // 15 graden linksom
            }
            if (objectManager != null && objectManager.selectedObject == this)
            {
                HandleScaling(); // Scaling
            }
        }
    }


    private void OnMouseEnter()
    {
        isMouseOver = true;
        Highlight(Color.green); // Word groen bij mouse over
        if (!isDragging && objectManager != null)
        {
            objectManager.SelectObject(this); // Zorg ervoor dat al ingeladen objecten ook geselecteerd kunnen worden
        }
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        Highlight(Color.white); // Terug naar wit als muis weg is
    }

    private Vector3 GetMousePosition()
    {
        Vector3 positionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionInWorld.z = 0;
        return positionInWorld;
    }

    private void Highlight(Color color)
    {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = color;
        }
    }

    private void OnMouseUpAsButton()
    {
        isDragging = !isDragging;

        if (!isDragging)
        {
            objectManager.ShowMenu();
            SaveObject();
        }
    }
    private void RotateObject(float degrees)
    {
        transform.Rotate(0f, 0f, degrees);

        if (objectManager != null)
        {
            objectManager.SaveObject(this.gameObject, prefabID);
        }
    }
    private void HandleScaling()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f) // beetje gevoeligheid
        {
            float scaleChange = 0.1f;
            Vector3 newScale = transform.localScale + Vector3.one * (scroll > 0 ? scaleChange : -scaleChange);

            // Voorkom negatief scalen
            newScale.x = Mathf.Max(0.1f, newScale.x);
            newScale.y = Mathf.Max(0.1f, newScale.y);

            transform.localScale = newScale;

            // 🔥 Meteen opslaan
            objectManager.SaveObject(this.gameObject, prefabID);
        }
    }



    public async void SaveObject()
    {
        if (apiClient != null)
        {
            objectManager.SaveObject(this.gameObject, prefabID);
        }
    }

    public void AssignObjectData(GameObjectData data)
    {
        id = data.id;
        prefabID = data.prefabID;
    }

}
