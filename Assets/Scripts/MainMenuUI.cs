using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panouri")]
    public GameObject mainPanel;
    public GameObject optionsPanel;

    [Header("Optiuni - Dimensiune labirint")]
    public Slider mazeSizeSlider;
    public TMP_Text mazeSizeLabel;

    [Header("Nume scena de joc")]
    public string gameSceneName = "SampleScene";

    private void Start()
    {
        ShowMain();

        if (mazeSizeSlider != null)
        {
            mazeSizeSlider.minValue = 11;
            mazeSizeSlider.maxValue = 31;
            mazeSizeSlider.wholeNumbers = true;
            mazeSizeSlider.value = PlayerPrefs.GetInt("MazeSize", 11);
            mazeSizeSlider.onValueChanged.AddListener(OnMazeSizeChanged);
            UpdateMazeSizeLabel((int)mazeSizeSlider.value);
        }
    }

    public void OnStartPressed()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnOptionsPressed()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnBackPressed()
    {
        PlayerPrefs.Save();
        ShowMain();
    }

    private void ShowMain()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    private void OnMazeSizeChanged(float value)
    {
        int size = (int)value;
        if (size % 2 == 0) size++;
        PlayerPrefs.SetInt("MazeSize", size);
        UpdateMazeSizeLabel(size);
    }

    private void UpdateMazeSizeLabel(int size)
    {
        if (mazeSizeLabel != null)
            mazeSizeLabel.text = $"Dimensiune harta: {size} x {size}";
    }
}