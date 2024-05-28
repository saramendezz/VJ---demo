using UnityEngine;

public class MusicMenu : MonoBehaviour
{
    public AudioClip mainMenuMusic;
    public AudioClip runingMusic;
    public AudioClip hitSound;
    private AudioSource mainSource;

    void Start()
    {
        mainSource = gameObject.AddComponent<AudioSource>();
        mainSource.clip = mainMenuMusic;
        mainSource.loop = true;
        mainSource.Play();
    }

    public void startRuningMusic()
    {
        mainSource.clip = runingMusic;
        mainSource.Play();
    }

    public void startDieMusic()
    {
        mainSource.clip = hitSound;
        mainSource.time = 0.5f;
        mainSource.Play();
    }


}
