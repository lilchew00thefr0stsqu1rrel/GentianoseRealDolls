using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class MusicChange : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource> ();

        SetDefaultMusic();
    }

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private int defaultClipID;

    public void SetMusic(int musicID)
    {
        audioSource.clip = clips[musicID];
        audioSource.Play ();
    }
    public void SetDefaultMusic()
    {
        if (defaultClipID >= 0)
        {
            audioSource.clip = clips[defaultClipID];
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
