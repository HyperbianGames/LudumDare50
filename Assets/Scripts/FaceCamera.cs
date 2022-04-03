using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (CameraIdentifier.PlayerObject.transform != null)
            this.transform.LookAt(CameraIdentifier.PlayerObject.transform);
    }
}
