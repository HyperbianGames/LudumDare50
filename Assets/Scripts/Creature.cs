using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    public GameObject HealthBar;
    public GameObject TargetingIndicator;

    public float CurrentHealth = 100;
    public float MaxHealth = 100;

    public float CurrentGCD = 0;

    public EnemyActionSequencer EAS = null;
    public GameObject playerModel;

    public Dictionary<string, Tuple<float, float>> Cooldowns { get; set; } = new Dictionary<string, Tuple<float, float>>();
    public bool IsPlayer = false;

    public bool CanAct()
    {
        return CurrentGCD <= 0 && CurrentTarget != null;
    }

    public bool IsKillable = true;
    public bool IsBoss = false;
    public Creature CurrentTarget { get; set; } = null;

    public NavMeshAgent agent;

    public Dictionary<string, IAura> Auras { get; set; } = new Dictionary<string, IAura>();

    // Start is called before the first frame update
    void Start()
    {
        if (!IsPlayer)
            CombatManager.Instance.Creatures.Add(this);
    }

    private void OnDestroy()
    {
        if (!IsPlayer)
        {
            if (!CombatManager.Instance.BossIsBeingFought)
            {
                PlayerController.Instance.Creature.CurrentHealth = PlayerController.Instance.Creature.MaxHealth;
            }

            CombatManager.Instance.CreatureDestroyed(this);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldMoveTowardsTarget())
        {
            agent.destination = CurrentTarget.transform.position;
        }
        

        if (CurrentHealth > 0)
        {
            // This really shouldn't be here / this Update method shouldn't exist but w/e jamming
            List<string> removeList = new List<string>();
            
            foreach (KeyValuePair<string, IAura> auraInstance in Auras)
            {
                IAura aura = auraInstance.Value;
                if (AuraShouldTick(aura))
                {
                    aura.LastTick = Time.time;
                    aura.ExecuteTick(this);
                }

                if (aura.AppliedTime + aura.Duration < Time.time)
                {
                    removeList.Add(auraInstance.Key);
                }
            }

            foreach (string removeKey in removeList)
            {
                Auras.Remove(removeKey);
            }
        }
        else
        {
            if (IsKillable)
            {
                

                if (IsPlayer)
                {
                    Destroy(playerModel);
                    GameMenuController.Instance.ShowLoseScreen();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                CurrentHealth = MaxHealth;
            }
        }
        
        if (HealthBar != null)
        {
            HealthBar.GetComponent<ProgressBarController>().CurrentValue = GetHealthPercentage() * 100;
        }
    }

    private bool ShouldMoveTowardsTarget()
    {
        return (!IsPlayer) && (CurrentTarget != null) && (agent != null) && NextAttackInRange();
    }

    private bool NextAttackInRange()
    {
        return true;
    }

    private bool AuraShouldTick(IAura aura)
    {
        return aura.LastTick + aura.TickRate < Time.time;
    }

    public void SetAsPlayerTarget(bool value)
    {
        if (!IsPlayer)
            TargetingIndicator.GetComponent<Image>().enabled = value;
    }

    public int ApplyDamage(Creature sourceCreature, int damageAmount)
    {
        if (damageAmount > 0 && !IsPlayer)
        {
            CurrentTarget = sourceCreature;
        }
        
        // This methos is mostly here in case we want to record this data or something
        CurrentHealth -= damageAmount;
        return damageAmount;
    }

    public void OnBecameVisible()
    {
        if (!IsPlayer)
            CombatManager.SetVisibleCreature(this, true);
    }

    public void OnBecameInvisible()
    {
        if (!IsPlayer)
            CombatManager.SetVisibleCreature(this, false);
    }

    public void CastSpell(Spell spell)
    {
        CombatManager.Instance.CastSpell(this, spell);
    }

    public void ApplyAura<T>(T aura) where T : Spell, IAura
    {
        Type type = typeof(T);
        aura.AppliedTime = Time.time;
        aura.Duration = aura.DefaultDuration;

        if (Auras.ContainsKey(type.Name))
        {
            float compareOne = Auras[type.Name].AppliedTime + Auras[type.Name].Duration - Time.time;
            float compareTwo = aura.DefaultDuration * 0.3f;

            if (compareOne < compareTwo)
            {
                aura.Duration += (Auras[type.Name].Duration) - (Time.time - Auras[type.Name].AppliedTime);               
            }
            else
            {
                aura.Duration = aura.Duration * 1.3f;
            }

            Auras[type.Name] = aura;
        }
        else
        {            
            Auras.Add(type.Name, aura);
        }
    }

    private float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth; 
    }
}
