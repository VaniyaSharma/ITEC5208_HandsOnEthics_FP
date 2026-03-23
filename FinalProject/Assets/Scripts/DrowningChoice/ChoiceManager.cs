using UnityEngine;

public class ChoiceManager : MonoBehaviour 
{
    public static ChoiceManager Instance;
    void Awake() => Instance = this;

    public void RegisterChoice(string choice)
    {
        Debug.Log($"Choice made: {choice}");
    }
}
