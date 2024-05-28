using UnityEngine;

public class CreditsMusic : MonoBehaviour
{
    public AudioClip creditsMusic;

    void Start()
    {
        // Aseg�rate de que el componente AudioSource est� agregado al GameObject
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = creditsMusic;
        audioSource.loop = true; // Repetir la m�sica en bucle
        audioSource.playOnAwake = true; // Iniciar la reproducci�n al comenzar la escena
        audioSource.Play();
    }
}