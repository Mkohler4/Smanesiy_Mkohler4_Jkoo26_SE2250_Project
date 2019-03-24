using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    private BoundsCheck _bndCheck;
    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
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
        if(pos.y < -40)
        {
            Destroy(gameObject);
        }
       
    }
    public virtual void Move()
    {
        //Moves the two enemies straight downward 
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        if(otherGO.tag == "ProjectileHero")
        {
            Destroy(otherGO);   //Destroy the Projectile
            Destroy(gameObject);    //Destroy this Enemy GameObject
        } else
        {
            print("Enemy hit by non-ProjectileHero:  " + otherGO.name);
        }
    }
}
