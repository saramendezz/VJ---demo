using UnityEngine;
using UnityEngine.SceneManagement; // Asegúrate de añadir esta línea

public class CreditsCanva : MonoBehaviour
{
    public AudioClip startGameSound; // Nuevo AudioClip para el sonido al iniciar el juego
    private AudioSource audioSource; // Referencia al AudioSource

    private void Start()
    {
        // Asegúrate de que el componente AudioSource esté agregado al GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void closeCredits()
    {
        SceneManager.LoadScene(0);
    }
}
