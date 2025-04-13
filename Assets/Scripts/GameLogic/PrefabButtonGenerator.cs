using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PrefabButtonGenerator : MonoBehaviour
{
    public GameObject buttonPrefab; 
    public Transform buttonParent;  
    public List<GameObject> prefabList; 
    public ObjectManager objectManager; 

    private void Start()
    {
        GenerateButtons();
    }
    // Alles om de knoppen te genereren
    private void GenerateButtons()
    {
        for (int i = 0; i < prefabList.Count; i++)
        {
            int capturedIndex = i; 

            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            Button button = buttonObj.GetComponent<Button>();
            Image img = buttonObj.GetComponent<Image>();

            if (img != null && prefabList[capturedIndex].GetComponent<SpriteRenderer>() != null)
            {
                img.sprite = prefabList[capturedIndex].GetComponent<SpriteRenderer>().sprite;
            }

            
            button.onClick.AddListener(() => objectManager.PlaceNewObject(capturedIndex));
        }
    }
}
