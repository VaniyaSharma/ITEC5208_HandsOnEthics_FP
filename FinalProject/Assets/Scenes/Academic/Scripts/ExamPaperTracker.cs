using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ExamPaperTracker : MonoBehaviour
{
    [Header("Manager")]
    public SceneManager manager;

    [HideInInspector] public DecisionZone currentZone;

    private XRGrabInteractable grabInteractable;
    private bool held = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        held = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        held = false;

        if (manager == null) return;
        if (!manager.ExperienceStarted || manager.DecisionMade) return;

        if (currentZone != null)
        {
            manager.FinalizeChoice(currentZone.reportChoice, gameObject, currentZone);
        }
    }

    public bool IsHeld()
    {
        return held;
    }
}