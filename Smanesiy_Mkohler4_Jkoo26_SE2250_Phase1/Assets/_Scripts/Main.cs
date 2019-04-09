using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main SHIP;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    //Declare variables
    [Header("Set in Inspector")]
    public int ndx;
    public GameObject[] prefabEnemies;
    public WeaponType powerUpType;
    public float enemySpawnPerSecond = 1f;
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public float powerUpDropChance = 1f;
    public int index = 0;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.simple, WeaponType.blaster, WeaponType.upgrade };
    private BoundsCheck _bndCheck;

    public void ShipDestroyed(Enemy enemy)
    {
        //Potentially generate a PowerUp chance is 40%
        if (Random.Range(0, 8) <= powerUpDropChance)
        {
            //Choose which PowerUp to pick
            //Pick one from the possibilities in powerUpFrequency
            index = Random.Range(0, 2);
            powerUpType = powerUpFrequency[index];

            //Spawn a PowerUp
            GameObject gameObj = Instantiate(prefabPowerUp) as GameObject;
            PowerUp powerUp = gameObj.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            powerUp.SetType(powerUpType);
            // Set it tot he position of the destroyed ship
            powerUp.transform.position = enemy.transform.position;
            
        }
    }

    void Awake()
    {
        SHIP = this;
        //Bound check for the ship so ship doesn't go off screen
        _bndCheck = GetComponent<BoundsCheck>();
        //Takes method name and invokes it every 1 second
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        //A generic Dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
        //Set score to 0 at the biggining of the next game
        ScoreManager.SCORE = 0;
    }
    
    //Gets called every second
    public void SpawnEnemy()
    {
        //Generate a random number between 0 and 2
        ndx = Random.Range(0, 3);
        //Don't let the baby enemy spawn as he will be used as a kid for the other enemies
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx==1?ndx-1:ndx]);
        float enemyPadding = enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;
        //float xMin = -bndCheck.camWidth + enemyPadding;
        //obtains the boundaries of the game on the x-axis
        float xMin = -Camera.main.orthographicSize * Camera.main.aspect + enemyPadding;
        float xMax = Camera.main.orthographicSize * Camera.main.aspect - enemyPadding;
        //float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        //obtains the boundaries of the y-axis within the game
        pos.y = _bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        //Spawn one enemy per second
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        //Invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        //Reload _Scene_0 to restart the game
        SceneManager.LoadScene("SampleScene");
    }
    //Static function that gets a WeaponDefintion from the WEAP_DICT static
    //protected field of the Main class.
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //Check to make sure that the key exists in the Dictionary
        //Attempting to retrieve a key that didn't exist, would throw an error,
        // so the following if statement is important.

        if(WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);

        }
        //This returns a new WeaponDefinition with a type of WeaponType.none
        // which means it had failed to find the right WeaponDefinition
        return (new WeaponDefinition());
    }
}
