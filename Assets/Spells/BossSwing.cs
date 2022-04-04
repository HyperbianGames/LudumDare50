using UnityEngine;
public class BossSwing : Spell
{
    public BossSwing()
    {
        Cooldown = 3;
    }

    public override void CastSuccess(Creature castingCreature)
    {
        castingCreature.CurrentTarget.ApplyDamage(castingCreature, 10);
    }

    public override void CastStart(Creature castingCreature)
    {

    }

    public override bool ValidCast(Creature castingCreature)
    {
        return (castingCreature.CurrentTarget != null) && (Vector3.Distance(castingCreature.gameObject.transform.position, castingCreature.CurrentTarget.gameObject.transform.position) < 7);
    }
}