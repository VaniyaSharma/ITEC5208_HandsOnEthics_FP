using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SceneManager : MonoBehaviour
{
    [Header("Intro")]
    public GameObject entranceIntroPanel;

    [Header("Helper Panels")]
    public GameObject friendDeskPanel;
    public GameObject teacherTrayLabelPanel;
    public GameObject bagLabelPanel;

    [Header("Outcome Panels")]
    public GameObject teacherThankYouPanel;
    public GameObject bagDecisionPanel;

    [Header("Audio")]
    public AudioSource bellAudio;

    [Header("Exam Paper")]
    public GameObject examPaper;
    public XRGrabInteractable examPaperGrab;

    private bool experienceStarted = false;
    private bool decisionMade = false;

    public bool ExperienceStarted => experienceStarted;
    public bool DecisionMade => decisionMade;

    public void BeginExperience()
    {
        if (experienceStarted) return;
        experienceStarted = true;

        // Hide intro panel
        if (entranceIntroPanel != null)
            entranceIntroPanel.SetActive(false);

        // Play bell audio
        if (bellAudio != null)
            bellAudio.Play();

        // Activate helper panels
        if (friendDeskPanel != null)
            friendDeskPanel.SetActive(true);

        if (teacherTrayLabelPanel != null)
            teacherTrayLabelPanel.SetActive(true);

        if (bagLabelPanel != null)
            bagLabelPanel.SetActive(true);

        // Make sure outcome panels are off
        if (teacherThankYouPanel != null)
            teacherThankYouPanel.SetActive(false);

        if (bagDecisionPanel != null)
            bagDecisionPanel.SetActive(false);

        // Enable grabbing if disabled at start
        if (examPaperGrab != null)
            examPaperGrab.enabled = true;

        Debug.Log("Experience started.");
    }

    public void FinalizeChoice(bool reported, GameObject paper, DecisionZone zone)
    {
        if (!experienceStarted || decisionMade) return;
        if (paper == null || zone == null) return;

        decisionMade = true;

        Rigidbody rb = paper.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        XRGrabInteractable grab = paper.GetComponent<XRGrabInteractable>();
        if (grab != null)
            grab.enabled = false;

        // Hide helper panels once choice is made
        if (friendDeskPanel != null)
            friendDeskPanel.SetActive(false);

        if (teacherTrayLabelPanel != null)
            teacherTrayLabelPanel.SetActive(false);

        if (bagLabelPanel != null)
            bagLabelPanel.SetActive(false);

        // Outcome
        if (reported)
        {
            if (zone.snapPoint != null)
            {
                paper.transform.SetPositionAndRotation(zone.snapPoint.position, zone.snapPoint.rotation);
            }

            if (teacherThankYouPanel != null)
                teacherThankYouPanel.SetActive(true);

            Debug.Log("Choice recorded: Reported to teacher.");
        }
        else
        {
            // Hide the paper to show that it was placed inside the bag
            paper.SetActive(false);

            if (bagDecisionPanel != null)
                bagDecisionPanel.SetActive(true);

            Debug.Log("Choice recorded: Kept in bag.");
        }

        zone.ResetLabel();
    }
}