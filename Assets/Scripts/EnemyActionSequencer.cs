using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionData
{
    public Func<Spell> SpellCastFactory;
    public Spell Spell;
    public float RecastCooldown;
    public float LastCastTime = 0;
}

public class EnemyActionSequencer : MonoBehaviour
{
    public bool IsActive = true;
    public Creature CreatureToControl;
    public string ActionListToUse;
    public EnemyActionData[] SpellList;

    private void Start()
    {
        SpellList = EnemyActionLists.GetMediumEnemyActionList(ActionListToUse);
    }

    // Update is called once per frame
    void Update()
    {
        if (CreatureToControl.CanAct())
        {
            EnemyActionData actionData = GetNextAction();
            if (actionData != null)
            {
                actionData.LastCastTime = Time.time;
                
                CreatureToControl.CastSpell(actionData.SpellCastFactory());
            }
        }
    }

    private EnemyActionData GetNextAction()
    {
        for (int i = 0; i < SpellList.Length; i++)
        {
            float cooldownWeCareAbout = GetGreater(SpellList[i].LastCastTime + SpellList[i].Spell.Cooldown, SpellList[i].LastCastTime + SpellList[i].RecastCooldown);

            if (Time.time > cooldownWeCareAbout)
            {
                return SpellList[i];
            }
        }
        
        return null;
    }

    private float GetGreater(float one, float two)
    {
        if (one > two)
        {
            return one;
        }
        else
        {
            return two;
        }
    }
}
