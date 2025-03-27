using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_WorkScene : MonoBehaviour
{
    #region Component
    [HideInInspector]
    public static GameManager_WorkScene Instance { get; private set; }
    private PlayerController_WorkScene playerController;

    [Header("Ui")]
    public GameObject pausePanel;

    [Header("Check Point")]
    public Checkpoint_Data_WorkScene[] checkpointConditions;
    [HideInInspector] public Vector2 checkPointPos;

    [Header("Animator")]
    [SerializeField] private Animator checkpointAnim;
    #endregion

    #region MonoBehaviour func
    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance.checkpointConditions = this.checkpointConditions;
            Destroy(gameObject);
        }
        #endregion
    }

    private void Start()
    {
        checkPointPos = transform.position;

        playerController = FindObjectOfType<PlayerController_WorkScene>();
        checkpointAnim = GameObject.FindWithTag("Main Checkpoint").GetComponent<Animator>();
    }

    #endregion

    #region Paused Menu
    public void ManiMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }

    public void PausedGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        FindObjectOfType<AudioSource>().Pause();
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        FindObjectOfType<AudioSource>().Play(); // make sure Audio is always playing when stop Paused
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        FindObjectOfType<AudioSource>().Play(); // make sure Audio is always playing when stop Paused
    }
    #endregion

    #region Checkpoint Management
    public void ChangeScene(GameObject player)
    {
        Inventory playerInventory = player.GetComponent<Inventory>();

        #region debug
        /*if (playerInventory == null)
        {
            Debug.LogError("no invetory");
            return;
        }

        if (Instance.checkpointConditions == null || Instance.checkpointConditions.Length == 0)
        {
            Debug.LogError("Checkpoint conditions are not initialized!");
            return;
        }*/
        #endregion

        string currentSceneName = SceneManager.GetActiveScene().name;
        foreach (Checkpoint_Data_WorkScene condition in Instance.checkpointConditions)
        {
            if (condition.sceneName == currentSceneName)
            {
                if (playerInventory.moneyCount >= condition.requiredItems)
                {
                    UnlockedNextLevel();
                    StartCoroutine(LoadNextScene(condition.nextSceneName));
                }
                else
                {
                    Debug.Log($"Not enough items: {playerInventory.moneyCount}/{condition.requiredItems}");
                }
                return;
            }
        }
        
    }

    private IEnumerator LoadNextScene(string nextSceneName)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.checkpointSFX);
            checkpointAnim.Play("Main_Checkpoint_Anim");

            // length may not correct so need a small time before get layer info
            yield return null;
            // Get state information of animation at layer 0 (Cause make sure always start at first sprite) 
            AnimatorStateInfo checkPointStateInfo = checkpointAnim.GetCurrentAnimatorStateInfo(0);

            // player can't move when animation is loading scene
            playerController.playerRb.simulated = false;
            yield return new WaitForSeconds(checkPointStateInfo.length);

            SceneManager.LoadScene(nextSceneName);
            playerController.playerRb.simulated = true;
        }
        else
        {
            Debug.LogWarning("No scene");
        }
    }

    private void UnlockedNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
    #endregion

}

