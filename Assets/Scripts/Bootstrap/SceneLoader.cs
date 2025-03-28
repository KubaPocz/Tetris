using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject cam;

    private void Start()
    {
        // Load gameplay and UI scenes additively
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);

        // Destroy initial camera (e.g., loading screen cam)
        Destroy(cam);
    }
}
