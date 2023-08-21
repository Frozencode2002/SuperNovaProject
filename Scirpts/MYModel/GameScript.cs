using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameScript : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the Panel containing the pause menu UI
    public bool isPaused = false;
    public bool PauseMenuflag = false;
    private void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(isPaused);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !PauseMenuflag)
        {
            PauseMenuflag = true;
            TogglePause();
        }
    }
    public void ResumeGame()
    {
        PauseMenuflag = !PauseMenuflag;
        TogglePause();
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Reset time scale to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    public void QuitGame()
    {
        Time.timeScale = 1; // Reset time scale to normal
        SceneManager.LoadScene("MenuScene"); // Replace "MainMenu" with the name of your main menu scene
    }
    public void TogglePause()
    {
        isPaused = !isPaused;

        pauseMenu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0 : 1;
    }
}
