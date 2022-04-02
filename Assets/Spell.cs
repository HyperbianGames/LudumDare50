using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType
{
    Instant,
}

public abstract class Spell
{
    public Action CastEndCallback { get; set; }
    public Action CastStartCallback { get; set; }

    public SpellType SpellType { get; set; }
    public abstract void CastEnd();
    public abstract void CastStart();
}

public class ActionOne : Spell
{
    public override void CastEnd()
    {
    }

    public override void CastStart()
    {
    }
}



