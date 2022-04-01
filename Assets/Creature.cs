using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public GameObject HealthBar;
    public float CurrentHealth = 100;
    public float MaxHealth = 100;

    public float CurrentGCD = 0;

    public Dictionary<Spell, float> Cooldowns { get; set; } = new Dictionary<Spell, float>();

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void CastSpell(Spell spell)
    {
        CombatManager.Instance.CastSpell(this, spell);
    }

    private float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth; 
    }
}
