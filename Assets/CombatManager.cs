using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; set; }
    public const float GCDLength = 1.5f;
    public List<Creature> Creatures { get; set; } = new List<Creature>();
    public List<Creature> VisibleCreatures { get; set; } = new List<Creature>();
    public GameObject Camera;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        foreach (Creature creature in Creatures)
        {
            if (creature.CurrentGCD > 0)
                creature.CurrentGCD -= Time.deltaTime;
        }
    }

    internal void CastSpell(Creature creature, Spell spell)
    {
        Creatures.AddIfNotExist(creature);

        if (creature.CurrentGCD <= 0)
        {
            if (spell.SpellType == SpellType.Instant)
                creature.CurrentGCD += GCDLength;
            
            spell.CastStart();
            spell.CastEnd();
        }
    }

    internal static void SetVisibleCreature(Creature creature, bool v)
    {
        if (v)
        {
            Instance.VisibleCreatures.Add(creature);
            Debug.Log("creature added");
        }
        else if (Instance.VisibleCreatures.Contains(creature))
        {
            Instance.VisibleCreatures.Remove(creature);
            Debug.Log("creature removed");
        }
    }
}

public static class ListExtensions
{
    public static void AddIfNotExist<T>(this List<T> thisList, T obj)
    {
        if (!thisList.Contains(obj))
            thisList.Add(obj);
    }
}