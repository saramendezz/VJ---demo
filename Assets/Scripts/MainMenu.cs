using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Asegúrate de añadir esta línea

public class MainMenu : MonoBehaviour
{
    public AudioClip startGameSound; // Nuevo AudioClip para el sonido al iniciar el juego
    private AudioSource audioSource; // Referencia al AudioSource
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);

        // Asegúrate de que el componente AudioSource esté agregado al GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void startGame()
    {
        playerMovement.startGame();
        transform.GetChild(1).gameObject.SetActive(false);

        // Inicia la corrutina para reproducir el sonido después de un retraso
        StartCoroutine(PlaySoundWithDelay(4f));
    }

    public void startGameFromPlayer()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);

        // Inicia la corrutina para reproducir el sonido después de un retraso
        StartCoroutine(PlaySoundWithDelay(4.5f));
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

    public void closeCredits()
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
        SceneManager.LoadScene(1); // Cambia a la escena con el índice 1
    }

    public void goBack()
    {
        SceneManager.LoadScene(0);
    }

    // Método para reproducir un sonido después de un retraso
    private IEnumerator PlaySoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(startGameSound);
    }

    // Método para reproducir un sonido
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
