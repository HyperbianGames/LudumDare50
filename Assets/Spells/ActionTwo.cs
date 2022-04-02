using UnityEngine;

public interface IAura
{
    public Creature AppliedBy { get; set; }
    public float LastTick { get; set; }
    public float TickRate { get; set; }
    public float Duration { get; set; }
    public float AppliedTime { get; set; }
    public float DefaultDuration { get; set; }
    public void ExecuteTick(Creature effectedCreature);
}

public class ActionTwo : Spell, IAura
{
    public float TickRate { get; set; } = .75f;
    public float DefaultDuration { get; set; } = 10f;
    public float Duration { get; set; }
    public Creature AppliedBy { get; set; }
    public float AppliedTime { get; set; }
    public float LastTick { get; set ; }

    public override void CastSuccess(Creature castingCreature)
    {
        AppliedBy = castingCreature;
        castingCreature.CurrentTarget.ApplyAura(this);
    }

    public override void CastStart(Creature castingCreature)
    {

    }

    public override bool ValidCast(Creature castingCreature)
    {
        return (castingCreature.CurrentTarget != null) && (Vector3.Distance(castingCreature.gameObject.transform.position, castingCreature.CurrentTarget.gameObject.transform.position) < 7);
    }

    public void ExecuteTick(Creature effectedCreature)
    {
        effectedCreature.ApplyDamage(AppliedBy, 3);
    }
}