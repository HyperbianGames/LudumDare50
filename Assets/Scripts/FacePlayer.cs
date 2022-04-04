using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (PlayerIdentifier.PlayerObject?.transform != null)
            {
                this.transform.LookAt(PlayerIdentifier.PlayerObject.transform);
            }
        }
        catch
        {

        }
    }
}
