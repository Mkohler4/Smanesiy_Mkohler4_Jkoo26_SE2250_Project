using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    static public Main ship;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 1f;
    public float enemyDefaultPadding = 1.5f;

    private BoundsCheck _bndCheck;

    void Awake()
    {
        ship = this;
        _bndCheck = GetComponent<BoundsCheck>();
        //Takes method name and invokes it every 1 second
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    //Gets called every second
    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);
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
}
