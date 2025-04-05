using UnityEngine;

public class Object2D : MonoBehaviour
{
    public ObjectManager objectManager;
    public string id;
    public int prefabID;
    public bool isDragging;
    public bool isMouseOver;
    

    private void Update()
    {
        if (isDragging)
            transform.position = GetMousePosition();

        if (isMouseOver && Input.GetKeyDown(KeyCode.D))
            DeleteObject();
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (string.IsNullOrEmpty(id))
            objectManager.SaveObject(this.gameObject, prefabID);
        else
            UpdateObject();
    }

    private void OnMouseEnter() => isMouseOver = true;
    private void OnMouseExit() => isMouseOver = false;

    public void AssignObjectData(GameObjectData data)
    {
        id = data.id;
        prefabID = data.prefabID;
    }

    private Vector3 GetMousePosition()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    private async void UpdateObject()
    {
        if (string.IsNullOrEmpty(id)) return;

        var updatedObject = new GameObjectData
        {
            id = id,
            environmentID = PlayerPrefs.GetString("SelectedEnvironmentID", ""),
            prefabID = prefabID,
            positionX = transform.position.x,
            positionY = transform.position.y,
            scaleX = transform.localScale.x,
            scaleY = transform.localScale.y,
            rotationZ = 0,
            sortingLayer = 0
        };

        await objectManager.apiClient.PutAsync<GameObjectData>($"objects/{id}", updatedObject);

    }

    private async void DeleteObject()
    {
        if (string.IsNullOrEmpty(id)) return;

        await objectManager.apiClient.DeleteAsync($"objects/{id}");

        Destroy(this.gameObject);
    }

}
