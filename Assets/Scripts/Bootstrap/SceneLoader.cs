using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject cam;
    void Start()
    {
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        Destroy(cam);
    }
}
