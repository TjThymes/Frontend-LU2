using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using WebApiClient.ApiClient;
using Assets.Scripts.Models;

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
    public async void Reload()
    {
        feedbackText.text = "Reloading environments...";
        LoadEnvironments();
    }
    public async Task LoadEnvironments()
    {
        string token = PlayerPrefs.GetString("UserToken", "");

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("❌ Token or UserId missing!");
            feedbackText.text = "❌ Please log in first.";
            return;
        }

        apiClient.SetAccessToken(token);

        try
        {
            var userId = PlayerPrefs.GetString("UserId", "");
            var environments = await apiClient.GetAsync<EnvironmentResponse[]>($"{GetEnvironmentIdsEndpoint}?userId={userId}");

            if (environments == null || environments.Length == 0)
            {
                Debug.LogWarning("⚠️ No environments found for this user.");
                feedbackText.text = "No environments available.";
                PopulateEnvironmentButtons(environments);
                return;
            }

            PopulateEnvironmentButtons(environments);
            feedbackText.text = $"✅ {environments.Length} environments loaded!";
        }
        catch (Exception ex)
        {
        }
    }


    private void PopulateEnvironmentButtons(EnvironmentResponse[] environments)
    {
        foreach (Transform child in environmentListContainer)
        {
            Destroy(child.gameObject); // ✅ Clear old buttons
        }

        float yOffset = 1f;
        float spacing = 10f;

        foreach (var env in environments)
        {
            GameObject button = Instantiate(environmentButtonPrefab, environmentListContainer);
            var text = button.GetComponentInChildren<TMP_Text>();
            if (text != null)
                text.text = env.name; // ✅ SHOW THE NAME!

            var rect = button.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-200f, -yOffset);

            yOffset += spacing;

            button.GetComponent<Button>().onClick.AddListener(() => OpenEnvironment(env.id));
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
