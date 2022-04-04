using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemySpellCast : Spell
{
    public MediumEnemySpellCast()
    {
        SpellType = SpellType.CastTime;
        CastTime = 3.5f;
        Cooldown = 1;
    }

    public override void CastSuccess(Creature castingCreature)
    {
        castingCreature.CurrentTarget.ApplyDamage(castingCreature, 25);
    }

    public override void CastStart(Creature castingCreature)
    {

    }

    public override bool ValidCast(Creature castingCreature)
    {
        return (castingCreature.CurrentTarget != null) && (Vector3.Distance(castingCreature.gameObject.transform.position, castingCreature.CurrentTarget.gameObject.transform.position) < 7);
    }
}
