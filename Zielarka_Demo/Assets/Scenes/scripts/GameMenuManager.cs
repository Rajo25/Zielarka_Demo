using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject feedbackPanel;
    public GameObject pauseMenuPanel;
    public GameObject creditsPanel; // <-- nowy panel Credits

    [Header("Options")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Dropdown languageDropdown;

    [Header("Feedback")]
    public InputField feedbackInput;

    private bool isPaused = false;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        languageDropdown.value = PlayerPrefs.GetInt("Language", 0);
        UpdateAudio();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    #region Main Menu

    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); 
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Loaded: " + data.playerPosition);
        }
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void OpenFeedback()
    {
        feedbackPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit(); // zamyka grę w buildzie
    }

    #endregion

    #region Pause Menu

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    #region Options

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        UpdateAudio();
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        UpdateAudio();
    }

    public void SetLanguage(int index)
    {
        PlayerPrefs.SetInt("Language", index);
        Debug.Log("Language changed to index: " + index);
    }

    private void UpdateAudio()
    {
        AudioListener.volume = musicSlider.value; 
    }

    #endregion

    #region Feedback

    public void SubmitFeedback()
    {
        string feedback = feedbackInput.text;
        if(!string.IsNullOrEmpty(feedback))
        {
            string path = Application.persistentDataPath + "/feedback.txt";
            File.AppendAllText(path, feedback + "\n---\n");
            feedbackInput.text = "";
            Debug.Log("Feedback submitted");
        }
    }

    public void CloseFeedback()
    {
        feedbackPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    #endregion

    #region Save System

    public void SaveGame(Vector3 playerPos)
    {
        GameData data = new GameData();
        data.playerPosition = playerPos;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        Debug.Log("Game Saved!");
    }

    #endregion
}

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
}