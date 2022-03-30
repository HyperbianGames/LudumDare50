using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    public GameObject BulletPrefab;
    public BulletSettings BulletSettings;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class BulletSettings
{
    public float Speed = 0;
    public int Damage = 0;
}
