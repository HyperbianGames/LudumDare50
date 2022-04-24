using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Hyperbius.Mortar;
using Hyperbius.Mortar.Core;

[Serializable]
public class SpawnRegionData
{
    public string RegionName;
    public GameObject[] SpawnLocations;
}

public class CombatManager : MonoBehaviour
{
    public IServer server = new ENetServer();
    
    public static CombatManager Instance { get; set; }
    public const float GCDLength = 1.5f;
    public List<Creature> Creatures { get; set; } = new List<Creature>();
    public List<Creature> VisibleCreatures { get; set; } = new List<Creature>();
    public GameObject Camera;

    public GameObject DummyObjectPrefab;
    public GameObject MediumEnemyObjectPrefab;
    public GameObject FinalBossObjectPrefab;

    public bool BossIsBeingFought = false;

    private bool gameOver = false;
    private float GameOverTime = 0;

    public SpawnRegionData[] SpawnRegionData;

    private Dictionary<string, SpawnRegionData> SpawnRegions = new Dictionary<string, SpawnRegionData>();

    [SerializeField]
    private InputActionReference AnyKey;

    private void Start()
    {
        Time.timeScale = 1;
        Instance = this;
        AnyKey.action.Enable();
        foreach (SpawnRegionData data in SpawnRegionData)
        {
            SpawnRegions.Add(data.RegionName, data);
        }

        foreach (GameObject spawnLoc in SpawnRegions["Dummies"].SpawnLocations)
        {
            GameObject newObj = Instantiate(DummyObjectPrefab);
            newObj.GetComponent<NavMeshAgent>().enabled = false;
            newObj.transform.position = spawnLoc.transform.position;
            newObj.transform.rotation = spawnLoc.transform.rotation;
            newObj.GetComponent<NavMeshAgent>().enabled = true;
        }

        foreach (GameObject spawnLoc in SpawnRegions["MediumEnemies"].SpawnLocations)
        {
            GameObject newObj = Instantiate(MediumEnemyObjectPrefab);
            newObj.GetComponent<NavMeshAgent>().enabled = false;
            newObj.transform.position = spawnLoc.transform.position;
            newObj.transform.rotation = spawnLoc.transform.rotation;
            newObj.GetComponent<NavMeshAgent>().enabled = true;
        }

        foreach (GameObject spawnLoc in SpawnRegions["BossSpawn"].SpawnLocations)
        {
            GameObject newObj = Instantiate(FinalBossObjectPrefab);
            newObj.GetComponent<NavMeshAgent>().enabled = false;
            newObj.transform.position = spawnLoc.transform.position;
            newObj.transform.rotation = spawnLoc.transform.rotation;
            newObj.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    internal void CreatureDestroyed(Creature creature)
    {
        Creatures.Remove(creature);
        if (creature.IsBoss)
        {
            gameOver = true;
            GameOverTime = Time.realtimeSinceStartup;
            GameMenuController.Instance.ShowWinScreen();
        }
    }

    private void Update()
    {
        foreach (Creature creature in Creatures)
        {
            if (creature.CurrentGCD > 0)
                creature.CurrentGCD -= Time.deltaTime;
        }

        if (gameOver && AnyKey.action.triggered)
        {
            if (GameOverTime + 1f < Time.realtimeSinceStartup)
            {
                GameMenuController.Instance.ShowMenu();
            }
        }
            
    }

    private bool IsOnCooldown(Creature creature, string spellName)
    {
        return (creature.Cooldowns.ContainsKey(spellName) && creature.Cooldowns[spellName].Item1 > Time.time);
    }

    internal void CastSpell(Creature creature, Spell spell)
    {
        Creatures.AddIfNotExist(creature);

        string spellName = spell.GetType().Name;

        switch (spell.SpellType)
        {
            case SpellType.Instant:
                if (creature.CurrentGCD <= 0 && !IsOnCooldown(creature, spellName))
                {
                    if (spell.ValidCast(creature))
                    {
                        if (!creature.Cooldowns.ContainsKey(spellName))
                        {
                            creature.Cooldowns.Add(spellName, new Tuple<float, float>(0f, 0f));
                        }
                        
                        spell.CastStart(creature);
                        spell.CastStartCallback?.Invoke();

                        creature.Cooldowns[spellName] = new Tuple<float, float>(Time.time + spell.Cooldown, spell.Cooldown);
                        creature.CurrentGCD += GCDLength;
                        spell.CastSuccess(creature);
                        spell.CastEndCallback?.Invoke();
                    }
                }
                break;
            case SpellType.CastTime:
                if (creature.CurrentGCD <= 0)
                {
                    if (spell.ValidCast(creature))
                    {
                        spell.CastStart(creature);
                        spell.CastStartCallback?.Invoke();
                        creature.CurrentGCD += GCDLength;
                        spell.CastSuccess(creature);
                        spell.CastEndCallback?.Invoke();
                    }
                }
                break;
        }
    }

    public abstract class SpellEffect
    { }


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