using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoader", menuName = "SceneLoader")]
public class SceneLoader : ScriptableObject {
    public SceneReference mainMenuScene;
    public SceneReference tutorialScene;
    public SceneReference gameScene;

    public void DebugTest(string testString) {
        Debug.Log("test " + testString + " at " + Time.time);
    }

    public void LoadTutorial() {
        Time.timeScale = 1;
        SceneManager.LoadScene(tutorialScene.ScenePath);
    }

    public void LoadGameScene() {
        Time.timeScale = 1;
        SceneManager.LoadScene(gameScene.ScenePath);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene.ScenePath);
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}