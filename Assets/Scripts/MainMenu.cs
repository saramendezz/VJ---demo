using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Asegúrate de añadir esta línea

public class MainMenu : MonoBehaviour
{
    PlayerMovement playerMovement;
    public int score, ctrSlowedTimes, scoreTime;
    public TextMeshProUGUI slowedTimes;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTimeText;
    public MusicMenu musicMenu;
    public AudioClip soundFx;
    public AudioClip startBtn;

    private AudioSource soundPlayer;

    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        score = 0;
        ctrSlowedTimes = 0;
        scoreTime = 0;
        slowedTimes.text = "Times Slowed: " + ctrSlowedTimes;
        slowedTimes.color = Color.gray;
        scoreText.text = "Coins: " + score;
        scoreTimeText.text = "Time Score: " + scoreTime;

        soundPlayer = gameObject.AddComponent<AudioSource>();
        soundPlayer.volume = 1.0f;
        soundPlayer.clip = soundFx;
        soundPlayer.time = 1.5f;
        soundPlayer.loop = false;
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
        scoreText.text = "Coins: " + score;

    }

    public void incrementScoreTime(int incrm)
    {
        scoreTime += incrm;
        scoreTimeText.text = "Time Score: " + incrm;
    }

    public void startGame()
    {
        soundPlayer.clip = startBtn;
        soundPlayer.loop = false;
        soundPlayer.Play();
        musicMenu.startRuningMusic();
        playerMovement.startGame();
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void startGameFromPlayer()
    {
        soundPlayer.clip = startBtn;
        soundPlayer.loop = false;
        soundPlayer.Play();
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
    }

    public void openCredits()
    {
        soundPlayer.Play();
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }
    public void openControls()
    {
        soundPlayer.Play();
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
    }
    public void openInstructions()
    {
        soundPlayer.Play();
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(true);
    }

    public void closeControls()
    {
        soundPlayer.Play();
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(false);
    }


    public void closeInstructions()
    {
        soundPlayer.Play();
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(false);
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
