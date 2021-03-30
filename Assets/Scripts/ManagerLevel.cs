using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerLevel : MonoBehaviour
{
    public static void PlayNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(sceneIndex + 1);
    }
}