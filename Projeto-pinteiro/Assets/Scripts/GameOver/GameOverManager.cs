using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    public GameObject gameOverUI;
    public static bool isGameOver = false;

    public void ShowGameOver() {

        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        isGameOver = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartPhase() {

        Time.timeScale = 1f;
        isGameOver = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoBackToMainMenu() {

        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}
