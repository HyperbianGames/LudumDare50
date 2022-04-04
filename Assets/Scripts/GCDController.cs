using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCDController : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject buttonParent;
    public string ActionName;
    Creature playerCreature;

    public void Start()
    {
        playerCreature = playerObject.GetComponent<Creature>();
    }

    // Update is called once per frame
    void Update()
    {
        float value = playerCreature.CurrentGCD / CombatManager.GCDLength;
        if (playerCreature.Cooldowns.ContainsKey(ActionName))
        {
            float timeLeft = playerCreature.Cooldowns[ActionName].Item1 - Time.time;
            if (timeLeft > playerCreature.CurrentGCD)
            {
                value = timeLeft / playerCreature.Cooldowns[ActionName].Item2;
            }
        }
        
        gameObject.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 60 - (value * 60), value * 60);
    }
}
