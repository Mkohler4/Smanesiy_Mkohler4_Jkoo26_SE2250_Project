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
        //fireDelegate += TempFire;
    }
    // Update is called once per frame
    void Update()
    {
        //Move hero with arrow or letter keys
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.y += yAxis * speed * Time.deltaTime;
        pos.x += xAxis * speed * Time.deltaTime;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        if(Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }

    }
    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject gameObj = rootT.gameObject;
        GameObject otherGameObj = other.gameObject;
        if (otherGameObj.tag == "ProjectileEnemy")
        {
            Projectile p = otherGameObj.GetComponent<Projectile>();
            shieldLevel--;
            Destroy(otherGameObj);
        }
        if (gameObj == _lastTriggerGo)
        {
            return;
        }
        _lastTriggerGo = gameObj;
        if(gameObj.tag == "ProjectileEnemy")
        {
            shieldLevel--;
            Destroy(gameObj);
        }
        if (gameObj.tag == "Enemy") //If the shield was triggered by an enemy
        {
            shieldLevel--;    //Decrease the level of the sheild by 1   
            Destroy(gameObj);       //... and Destroy the enemy
            
        }
        else if(gameObj.tag == "PowerUp")
        {
            //If the shield was triggered by a PowerUp
            AbsorbPowerUp(gameObj);
        }
        else
        {
            print("Triggered by non-Enemy: " + gameObj.name);
        }
    }
    /*public void OnCollisionEnter(Collision collision)
    {
        GameObject otherGameObj = collision.gameObject;
        if(otherGameObj.tag == "ProjectileEnemy")
        {
            Projectile p = otherGameObj.GetComponent<Projectile>();
            shieldLevel--;
            Destroy(otherGameObj);


        }
    }*/
    public void AbsorbPowerUp(GameObject gameObj)
    {
        PowerUp powerUp = gameObj.GetComponent<PowerUp>();
        switch (powerUp.type)
        {
            
        }
        powerUp.AbsorbedBy(this.gameObject);
        if(Main.SHIP.powerUpFrequency[1] == Main.SHIP.powerUpType)
        {
            Hero.ship.shieldLevel++;

        }
        if(Main.SHIP.powerUpFrequency[0] == Main.SHIP.powerUpType)
        {
            unlock = 300;
        }
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                ScoreManager.SCORE = 0;
                //Tell Main.ship to restart the game after a delay
                Main.SHIP.DelayedRestart(gameRestartDelay);
            }
        }
    }
    
}
