using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_1 : Enemy
{
    int rndnumber = 0;
    private BoundsCheck _bndCheck;
    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
    }

    void Start()
    {
        //Chooses the the direction of the 45 degree enemy based off where they spawn
        if (pos.x > (((Camera.main.orthographicSize* Camera.main.aspect)/2) - 10))
        {
            rndnumber = 1;
        }
      
    }
    //Override the update function so children can't have children objects
    void Update()
    {

        //if the enemy moves off the screen in the y direction destroy the object
        Move();
        if (_bndCheck != null && _bndCheck.offDown)
        {
            Destroy(gameObject);
        }
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
    }
    //Move the enemy in a diagonal direction
    //Override the move function
    public override void Move()
    {
        //If they spawn more to the left of the screen
        if (rndnumber == 1)
        {
            //Move the enemy left downwards
            Vector3 tempPos = pos;
            tempPos.x -= speed * Time.deltaTime;
            tempPos.y -= speed * Time.deltaTime;
            tempPos.z = 0;
            pos = tempPos;
        }
        else
        {
            //Move enemy right downwards
            Vector3 tempPos = pos;
            tempPos.x += speed * Time.deltaTime;
            tempPos.y -= speed * Time.deltaTime;
            tempPos.z = 0;
            pos = tempPos;
        }
        
    }
    //Destroy the enemy if the enemy collides with the hero
    public override void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                // If this enemy is off screen don't damage it
                health -= 10;
                if (health <= 0)
                {
                    if (!notifiedOfDestruction)
                    {
                        Main.SHIP.ShipDestroyed(this);
                    }
                    //Increment the total score
                    ScoreManager.SCORE += 10;
                    // Destroy this Enemy
                    Destroy(this.gameObject);
                    
                }
                //Show the damage the enemy has taken and destroy the enemy
                Destroy(otherGO);
                ShowDamage();
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }

    }
}
