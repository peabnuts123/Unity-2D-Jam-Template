using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadSceneByPath(string scenePath)
    {
        SceneManager.LoadScene(scenePath);
    }
}