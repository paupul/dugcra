using UnityEngine;
using UnityEngine.SceneManagement;

public class Change_scene : MonoBehaviour
{
    public void ChangeToScene(int sceneToChangeTo)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToChangeTo);
    }

    public static void ChangeToLevel(int levelScene, string levelName, bool random)
    {
        LevelManager.levelName = levelName;
        LevelManager.isRandom = random;

        Time.timeScale = 1;
        SceneManager.LoadScene(levelScene);
    }

    public void NextLevel()
    {
        try
        {
            if (LevelManager.levelIndex + 1 < LevelManager.levels.Count)
            {
                LevelManager.levelName = LevelManager.levels[LevelManager.levelIndex + 1];
            }

            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }
        catch (System.Exception)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
            throw;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
