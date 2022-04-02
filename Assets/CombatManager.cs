using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnRegionData
{
    public string RegionName;
    public GameObject[] SpawnLocations;
}

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; set; }
    public const float GCDLength = 1.5f;
    public List<Creature> Creatures { get; set; } = new List<Creature>();
    public List<Creature> VisibleCreatures { get; set; } = new List<Creature>();
    public GameObject Camera;

    public GameObject DummyObjectPrefab;
    public GameObject MediumEnemyObjectPrefab;
    public GameObject FinalBossObjectPrefab;

    public SpawnRegionData[] SpawnRegionData;

    private Dictionary<string, SpawnRegionData> SpawnRegions = new Dictionary<string, SpawnRegionData>();

    private void Start()
    {
        Instance = this;
        foreach (SpawnRegionData data in SpawnRegionData)
        {
            SpawnRegions.Add(data.RegionName, data);
        }

        foreach (GameObject spawnLoc in SpawnRegions["Dummies"].SpawnLocations)
        {
            GameObject newObj = Instantiate(DummyObjectPrefab);
            newObj.transform.position = spawnLoc.transform.position;
            newObj.transform.rotation = spawnLoc.transform.rotation;
        }

        foreach (GameObject spawnLoc in SpawnRegions["MediumEnemies"].SpawnLocations)
        {
            GameObject newObj = Instantiate(MediumEnemyObjectPrefab);
            newObj.transform.position = spawnLoc.transform.position;
            newObj.transform.rotation = spawnLoc.transform.rotation;
        }
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
          
            if (spell.ValidCast(creature))
            {
                if (spell.SpellType == SpellType.Instant)
                    creature.CurrentGCD += GCDLength;

                spell.CastStart(creature);
                spell.CastStartCallback?.Invoke();
                spell.CastSuccess(creature);
                spell.CastEndCallback?.Invoke();
            }
        }
    }

    internal static void SetVisibleCreature(Creature creature, bool v)
    {
        if (v)
        {
            Instance.VisibleCreatures.Add(creature);
        }
        else if (Instance.VisibleCreatures.Contains(creature))
        {
            Instance.VisibleCreatures.Remove(creature);
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