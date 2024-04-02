using UnityEngine;

[CreateAssetMenu]
public class Prompt : ScriptableObject
{
    [TextArea] public string promptText;
    [Space] 
    public bool queueOnStart;
    public bool canBeCompletedEarly;
}