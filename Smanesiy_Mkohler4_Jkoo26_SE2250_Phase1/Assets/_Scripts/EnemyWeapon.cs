using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    //Declare variables
    public GameObject collar;
    private Renderer _collarRend;
    static public Transform PROJECTILE_ANCHOR;
    public float lastShotTime;
    public float delayBetweenShots = 0;
    public float velocity = 50;
    public GameObject projectilePrefab;
    public float counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Find where the collar object is located as the bullets will fire from there
        collar = transform.Find("Collar").gameObject;
        _collarRend = collar.GetComponent<Renderer>();
      
        // Dynamically create an anchor for all Projectiles
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        //Find the fireDelegate of the root GameObject
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Enemy>() != null)
        {
            rootGO.GetComponent<Enemy>().fireDelegate += Fire;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Enemies start firing (Level 2)
        if (ScoreManager.SCORE >= 250 && ScoreManager.SCORE < 500)
        {
            //Enemies fire every 50 frames
            counter++;
            if (counter % 50 == 0)
            {
                //Call fire method
                Fire();
            }
        }
        //Enemies start shooting faster (Level 3 and max level)
        //Shoot once every 30 frames
        else if(ScoreManager.SCORE >= 500)
        {
            counter++;
            if(counter % 25 == 0)
            {
                Fire();
            }
        }
        
    }
    //Fire function used for firing the enemy weapon
    void Fire()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Time.time - lastShotTime < delayBetweenShots)
        {
            return;
        }
        Projectile projectile;
        //Make the velocity of the bullet go straight down
        Vector3 vel = Vector3.down * velocity * 3;
        
        //If bullet is firing up make it fire down so it hits the hero
        if (transform.up.y > 500)
        {
            vel.y = -vel.y;
        }
        //Make a projectile
        projectile = MakeProjectile();
        projectile.rigid.velocity = vel;
        //(Level 4) Enimies start making different projectiles
        if(ScoreManager.SCORE >= 750)
        {
            //Make two different projectiles for the enemies with different angles
            projectile = MakeProjectile();
            //Get the angle that the bullet will be traveling
            projectile.transform.rotation = Quaternion.AngleAxis(5, Vector3.back);
            //Get the velocity of the bullet
            projectile.rigid.velocity = projectile.transform.rotation * vel;
            projectile = MakeProjectile();
            projectile.transform.rotation = Quaternion.AngleAxis(-5, Vector3.back);
            projectile.rigid.velocity = projectile.transform.rotation * vel;
        }
    }
    public Projectile MakeProjectile()
    {
        GameObject gameObj = Instantiate<GameObject>(projectilePrefab);
        //Projectile is given the proper tag and physics layer
        if (transform.parent.gameObject.tag == "Enemy")
        {
            gameObj.tag = "ProjectileEnemy";
            gameObj.layer = LayerMask.NameToLayer("ProjectileEnemy");

        }
        else
        {
            gameObj.tag = "ProjectileHero";
            gameObj.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        //Find the position for the collar of the barrel
        gameObj.transform.position = collar.transform.position;
        //The projectile GameObject's parent is set to projectil_anchor
        gameObj.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile projectile = gameObj.GetComponent<Projectile>();
        //LastShotTime is set to current time, preventing weapon from shooting for delayBetweenShots
        lastShotTime = Time.time;
        return (projectile);

    }
}
