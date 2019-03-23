using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero ship;
    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    [Header("Set Dynamically")]
    public float sheildLevel = 1;
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

        Vector3 pos = transform.position;
        pos.y += yAxis * speed * Time.deltaTime;
        pos.x += xAxis * speed * Time.deltaTime;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);


    }
}
