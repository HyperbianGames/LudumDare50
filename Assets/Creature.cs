using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    public GameObject HealthBar;
    public GameObject TargetingIndicator;

    public float CurrentHealth = 100;
    public float MaxHealth = 100;

    public float CurrentGCD = 0;

    public Dictionary<Spell, float> Cooldowns { get; set; } = new Dictionary<Spell, float>();
    public bool IsPlayer { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        CombatManager.Instance.Creatures.Add(this);
    }

    private void OnDestroy()
    {
        CombatManager.Instance.Creatures.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= Time.deltaTime;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
        
        if (HealthBar != null)
        {
            HealthBar.GetComponent<ProgressBarController>().CurrentValue = GetHealthPercentage() * 100;
        }
    }

    void OnMouseEnter()
    {
        print("Creature moused");
    }

    private void OnMouseExit()
    {
        print("Creature moused-");
    }

    public void SetAsTarget(bool value)
    {
        if (!IsPlayer)
            TargetingIndicator.GetComponent<Image>().enabled = value;
    }

    void OnBecameVisible()
    {
        if (!IsPlayer)
            CombatManager.SetVisibleCreature(this, true);
    }

    private void OnBecameInvisible()
    {
        if (!IsPlayer)
            CombatManager.SetVisibleCreature(this, false);
    }

    public void CastSpell(Spell spell)
    {
        CombatManager.Instance.CastSpell(this, spell);
    }

    private float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth; 
    }

    
}
