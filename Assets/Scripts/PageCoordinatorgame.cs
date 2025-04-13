using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageCoordinatorGame : MonoBehaviour
{
    SceneManager sceneManager;
    private PageCoordinator pageCoordinator;
    public GameObject Game;
    public GameObject Controls;


    public void ShowEnvironments()
    {
        SceneManager.LoadScene("Home");
        pageCoordinator.ShowEnvironmets();
    }
    public void ShowGame()
    {
        Controls.SetActive(false);
        Game.SetActive(true);

    }
    public void ShowControls()
    {
        Game.SetActive(false);
        Controls.SetActive(true);
    }
}
