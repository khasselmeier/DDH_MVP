using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
            //Debug.Log("MusicManager instance created");
        }
        else
        {
            //Debug.Log("Another MusicManager instance destroyed");
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            //Debug.Log("Playing music");
        }
    }
}