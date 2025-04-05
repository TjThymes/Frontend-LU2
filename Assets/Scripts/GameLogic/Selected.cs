using UnityEngine.EventSystems;
using UnityEngine;

public class SelectableObject : MonoBehaviour, IPointerClickHandler
{
    private ObjectManager objectManager;

    private void Start()
    {
        objectManager = FindObjectOfType<ObjectManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        objectManager.SelectObject(gameObject);
    }
}