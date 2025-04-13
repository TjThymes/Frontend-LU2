using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace WebApiClient.ApiClient
{
    // 🔹 **API Client Class**
    // This class handles API requests and responses.
    // It uses UnityWebRequest for making HTTP calls.
    // It also manages the base URL and access token for authentication.

    public class ApiClient : MonoBehaviour
    {
        private string _baseUrl;
        private string _accessToken;

        public void Initialize(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public void SetAccessToken(string token)
        {
            _accessToken = token;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var url = $"{_baseUrl}{endpoint}";
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                if (!string.IsNullOrEmpty(_accessToken))
                {
                    request.SetRequestHeader("Authorization", $"Bearer {_accessToken}");
                }

                request.SetRequestHeader("Accept", "application/json");

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"GET failed: {request.error}");
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
            }
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string endpoint, TRequest data)
        {
            var request = new UnityWebRequest(_baseUrl + endpoint, "POST");

            var jsonData = JsonConvert.SerializeObject(data); // ✅ Hier aangepast!
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(_accessToken))
                request.SetRequestHeader("Authorization", "Bearer " + _accessToken);

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception(request.error);
            }

            if (string.IsNullOrWhiteSpace(request.downloadHandler.text))
            {
                return default(TResult)!;
            }

            return JsonConvert.DeserializeObject<TResult>(request.downloadHandler.text);
        }


        public async Task DeleteAsync(string endpoint)
        {
            using (UnityWebRequest request = UnityWebRequest.Delete(_baseUrl + endpoint))
            {
                request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("UserToken"));
                request.SetRequestHeader("Content-Type", "application/json");

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"❌ API DELETE Failed: {request.error}");
                }
                else
                {
                    Debug.Log("✅ DELETE Successful: " + endpoint);
                }
            }
        }
        public async Task PutAsync<T>(string endpoint, T body)
        {
            var json = JsonUtility.ToJson(body);
            using (UnityWebRequest request = new UnityWebRequest(_baseUrl + endpoint, "PUT"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("UserToken"));
                request.SetRequestHeader("Content-Type", "application/json");

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                    throw new System.Exception($"PUT failed: {request.error}");
                else
                    Debug.Log($"✅ PUT Success at endpoint: {endpoint}");
            }
        }
    }
}
