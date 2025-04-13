using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using WebApiClient.ApiClient;

public class EnvironmentMaker : MonoBehaviour
{
    private const string CreateEnvironmentEndpoint = "environments2d/create";

    public TMP_InputField environmentNameInput;
    public TMP_InputField environmentDescriptionInput;
    public TMP_InputField widthInput;  // 🆕
    public TMP_InputField heightInput; // 🆕
    public TMP_Text feedbackText;
    public ApiClient apiClient; // ✅ Use ApiClient service
    public EnvironmentLoader EnvironmentLoader;

    private void Start()
    {
        string token = PlayerPrefs.GetString("UserToken", "");

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("❌ No user token found. Please log in first.");
            feedbackText.text = "❌ Please log in before creating an environment.";
            return;
        }

        apiClient.SetAccessToken(token); // ✅ Set token for API requests
        Debug.Log("✅ Token Loaded: " + token);
    }

    public async void CreateEnvironment()
    {
        if (environmentNameInput == null || feedbackText == null)
        {
            Debug.LogError("❌ UI elements are not assigned in the Unity Inspector!");
            return;
        }

        if (string.IsNullOrWhiteSpace(environmentNameInput.text))
        {
            feedbackText.text = "❌ Please provide a name for the environment!";
            return;
        }

        var newEnvironment = new EnvironmentData
        {
            Name = environmentNameInput.text,
            Description = string.IsNullOrWhiteSpace(environmentDescriptionInput.text) ? null : environmentDescriptionInput.text,
            Width = int.Parse(widthInput.text),  // 🆕
            Height = int.Parse(heightInput.text) // 🆕
        };

        try
        {
            await apiClient.PostAsync<EnvironmentData, object>(CreateEnvironmentEndpoint, newEnvironment);
            feedbackText.text = "✅ Environment created successfully!";
            EnvironmentLoader.Reload();
            Debug.Log("✅ Environment Created!");

        }
        catch (Exception ex)
        {
        }
    }
}

[System.Serializable]
public class EnvironmentData
{
    public string Name;
    public string Description;
    public int Width;
    public int Height;
}
