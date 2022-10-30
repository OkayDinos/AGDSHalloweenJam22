using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();

    void Awake()
    {
        if (SoundManager.instance)
        {
            Destroy(this);
            Destroy(gameObject);
        }
        else
            SoundManager.instance = this;

        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayFootstepsSound()
    {
        PlaySounds();
    }

    async void PlaySounds()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        foreach (AudioClip clip in audioClips)
        {
            audioSource.clip = clip;
            audioSource.Play();

            while (audioSource.isPlaying)
            {
                await System.Threading.Tasks.Task.Yield();

                Debug.Log("Playing sound");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
