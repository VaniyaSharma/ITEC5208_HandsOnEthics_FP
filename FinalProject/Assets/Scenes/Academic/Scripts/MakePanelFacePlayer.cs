using UnityEngine;

public class MakePlayerFacePlayer: MonoBehaviour
{
    public Transform targetCamera;
    public bool lockX = false;
    public bool lockY = false;
    public bool lockZ = false;

    private void LateUpdate()
    {
        if (targetCamera == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                targetCamera = cam.transform;
        }

        if (targetCamera == null) return;

        Vector3 direction = transform.position - targetCamera.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 euler = lookRotation.eulerAngles;

        if (lockX) euler.x = transform.rotation.eulerAngles.x;
        if (lockY) euler.y = transform.rotation.eulerAngles.y;
        if (lockZ) euler.z = transform.rotation.eulerAngles.z;

        transform.rotation = Quaternion.Euler(euler);
    }
}