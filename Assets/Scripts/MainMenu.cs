using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Aseg�rate de a�adir esta l�nea

public class MainMenu : MonoBehaviour
{
    PlayerMovement playerMovement;
    public int score, ctrSlowedTimes;
    public TextMeshProUGUI slowedTimes;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        score = 0;
        ctrSlowedTimes = 0;
        slowedTimes.text = "Times Slowed: " + ctrSlowedTimes;
        slowedTimes.color = Color.gray;
        scoreText.text = "Score: " + score;
    }

    public void incrementSlow()
    {
        ++ctrSlowedTimes;
        slowedTimes.text = "Times Slowed:" + ctrSlowedTimes;
        if (ctrSlowedTimes == 3) playerMovement.Die();
        else if (ctrSlowedTimes == 2) slowedTimes.color = Color.red;
        else if (ctrSlowedTimes == 1) slowedTimes.color = Color.yellow;
    }
    public void incrementScore()
    {
        ++score;
        scoreText.text = "Score: " + score;

    }

    public void startGame()
    {
        playerMovement.startGame();
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void startGameFromPlayer()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
    }

    public void openCredits()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }
    public void openControls()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
    }

    public void closeControls()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(false);
    }

    public void setGodMode()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
    }
    public void exitGodMode()
    {
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void setSlowed()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
    }
    public void exitSlowed()
    {
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void goBack() 
    {
    }
}
