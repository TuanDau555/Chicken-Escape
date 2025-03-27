using UnityEngine;

[System.Serializable]
public class TutorialDialoguesData 
{
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;
}
