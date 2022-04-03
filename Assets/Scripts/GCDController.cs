using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCDController : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject buttonParent;

    Spell spell;
    Creature playerCreature;
    public void Start()
    {
        playerCreature = playerObject.GetComponent<Creature>();
    }

    // Update is called once per frame
    void Update()
    {
        float value = playerCreature.CurrentGCD / CombatManager.GCDLength;
        gameObject.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 60 - (value * 60), value * 60);
    }
}
