using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RaceTimer : MonoBehaviour
{
    public Text timerText;
    public Text bestTimeText;
    
    public GameObject resultsPanel;
    public Text resultsText;
    
    private float startTime;
    private bool finished = false;
    private float bestTime;
    
    private void Start()
    {
        // PlayerPrefs.SetFloat("BestTime", float.MaxValue);
        resultsPanel?.SetActive(false);

        startTime = Time.time;
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        UpdateBestTimeText();
    }

    private void Update()
    {
        if (finished) return;

        float currentTime = Time.time - startTime;
        UpdateTimerText(currentTime);
    }

    public void FinishRace()
    {
        finished = true;
        float finalTime = Time.time - startTime;
        UpdateTimerText(finalTime);
        
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
        }

        if (resultsText != null)
        {
            resultsText.text = "Finished! Total time: " + finalTime.ToString("F2") + " seconds";
        }
        
        if (finalTime < bestTime)
        {
            bestTime = finalTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            UpdateBestTimeText();
        }
        
        StartCoroutine(ReturnToMainMenuAfterDelay(5f));
    }

    private void UpdateTimerText(float time)
    {
        timerText.text = time.ToString("F2");
    }

    private void UpdateBestTimeText()
    {
        if (bestTime != float.MaxValue)
        {
            bestTimeText.text = bestTime.ToString("F2");
        }
        else
        {
            bestTimeText.text = "N/A";
        }
    }
    private IEnumerator ReturnToMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 在这里加载主菜单场景
        SceneManager.LoadScene("MenuScene");
    }
}