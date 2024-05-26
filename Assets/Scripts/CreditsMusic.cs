using UnityEngine;

public class CreditsMusic : MonoBehaviour
{
    public AudioClip creditsMusic;

    void Start()
    {
        // Asegúrate de que el componente AudioSource esté agregado al GameObject
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = creditsMusic;
        audioSource.loop = true; // Repetir la música en bucle
        audioSource.playOnAwake = true; // Iniciar la reproducción al comenzar la escena
        audioSource.Play();
    }
}
