using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (PlayerIdentifier.PlayerObject.transform != null)
            this.transform.LookAt(PlayerIdentifier.PlayerObject.transform);
    }
}
