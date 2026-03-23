using UnityEngine;

public class InstructionsManager: MonoBehaviour
{
    public GameObject instructionsCanvas;
    
    public void onBeginClick()
    {
        instructionsCanvas.SetActive(false);
    }
}
