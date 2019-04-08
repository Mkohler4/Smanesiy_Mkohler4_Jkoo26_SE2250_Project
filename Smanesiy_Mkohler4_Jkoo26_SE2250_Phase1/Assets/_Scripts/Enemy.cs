using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    //public GameObject[] prefabEnemies;
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public float showDamageDuration = 0.1f; //#seconds to show damage 
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;
    int counter = 1;

    //[Header("These fields are set dynamically")]

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
        //Public property
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
    //Update function used by enemy0 and enemy2 
    void Update()
    {
        
        //if the enemy moves off the screen in the y direction destroy the object
        Move();
        if(_bndCheck != null && _bndCheck.offDown)
        {
            Destroy(gameObject);
        }
        //unshow the damage the enemy has previously taken
        if(showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
        if (ScoreManager.SCORE >= 500)
        {
            //Instatiate a child after 50 frames
            if (counter % 50 == 0)
            {
                GameObject go = Instantiate<GameObject>(Main.SHIP.prefabEnemies[1]);
                //Get the position of the parent object
                go.transform.position = pos;
                //Set that position to the position of the child object 
                Vector3 temp = pos;
            }
        }
        //Increment counter to keep track of the frames
        counter++;
             
    }
    //Move the enemy straight down the screen
    public virtual void Move()
    {
        //Moves the two enemies straight downward 
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime * 3;
        pos = tempPos;
    }
    //If there is a collision between the hero and the enemy destroy the enemy 
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
                health -= 2;
                if(health <= 0 )
                {
                    //Tell the Main singleton that this ship was destroyed
                    if (!notifiedOfDestruction)
                    {
                        Enemy enemy = this;
                        Main.SHIP.ShipDestroyed(enemy);
                    }
                    notifiedOfDestruction = true;
                    // Destroy this Enemy
                    Destroy(this.gameObject);
                    //Update score when specific enemy is destroyed by 100
                    ScoreManager.SCORE += 25;
                }
                Destroy(otherGO);
                ShowDamage();
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }

    }
    //Show the damge dealt to the enemy while the enemy is taking damage
    public void ShowDamage()
    {
        //When shot at change enemy colour to red on impact
        foreach (Material mats in materials)
        {
            mats.color = Color.red;
        }
        showingDamage = true;
        //Show damage for showDamageDuration time
        damageDoneTime = Time.time + showDamageDuration;
    }
    public void UnShowDamage()
    {
        //Change back to original color once the bullet is already impacted the enemy 
        for(int i=0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
