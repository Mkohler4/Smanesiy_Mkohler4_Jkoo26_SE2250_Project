using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    static public Hero ship; //Singleton
    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public float unlock = 0;

    private BoundsCheck _bndCheck;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;
    private GameObject _lastTriggerGo = null;
    //Declare a  new delagte type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //Create a WeaponFireDelagte field named fireDelegate
    public WeaponFireDelegate fireDelegate;
    //Singleton of a hero so one instance can only be made
    void Awake()
    {
        if (ship == null)
        {
            ship = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Move hero with arrow or letter keys
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        //Calculate the ships position
        Vector3 pos = transform.position;
        pos.y += yAxis * speed * Time.deltaTime * 2;
        pos.x += xAxis * speed * Time.deltaTime * 2;
        transform.position = pos;
        //Tilt the ship based off of position
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        if(Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }

    }
    //Collision witht the projectile of the enemy is ontrigger so the collision can't physically be seen
    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject gameObj = rootT.gameObject;
        GameObject otherGameObj = other.gameObject;
        //If the hero is is with enemy projectile subtract a sheild from the hero
        if (otherGameObj.tag == "ProjectileEnemy")
        {
            Projectile p = otherGameObj.GetComponent<Projectile>();
            shieldLevel--;
            //Destroy the projectile after collision
            Destroy(otherGameObj);
        }
        if (gameObj == _lastTriggerGo)
        {
            return;
        }
        _lastTriggerGo = gameObj;
        //If hero ship collides with enemy subtract 1 from the shild of the hero
        if (gameObj.tag == "Enemy") //If the shield was triggered by an enemy
        {
            shieldLevel--;    //Decrease the level of the sheild by 1   
            Destroy(gameObj);       //... and Destroy the enemy
            
        }
        //If hero collides with powerup absorb the powerup
        else if(gameObj.tag == "PowerUp")
        {
            //If the shield was triggered by a PowerUp
            AbsorbPowerUp(gameObj);
        }
       
    }
    public void AbsorbPowerUp(GameObject gameObj)
    {
        PowerUp powerUp = gameObj.GetComponent<PowerUp>();
        powerUp.AbsorbedBy(this.gameObject);
        //If hero collides with blue power up add one to the hero's sheild
        if(powerUp.type == WeaponType.blaster)
        {
            Hero.ship.shieldLevel++;

        }
        //If hero collides with the white power up unlock the special weapon for 30 frames
        if(powerUp.type == WeaponType.simple)
        {
            unlock = 300;
        }
    }
    //Define setter and getter methods for the sheild variable
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            //Set the max sheild level possible to obtain to 4
            _shieldLevel = Mathf.Min(value, 4);
            //If hero has no more sheild and gets hit by enemy object destroy the hero
            if (value < 0)
            {
                Destroy(this.gameObject);                            
                //Tell Main.ship to restart the game after a delay
                Main.SHIP.DelayedRestart(gameRestartDelay);
            }
        }
    }
    
}
