using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void startGame()
    {
        playerMovement.startGame();
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void startGameFromPlayer()
    {
        transform.GetChild(1).gameObject.SetActive(false);
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
}
