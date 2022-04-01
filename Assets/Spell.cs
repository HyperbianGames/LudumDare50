using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType
{
    Instant,
}

public abstract class Spell
{
    public SpellType SpellType { get; set; }
    public abstract void CastEnd();
    public abstract void CastStart();
}

public class ActionOne : Spell
{
    public override void CastEnd()
    {
        Debug.Log("ActionOne Cast Complete");
    }

    public override void CastStart()
    {
        Debug.Log("ActionOne Cast");
    }
}



