using UnityEngine;

public class EnemyModelEventMarshal : MonoBehaviour
{
    public GameObject MainEnemyObject;
    public Creature CreatureObj;

    // Update is called once per frame
    void Update()
    {
        if (CreatureObj == null)
            CreatureObj = MainEnemyObject.GetComponent<Creature>();
    }

    public void OnBecameVisible()
    {
        CreatureObj.OnBecameVisible();
    }

    public void OnBecameInvisible()
    {
        CreatureObj.OnBecameInvisible();
    }
}
