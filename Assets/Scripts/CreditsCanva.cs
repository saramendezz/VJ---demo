using UnityEngine;
using UnityEngine.SceneManagement; // Aseg�rate de a�adir esta l�nea

public class CreditsCanva : MonoBehaviour
{
    public AudioClip startGameSound; // Nuevo AudioClip para el sonido al iniciar el juego
    private AudioSource audioSource; // Referencia al AudioSource

    private void Start()
    {
        // Aseg�rate de que el componente AudioSource est� agregado al GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void closeCredits()
    {
        SceneManager.LoadScene(0);
    }
}
