using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [HideInInspector]
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Background Audio")]
    public AudioClip[] levelMusic;

    [Header("SFX")]
    public AudioClip jumpSFX;
    public AudioClip collectMoney;
    public AudioClip checkpointSFX;
    public AudioClip playerDeath;

    private int currentSceneIndex = -1; // Save the previous level to avoid constantly changing music
    private int currentMusicIndex = -1; // Save the state of the song is playing

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (backgroundSource == null)
            {
                backgroundSource = gameObject.AddComponent<AudioSource>();
            }

            PlayBackground();
            SceneManager.sceneLoaded += OnSceneLoaded; // Just want to delegate one time 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PlayBackground()
    {
        backgroundSource.loop = true;
        backgroundSource.playOnAwake = false;
        backgroundSource.volume = 0.4f;

        PlayMusicForCurrentLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlaySFX(AudioClip SFXClip)
    {
        SFXSource.PlayOneShot(SFXClip,0.5f);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Play Audio When load Scene
        PlayMusicForCurrentLevel(scene.buildIndex);
    }
   
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Check if to change the audio or not
    /// </summary>
    private void PlayMusicForCurrentLevel(int sceneIndex)
    {
        if (sceneIndex == currentSceneIndex) return; // if the scene didn't change, no need to change the audio
        currentSceneIndex = sceneIndex; // Update current scene, else dont know what Scene is current showed

        int musicIndex = GetMusicIndexForLevel(sceneIndex);
        if (musicIndex != currentMusicIndex) // only change the audio if there is actually have an new audio
        {
            currentMusicIndex = musicIndex;
            backgroundSource.clip = levelMusic[musicIndex]; // audio clip of current scene
            backgroundSource.Play();
        }
    }

    private int GetMusicIndexForLevel(int levelIndex)
    {
        if (levelIndex == 0) return 1; // Play "Long Road Ahead" in Main Menu
        if (levelIndex == 16) return 2; // Play "World Travelers" in Credit Scene 
        return 0; // default audio is "Journey Across the Blue"
    }

}
