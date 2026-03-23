using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabChoice: MonoBehaviour
{
    public string choiceLabel;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        GetComponent<Animator>().SetTrigger("IsGrabbed");
        Debug.Log($"Player chose: {choiceLabel}");
        ChoiceManager.Instance.RegisterChoice(choiceLabel);
    }

}

