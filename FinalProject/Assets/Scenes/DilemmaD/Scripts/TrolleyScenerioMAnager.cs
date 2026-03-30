using UnityEngine;
using TMPro;
using System.Collections;

public class TrolleyScenarioManager : MonoBehaviour
{
    [Header("Train")]
    public Transform train;
    public Transform trainStart;
    public Transform forkPoint;
    public Transform selfEndPoint;
    public Transform peopleEndPoint;
    public float trainSpeed = 8f;
    public float reachDistance = 0.1f;

    [Header("Result UI")]
    public CanvasGroup resultCanvasGroup;
    public TMP_Text resultText;
    public float fadeDuration = 1.25f;

    [Header("Optional")]
    public Behaviour buttonInteractableToDisable;
    public GameObject buttonPressedVisual;

    [Header("Button Visual")]
    [SerializeField] private Transform buttonModel;
    [SerializeField] private float pressedLocalY = -0.02f;

    [Header("Lock Options")]
    [SerializeField] private bool lockPosition = true;
    [SerializeField] private bool lockRotation = true;

    [Header("Axis Locks")]
    [SerializeField] private bool lockX = true;
    [SerializeField] private bool lockY = true;
    [SerializeField] private bool lockZ = true;

    //[SerializeField] private GameObject startPanel;


    private bool switchedTrack = false;
    private bool scenarioEnded = false;
    private bool passedFork = false;
    public float turnSpeed = 120f;
    private Vector3 lockedPosition;
    private Vector3 lockedEuler;
    //private bool experienceStarted = false;

    private void Start()
    {
        if (train != null && trainStart != null)
        {
            train.position = trainStart.position;
            //train.rotation = trainStart.rotation;
        }
        lockedPosition = transform.position;
        lockedEuler = transform.eulerAngles;

        if (resultCanvasGroup != null)
            resultCanvasGroup.alpha = 0f;

        if (resultText != null)
            resultText.text = "";
    }

    //private void Update()
    //{
    //    if (scenarioEnded || train == null) return;

    //    Transform currentTarget = !passedFork
    //        ? forkPoint
    //        : (switchedTrack ? peopleEndPoint : selfEndPoint);

    //    if (currentTarget == null) return;

    //    Vector3 targetPos = currentTarget.position;
    //    targetPos.y = train.position.y;

    //    train.position = Vector3.MoveTowards(train.position, targetPos, trainSpeed * Time.deltaTime);

    //    if (Vector3.Distance(train.position, targetPos) <= reachDistance)
    //    {
    //        if (!passedFork)
    //        {
    //            passedFork = true;
    //        }
    //        else
    //        {
    //            EndScenario();
    //        }
    //    }
    //}
    private void Update()
    {
        // Auto-start when panel is disabled
        //if (!experienceStarted)
        //{
        //    if (startPanel == null || !startPanel.activeInHierarchy)
        //    {
        //        experienceStarted = true;
        //        Debug.Log("Experience started (panel disabled).");
        //    }
        //}
      

        //if (!experienceStarted) return;
        if (scenarioEnded || train == null) return;

        Transform currentTarget = !passedFork
            ? forkPoint
            : (switchedTrack ? peopleEndPoint : selfEndPoint);

        if (currentTarget == null) return;

        Vector3 targetPos = currentTarget.position;
        targetPos.y = train.position.y;

        // ONLY rotate after the fork IF train is going to self/player track
        if (passedFork && !switchedTrack)
        {
            Quaternion targetRotation = Quaternion.Euler(
                0f,
                selfEndPoint.eulerAngles.y,
                0f
            );

            train.rotation = Quaternion.RotateTowards(
                train.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }

        // Move train
        train.position = Vector3.MoveTowards(
            train.position,
            targetPos,
            trainSpeed * Time.deltaTime
        );

        if (Vector3.Distance(train.position, targetPos) <= reachDistance)
        {
            if (!passedFork)
            {
                passedFork = true;
            }
            else
            {
                EndScenario();
            }
        }

    }
    // Call this from the button's XR Simple Interactable event
    public void PressButton()
    {
        if (scenarioEnded || switchedTrack) return;

        switchedTrack = true;
        Debug.Log("BUTTON PRESSED");

        if (buttonModel != null)
        {
            Vector3 localPos = buttonModel.localPosition;
            localPos.y = pressedLocalY;
            buttonModel.localPosition = localPos;
        }

        if (buttonInteractableToDisable != null)
            buttonInteractableToDisable.enabled = false;

        if (buttonPressedVisual != null)
            buttonPressedVisual.SetActive(true);

        Debug.Log("Button pressed: train switched to people track.");
    }

    private void EndScenario()
    {
        if (scenarioEnded) return;
        scenarioEnded = true;

        if (switchedTrack)
        {
            StartCoroutine(FadeAndShow("Congrats, you're alive."));
        }
        else
        {
            StartCoroutine(FadeAndShow("Congrats, you saved them."));
        }
    }

    private IEnumerator FadeAndShow(string message)
    {
        if (resultText != null)
            resultText.text = message;

        if (resultCanvasGroup == null)
            yield break;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            resultCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        resultCanvasGroup.alpha = 1f;
    }
}