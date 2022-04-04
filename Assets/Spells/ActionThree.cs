using UnityEngine;
public class ActionThree : Spell
{
    public ActionThree()
    {
        Cooldown = 5;
    }
    
    public override void CastSuccess(Creature castingCreature)
    {
        int damageDone = castingCreature.CurrentTarget.ApplyDamage(castingCreature, 30);
        castingCreature.ApplyDamage(castingCreature, damageDone * -1);
    }

    public override void CastStart(Creature castingCreature)
    {

    }

    public override bool ValidCast(Creature castingCreature)
    {
        return (castingCreature.CurrentTarget != null) && (Vector3.Distance(castingCreature.gameObject.transform.position, castingCreature.CurrentTarget.gameObject.transform.position) < 7);
    }
}