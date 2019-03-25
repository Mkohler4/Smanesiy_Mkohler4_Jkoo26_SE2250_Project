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

        //Allow the ship to fire
       // if(Input.GetKeyDown(KeyCode.Space))
      //  {
      //      TempFire();
      //  }

        //Use the fireDelegate to fire Weapons
        //First, make sure the button is oressed: Axis("Jump")
        //Then ensure that fireDelegate isn't null to avoid an error
        if(Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }

    }
    /*void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * projectileSpeed;

        Projectile proj = projGO.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }*/
    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        
        if (go == _lastTriggerGo)
        {
            return;
        }
        _lastTriggerGo = go;
        if (go.tag == "Enemy") //If the shield was triggered by an enemy
        {
            shieldLevel--;    //Decrease the level of the sheild by 1   
            Destroy(go);       //... and Destroy the enemy
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
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
                ScoreManager.score = 0;
                //Tell Main.ship to restart the game after a delay
                Main.ship.DelayedRestart(gameRestartDelay);
            }
        }
    }
    
}
