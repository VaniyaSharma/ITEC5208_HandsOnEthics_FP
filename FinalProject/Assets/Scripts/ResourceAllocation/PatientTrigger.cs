using UnityEngine;

public class PatientTrigger: MonoBehaviour 
{
    public string patientLabel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vial"))
        {
            Debug.Log($"Vial given to: {patientLabel}");
            ChoiceManager.Instance.RegisterChoice(patientLabel);
        }
    }
}
