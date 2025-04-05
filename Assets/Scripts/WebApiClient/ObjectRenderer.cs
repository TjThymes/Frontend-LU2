using UnityEngine;
using TMPro;

public class ObjectRenderer : MonoBehaviour
{
    public TextMeshProUGUI EnvIDText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string EnvID = PlayerPrefs.GetString("SelectedEnvironmentID", "");
        EnvIDText.text = EnvID;
    }
}
