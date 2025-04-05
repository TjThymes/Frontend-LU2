using UnityEngine;
using UnityEngine.SceneManagement;

public class PageCoordinatorGame : MonoBehaviour
{
    SceneManager sceneManager;
    private PageCoordinator pageCoordinator; // ✅ Declare as an instance variable


    public void ShowEnvironments()
    {
        SceneManager.LoadScene("Home");
        pageCoordinator.ShowEnvironmets(); // ✅ Call on the instance, not the class
    }
}
