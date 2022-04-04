using System;
using UnityEngine;

public enum SpellType
{
    Instant,
    CastTime,
}

public abstract class Spell
{
    public Action CastEndCallback { get; set; }
    public Action CastStartCallback { get; set; }

    public float CastTime { get; set; } = 0f;
    public float Cooldown { get; set; } = 0f;

    public SpellType SpellType { get; set; } = SpellType.Instant;
    public abstract void CastSuccess(Creature castingCreature);
    public abstract void CastStart(Creature castingCreature);

    public abstract bool ValidCast(Creature castingCreature);
}





