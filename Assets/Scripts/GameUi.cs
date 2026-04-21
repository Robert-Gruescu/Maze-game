using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Ataseaza pe un GameObject gol "GameUI" in scena de joc (SampleScene)
public class GameUI : MonoBehaviour
{
    [Header("Referinte")]
    public MazeGenerator mazeGenerator;

    [Header("Panouri")]
    public GameObject hudPanel;        // HUD normal (buton New Maze etc.)
    public GameObject pausePanel;      // Panou de pauza (optional)

    [Header("Nume scena meniu")]
    public string menuSceneName = "MainMenu";

    private bool isPaused = false;

    private void Start()
    {
        // Citeste dimensiunea salvata din meniu si o aplica
        if (mazeGenerator != null)
        {
            int savedSize = PlayerPrefs.GetInt("MazeSize", 11);
            // Forteaza impar
            if (savedSize % 2 == 0) savedSize++;
            mazeGenerator.width = savedSize;
            mazeGenerator.height = savedSize;
        }

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void Update()
    {
        // Escape = pauza
        if (UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    // ── Butoane HUD ──

    public void OnGenerateMapPressed()
    {
        if (mazeGenerator != null)
            mazeGenerator.GenerateNewMaze();
    }

    // ── Butoane Pause ──

    public void OnResumePressed()
    {
        TogglePause();
    }

    public void OnQuitToMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    public void OnQuitGamePressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ── Helpers ──

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        // Opreste/porneste fizica
        Time.timeScale = isPaused ? 0f : 1f;

        // Cursor vizibil in pauza
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.None;
        Cursor.visible = true;
    }
}