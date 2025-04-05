using UnityEngine;
using TMPro;

public class PageCoordinator : MonoBehaviour
{
    [Header("Pages")]
    public GameObject startPage;
    public GameObject loginPage;
    public GameObject registerPage;
    public GameObject MakeEnvironment;
    public GameObject Environments;


    private void Start()
    {
        ShowStartPage();
    }

    public void ShowStartPage()
    {
        startPage.SetActive(true);
        loginPage.SetActive(false);
        registerPage.SetActive(false);
        MakeEnvironment.SetActive(false);
        Environments.SetActive(false);

    }

    public void ShowLoginPage()
    {
        startPage.SetActive(false);
        loginPage.SetActive(true);
        registerPage.SetActive(false);
    }

    public void ShowRegisterPage()
    {
        startPage.SetActive(false);
        loginPage.SetActive(false);
        registerPage.SetActive(true);
        

    }

    public void ShowMakeEnvironment()
    {
        startPage.SetActive(false);
        loginPage.SetActive(false);
        registerPage.SetActive(false);
        MakeEnvironment.SetActive(true);
        Environments.SetActive(false);
    }
    public void ShowEnvironmets()
    {
        startPage.SetActive(false);
        loginPage.SetActive(false);
        registerPage.SetActive(false);
        MakeEnvironment.SetActive(false);
        Environments.SetActive(true);
    }
    public void QuitApplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
  
}

