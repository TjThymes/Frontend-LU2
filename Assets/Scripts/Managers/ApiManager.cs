using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using WebApiClient.ApiClient;
using Assets.Scripts.Models;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    public TMP_InputField NameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameInputReg;
    public TMP_InputField emailInputReg;
    public TMP_InputField passwordInputReg;
    public TMP_Text feedbackText;
    public TMP_Text Regisertext;
    public GameObject Environments;
    public GameObject Loginscreen;
    public ApiClient apiClient;

    private void Start()
    {
        apiClient.Initialize("https://avansict2232490.azurewebsites.net/api/");
    }

    public async void Register()
    {
        if (string.IsNullOrWhiteSpace(usernameInputReg.text) ||
            string.IsNullOrWhiteSpace(emailInputReg.text) ||
            string.IsNullOrWhiteSpace(passwordInputReg.text))
        {
            feedbackText.text = "❌ Please fill in all fields!";
            return;
        }

        var newUser = new RegisterModel
        {
            Name = usernameInputReg.text,
            Email = emailInputReg.text,
            Password = passwordInputReg.text
        };

        try
        {
            await apiClient.PostAsync<RegisterModel, object>("auth/register", newUser);
            Regisertext.text = "✅ Registration successful!";
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Registration Error: {ex.Message}");
            Regisertext.text = "❌ Registration failed!";
        }
    }

    public async void Login()
    {
        if (string.IsNullOrWhiteSpace(NameInput.text) || string.IsNullOrWhiteSpace(passwordInput.text))
        {
            feedbackText.text = "❌ Please enter both email and password!";
            return;
        }

        var loginData = new LoginModel
        {
            UserName = NameInput.text,
            Password = passwordInput.text
        };

        try
        {
            var response = await apiClient.PostAsync<LoginModel, AuthResponse>("auth/login", loginData);

            PlayerPrefs.SetString("UserToken", response.accessToken);

            // ✅ Decode userId from the token:
            string userId = JwtDecoder.GetUserIdFromToken(response.accessToken);
            PlayerPrefs.SetString("PlayerID", userId);

            PlayerPrefs.Save();

            Debug.Log($"✅ Token Saved: {response.accessToken} ✅ UserId Decoded: {userId}");


            apiClient.SetAccessToken(response.accessToken);

            Loginscreen.SetActive(false);
            Environments.SetActive(true);
            feedbackText.text = "✅ Login successful!";
            Debug.Log(PlayerPrefs.GetString("UserToken"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Login Error: {ex.Message}");
            feedbackText.text = "❌ Login failed!";
        }
    }

    public async void UserNameValid()
    {
        var username = usernameInputReg.text;

        if (string.IsNullOrWhiteSpace(username))
        {
            Regisertext.text = "❌ Please enter a username!";
            return;
        }

        try
        {
            
            var response = await apiClient.GetAsync<UsernameAvailabilityResponse>($"auth/check-username?username={username}");

            Regisertext.text = response.available
                ? "✅ Username is available!"
                : "❌ Username is already taken!";
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Username check failed: {ex.Message}");
            Regisertext.text = "❌ Error checking username!";
        }
    }
}


    [System.Serializable]
public class UsernameAvailabilityResponse
{
    public bool available;
}
