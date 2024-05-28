using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;
    public AudioClip coinSound; // Nuevo AudioClip para el sonido de la moneda

    private void OnTriggerEnter(Collider other)
    {
        // Check if we collide with the player
        if (other.gameObject.name != "Player")
        {
            return;
        }

        // Add to the player's score
        GameManager.inst.IncrementScore();

        // Reproduce el sonido y espera antes de destruir la moneda
        StartCoroutine(PlaySoundAndDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }

    // Corrutina para reproducir el sonido y destruir la moneda después de un segundo
    private IEnumerator PlaySoundAndDestroy()
    {
        // Crear un nuevo GameObject para reproducir el sonido
        GameObject tempAudioSource = new GameObject("TempAudio");
        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.clip = coinSound;
        audioSource.Play();

        // Destruir la moneda inmediatamente
        Destroy(gameObject);

        // Esperar hasta que el sonido termine de reproducirse
        yield return new WaitForSeconds(audioSource.clip.length);

        // Destruir el GameObject temporal
        Destroy(tempAudioSource);
    }
}
