using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public GameObject HealthBar;
    public float CurrentHealth = 100;
    public float MaxHealth = 100;
    


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
            HealthBar.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 2-(GetHealthPercentage() * 2), GetHealthPercentage() * 2);
        }
    }

    private float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth; 
    }
}
