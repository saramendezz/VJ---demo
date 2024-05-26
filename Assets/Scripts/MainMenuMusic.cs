using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public AudioClip mainMenuMusic;

    void Start()
    {
        // Asegúrate de que el componente AudioSource esté agregado al GameObject
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = mainMenuMusic;
        audioSource.loop = true; // Repetir la música en bucle
        audioSource.playOnAwake = true; // Iniciar la reproducción al comenzar la escena
        audioSource.Play();
    }
}
