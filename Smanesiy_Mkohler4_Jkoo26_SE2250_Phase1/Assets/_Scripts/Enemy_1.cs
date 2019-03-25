using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_1 : Enemy
{
    int rndnumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Chooses the the direction of the 45 degree enemy based off where they spawn
        if (pos.x > (Camera.main.orthographicSize* Camera.main.aspect)/2)
        {
            rndnumber = 1;
        }
      
    }
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
    public override void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                // If this enemy is off screen don't damage it
                /*if (!_bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }*/
                // Hurt this Enemy
                // Get the damage amount from the Main WEAP_DICT
                health -= 10; //Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    // Destroy this Enemy
                    Destroy(this.gameObject);
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
