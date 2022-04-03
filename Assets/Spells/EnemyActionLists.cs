using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyActionLists
{
    private const string MediumEnemyOne = "MediumEnemyOne";

    public static EnemyActionData[] GetMediumEnemyActionList(string actionListName)
    {
        return actionListName switch
        {
            MediumEnemyOne => new EnemyActionData[]
            {
                new EnemyActionData
                {
                    Spell = new MediumEnemySpellCast(),
                    SpellCastFactory = () => { return new MediumEnemySpellCast(); },
                    RecastCooldown = 2,
                },
            },
            _ => null,
        };
    }
}
