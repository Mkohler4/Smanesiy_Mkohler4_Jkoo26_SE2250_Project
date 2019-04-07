using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is an enum of the varioys possible weapon types
//It also inlcudes a shield typer to allow a shield power-up
public enum WeaponType
{

    none,//The default / no weapon
    simple, //A simple blaster
    blaster,     //Two shots simultaneously
    shield, //Raise shieldLevel
    upgrade,
}
//The WeaponDefinition class allows you to set the properties
//of a specific weapon in the Inspector. The Main class has
//an array of WeaponDefinitions that makes this possible.
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;       //Letter to show on the power-up
    public Color color = Color.white;   //Color of Collar and power-up
    public GameObject projectilePrefab; //Prefab for projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 1000;   //Amount of damage caused
    public float continuousDamage = 1;  //Damage per second(Laser)
    public float delayBetweenShots = 0; 
    public float velocity = 20; //Speed of Projectiles

}
public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;
    public int angle = 0;
    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; // Time las shot was fired
    public int upgrade = 0;
    private Renderer _collarRend;
    private int _timer = 10;
    private bool _stop = false;
    
    // Start is called before the first frame update
    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        _collarRend = collar.GetComponent<Renderer>();

        // Call SetType() for the default _type of WeaponType.none
        SetType(_type);

        // Dynamically create an anchor for all Projectiles
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        //Find the fireDelegate of the root GameObject
        GameObject rootGO = transform.root.gameObject;
        if(rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
        
    }
    //Getter/Setter method public property
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    public static Projectile projectile { get; internal set; }

    //Set the weapon of choice
    //If weapon type is none the game object is disabled and visually disappears from the scene
    public void SetType(WeaponType weaponType)
    {
        _type = weaponType;
        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        //Grab the projectile color
        _collarRend.material.color = def.color;
        lastShotTime = 0; // Setting lastShotTime to zero allows the weapon to fire immedietly
    }
    public void Fire()
    {
        // If this.gameObject is inactive, return, weapon will not fire
        if (!gameObject.activeInHierarchy) return;
        //If it hasn't been enough time netween shots, return
        //If the current time minus the lastShotTime is less that the delatBetweenShots weapon will not fire
        if(Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile projectile;
        Vector3 vel = Vector3.up * def.velocity;
        //Initial velocity is set upward but if enemy weapons are facing downward the vel component is set downward aswell
        if(transform.up.y < 0)
        {
            vel.y = -vel.y;
        }
        //Returns a reference to the projectile class instance attached to the new projectile game object
        switch (type)
        {
            //Project the simple weapon
            case WeaponType.simple:
                projectile = MakeProjectile(); //Make projectile
                //Assign velocity to the gameobjects rigid body in direction of vel
                projectile.rigid.velocity = vel;
                break;

            //Project the blaster, three projectiles are created, to have their direction rotated 30 degrees
            case WeaponType.blaster:
                projectile = MakeProjectile();   // Make projectile
                //Assign velocity to the gameobjects rigid body in direction of vel
                projectile.rigid.velocity = vel;
                projectile = MakeProjectile();   //Make projectile
                //Rotate projectile around vector3.back axis
                projectile.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                projectile.rigid.velocity = projectile.transform.rotation * vel;
                projectile = MakeProjectile();   //Make projectile
                //Rotate projectile around vector3.back axis
                projectile.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                projectile.rigid.velocity = projectile.transform.rotation * vel;
                break;
            case WeaponType.upgrade:
                projectile = MakeProjectile();   // Make projectile
                //Assign velocity to the gameobjects rigid body in direction of vel
                projectile.rigid.velocity = vel;
                projectile = MakeProjectile();   //Make projectile
                //Rotate projectile around vector3.back axis
                projectile.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                projectile.rigid.velocity = projectile.transform.rotation * vel;
                projectile = MakeProjectile();   //Make projectile
                //Rotate projectile around vector3.back axis
                projectile.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                projectile.rigid.velocity = projectile.transform.rotation * vel;
                projectile = MakeProjectile();   //Make projectile
                //Rotate projectile around vector3.back axis
                projectile.transform.rotation = Quaternion.AngleAxis(15, Vector3.back);
                projectile.rigid.velocity = projectile.transform.rotation * vel;
                projectile = MakeProjectile();   //Make projectile
                //Rotate projectile around vector3.back axis
                projectile.transform.rotation = Quaternion.AngleAxis(-15, Vector3.back);
                projectile.rigid.velocity = projectile.transform.rotation * vel;
                break;
        }
                    
    }
    //Instantiates a clone of the prefab stored in the weapon definition, return reference attached projectile class instance
    public Projectile MakeProjectile()
    {
        GameObject gameObj = Instantiate<GameObject>(def.projectilePrefab);
        //Projectile is given the proper tag and physics layer
        if(transform.parent.gameObject.tag == "Hero")
        {
            gameObj.tag = "ProjectileHero";
            gameObj.layer = LayerMask.NameToLayer("ProjectileHero");

        } else
        {
            gameObj.tag = "ProjectileEnemy";
            gameObj.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        gameObj.transform.position = collar.transform.position;
        //The projectile GameObject's parent is set to projectil_anchor
        gameObj.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile projectile = gameObj.GetComponent<Projectile>();
        projectile.type = type;
        //LastShotTime is set to current time, preventing weapon from shooting for delayBetweenShots
        lastShotTime = Time.time;
        return (projectile);

    }

    // Update is called once per frame
    void Update()
    {
        
        //If keycode C is pressed switch the current weapon
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(type == WeaponType.simple)
            {
                type = WeaponType.blaster;
            }
            else
            {
                type = WeaponType.simple;
            }
        }
        if(Hero.ship.unlock !=0)
        {
            type = WeaponType.upgrade;
            Hero.ship.unlock--;
        }
        if(type == WeaponType.upgrade && Hero.ship.unlock == 0)
        {
            type = WeaponType.simple;
        }
    }
}
