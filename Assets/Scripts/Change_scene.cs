using UnityEngine;
using UnityEngine.SceneManagement;

public class Change_scene : MonoBehaviour {

	public void ChangeToScene (int sceneToChangeTo) {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToChangeTo);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
