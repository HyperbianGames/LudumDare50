using System;

public enum SpellType
{
    Instant,
}

public abstract class Spell
{
    public Action CastEndCallback { get; set; }
    public Action CastStartCallback { get; set; }

    public SpellType SpellType { get; set; }
    public abstract void CastSuccess(Creature castingCreature);
    public abstract void CastStart(Creature castingCreature);

    public abstract bool ValidCast(Creature castingCreature);
}





