using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialDialoguesData dialogue;
    
    public void TriggerInstructor()
    {
        TutorialDialogues.Instance.StartInstructorPlayer(dialogue);
    }
}
