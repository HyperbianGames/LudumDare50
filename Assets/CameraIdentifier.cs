using UnityEngine;

public class CameraIdentifier : MonoBehaviour
{
    public static GameObject PlayerObject { get; set; } = null;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObject = gameObject;
    }
}
