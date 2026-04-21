// ===== WinMenu.cs =====
// Ataseaza pe un GameObject gol "WinManager" in SampleScene

using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [Header("Referinte")]
    public GameObject winCanvas;          // Drag WinCanvas aici
    public MazeGenerator mazeGenerator;   // Drag GameManager aici

    private void Start()
    {
        if (winCanvas != null)
            winCanvas.SetActive(false);
    }

    public void ShowWinScreen()
    {
        if (winCanvas != null)
            winCanvas.SetActive(true);

        // Deblocheaza cursorul
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Opreste miscarea playerului
        Time.timeScale = 0f;
    }

    // Butonul PLAY AGAIN
    public void OnPlayAgainPressed()
    {
        Time.timeScale = 1f;

        if (winCanvas != null)
            winCanvas.SetActive(false);

        if (mazeGenerator != null)
            mazeGenerator.GenerateNewMaze();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    // Butonul QUIT
    public void OnQuitPressed()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}