using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{

    public UnityEvent interaction;
    public KeyCode interactedKey;

    [SerializeField] private SpriteRenderer notifycation;
    
    private bool isInteractable;

    private void Start()
    {
        notifycation = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(interactedKey))        // Press E Button to start action
            {
                interaction.Invoke(); 
            }
        }
    }


    // Khi Player bước vào một Collider2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            if (!isInteractable)
            {
                isInteractable = true;
                interactShowedKey();                
            }
        }
    }

    // Khi Player rời khỏi một Collider2D
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            isInteractable = false;
            interactShowedKeyDeactive();
            //Close Dialogue Box
            TutorialDialogues.Instance.EndDialogue();

        }
    }
   
    private void interactShowedKey()
    {
        notifycation.enabled = true;
    }
    private void interactShowedKeyDeactive()
    {
        notifycation.enabled = false;
    }  
}
