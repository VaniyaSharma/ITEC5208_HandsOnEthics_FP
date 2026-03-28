using UnityEngine;
using TMPro;

public class DecisionZone : MonoBehaviour
{
    [Header("Choice Type")]
    public bool reportChoice; // true = teacher tray, false = bag

    [Header("Optional Snap Point")]
    public Transform snapPoint; // use for tray, leave empty for bag

    [Header("Manager")]
    public SceneManager manager;

    [Header("Label")]
    public TMP_Text labelText;
    [TextArea] public string defaultText = "Place Object";
    [TextArea] public string releaseText = "Release to Place";

    private void Start()
    {
        ResetLabel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ExamPaper")) return;
        if (manager != null && (!manager.ExperienceStarted || manager.DecisionMade)) return;

        ExamPaperTracker tracker = other.GetComponent<ExamPaperTracker>();
        if (tracker == null) return;

        if (tracker.IsHeld())
        {
            tracker.currentZone = this;

            if (labelText != null)
                labelText.text = releaseText;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("ExamPaper")) return;
        if (manager != null && (!manager.ExperienceStarted || manager.DecisionMade)) return;

        ExamPaperTracker tracker = other.GetComponent<ExamPaperTracker>();
        if (tracker == null) return;

        if (tracker.IsHeld())
        {
            tracker.currentZone = this;

            if (labelText != null)
                labelText.text = releaseText;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ExamPaper")) return;

        ExamPaperTracker tracker = other.GetComponent<ExamPaperTracker>();
        if (tracker == null) return;

        if (tracker.currentZone == this)
            tracker.currentZone = null;

        ResetLabel();
    }

    public void ResetLabel()
    {
        if (labelText != null)
            labelText.text = defaultText;
    }
}