using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyActionLists
{
    private const string MediumEnemyOne = "MediumEnemyOne";
    private const string Boss = "Boss";

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
            Boss => new EnemyActionData[]
            {
                new EnemyActionData
                {
                    Spell = new BossSwing(),
                    SpellCastFactory = () => { return new BossSwing(); },
                    RecastCooldown = 0,
                },
            },
            _ => null,
        };
    }
}
