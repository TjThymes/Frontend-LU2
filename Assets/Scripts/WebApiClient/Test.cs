//using UnityEngine;

//public class Test : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        RegisterModel newUser = new RegisterModel
//        {
//            Name = "TestUser",
//            Email = "test@example.com",
//            Password = "SterkWachtwoord1!"
//        };

//        ApiManager apiManager = GetComponent<ApiManager>();
//        if (apiManager != null)
//        {
//            StartCoroutine(apiManager.RegisterUser(newUser, (success, message) =>
//            {
//                if (success)
//                    Debug.Log("Registratie gelukt: " + message);
//                else
//                    Debug.LogError("Fout bij registratie: " + message);
//            }));
//        }
//        else
//        {
//            Debug.LogError("ApiManager component not found.");
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
