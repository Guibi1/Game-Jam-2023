using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private void Update()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        Vector3 right = transform.right;
        right.y = 0f;

        if(PlayerMouvement.instance != null)
        PlayerMouvement.instance.SetCameraVectors(forward, right);
    }
}
