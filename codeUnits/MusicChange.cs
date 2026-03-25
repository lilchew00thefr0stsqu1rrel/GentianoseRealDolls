using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class MusicChange : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource> ();


    }

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private int defaultClipID;

    public void SetMusic(int musicID)
    {
        audioSource.clip = clips[musicID];
        audioSource.Play ();
    }
    public void SetDefaultMusic(int musicID)
    {
        audioSource.clip = clips[defaultClipID];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
