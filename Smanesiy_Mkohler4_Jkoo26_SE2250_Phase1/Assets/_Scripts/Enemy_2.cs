﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    //Declarations
    [Header("Set in Inspector: Enemy_3")]
    public float waveFrequency = 2;
    public float waveWidth = 4;
    public float waveRotY = 45;
    private float _x0;
    private float _birthTime;


    //Start is called before the first frame update
    void Start()
    {
        _x0 = pos.x;
        _birthTime = Time.time;
        score = 50;
    }

    //Overide Move function on Enemy
    public override void Move()
    {
        //Move enemy in a sin wave pattern 
        Vector3 tempPos = pos;
        float age = Time.time - _birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = _x0 + waveWidth * sin;
        pos = tempPos;

        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        base.Move();
    }
    public override void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                
                health -= 5; //Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    // Destroy this Enemy
                    Destroy(this.gameObject);
                    //ScoreManager.S.addScore(score);
                    ScoreManager.score += 50;
                }
                Destroy(otherGO);
                ShowDamage();
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }

    }
}
