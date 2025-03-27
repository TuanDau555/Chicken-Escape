using UnityEngine;

[CreateAssetMenu(fileName = "Checkpoint Data", menuName = "Checkpoint/SceneCondition")]
public class Checkpoint_Data_WorkScene : ScriptableObject
{
    public string sceneName;
    public string nextSceneName;
    public int requiredItems;
}
