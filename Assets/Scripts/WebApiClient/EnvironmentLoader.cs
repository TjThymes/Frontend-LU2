using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using WebApiClient.ApiClient;

public class EnvironmentLoader : MonoBehaviour
{
    private const string GetEnvironmentIdsEndpoint = "environments2d/myenvironmentids";

    public GameObject environmentButtonPrefab;
    public Transform environmentListContainer;
    public TMP_Text feedbackText;
    public ApiClient apiClient;

    private async void Start()
    {
        await LoadEnvironments();
    }

    public async Task LoadEnvironments()
    {
        string token = PlayerPrefs.GetString("UserToken", "");
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("❌ User token missing! Login first.");
            feedbackText.text = "❌ Please log in first.";
            return;
        }

        apiClient.SetAccessToken(token);

        try
        {
            var environmentIds = await apiClient.GetAsync<string[]>($"{GetEnvironmentIdsEndpoint}");

            if (environmentIds == null || environmentIds.Length == 0)
            {
                Debug.LogWarning("⚠️ No environments found for this user.");
                feedbackText.text = "No environments available.";
                return;
            }

            PopulateEnvironmentButtons(environmentIds);
            feedbackText.text = $"✅ {environmentIds.Length} environments loaded!";
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Failed to load environments: {ex.Message}");
            feedbackText.text = "❌ Failed to load environments!";
        }
    }

    private void PopulateEnvironmentButtons(string[] environmentIds)
    {
        foreach (Transform child in environmentListContainer)
        {
            Destroy(child.gameObject); // ✅ Clear old buttons
        }

        float yOffset = 0f;
        float spacing = 50f;

        foreach (var envId in environmentIds)
        {
            GameObject button = Instantiate(environmentButtonPrefab, environmentListContainer);
            var text = button.GetComponentInChildren<TMP_Text>();
            if (text != null)
                text.text = $"Environment ID: {envId}";

            var rect = button.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-200f, -yOffset);

            yOffset += spacing;

            button.GetComponent<Button>().onClick.AddListener(() => OpenEnvironment(envId));
        }
    }

    private void OpenEnvironment(string environmentId)
    {
        Debug.Log($"🌍 Selected environment: {environmentId}");
        PlayerPrefs.SetString("SelectedEnvironmentID", environmentId);
        PlayerPrefs.Save();

        feedbackText.text = "Loading environment...";

        SceneManager.LoadScene("Game");
    }
}
