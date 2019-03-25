﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    protected int score = 100;
    public float showDamageDuration = 0.1f; //#seconds to show damage

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials; //All the Materials of this & its children
    public bool showingDamage = false;
    public float damageDoneTime; //Time to show showing damage
    public bool notifiedOfDestruction = false;

    
    private BoundsCheck _bndCheck;
    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();

        //Get materials and colors for this GameObject and its children
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for(int i=0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

    }
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        //if the enemy moves off the screen in the y direction destroy the object
        Move();
        /*if(pos.y < -40)
        {
            Destroy(gameObject);
        }*/
        if(_bndCheck != null && _bndCheck.offDown)
        {
            Destroy(gameObject);
        }
        if(showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
       
    }
    public virtual void Move()
    {
        //Moves the two enemies straight downward 
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    public virtual void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch(otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                // If this enemy is off screen don't damage it
                if(!_bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                // Hurt this Enemy
                // Get the damage amount from the Main WEAP_DICT
                health -= 2; //Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health <= 0 )
                {
                    // Destroy this Enemy
                    Destroy(this.gameObject);
                    //ScoreManager.S.addScore(score);
                    ScoreManager.score += 100;
                }
                Destroy(otherGO);
                ShowDamage();
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }

    }

    public void ShowDamage()
    {
        foreach (Material mats in materials)
        {
            mats.color = Color.red;


        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }
    public void UnShowDamage()
    {
        for(int i=0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;

    
    }
}
