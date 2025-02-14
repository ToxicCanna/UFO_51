using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 

    [System.Serializable]
    public class Sound
    {
        public string name;  // Name of the sound 
        public AudioClip clip;  // The sound clip
        public float volume = 1f;  // Default volume
        public bool loop;  // Should the sound loop?

        [HideInInspector] public AudioSource source; 
    }

    public Sound[] sounds;  // Array of sounds to manage

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("AudioManager initialized");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); 

        // Initialize all sounds
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        Debug.Log("AudioManager initialized with " + sounds.Length + " sounds.");
    }

    // Play a sound by name
    public void Play(string soundName)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        Debug.Log("Playing sound: " + soundName);
        s.source.Play();
    }

    // Stop a sound by name
    public void Stop(string soundName)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.source.Stop();
    }
}
