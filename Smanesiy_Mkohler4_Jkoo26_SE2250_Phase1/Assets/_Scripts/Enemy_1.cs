using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_1 : Enemy
{
    int rndnumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Chooses the the direction of the 45 degree enemy based off where they spawn
        if (pos.x > (Camera.main.orthographicSize* Camera.main.aspect)/2)
        {
            rndnumber = 1;
        }
      
    }
    public override void Move()
    {
        //If they spawn more to the left of the screen
        if (rndnumber == 1)
        {
            //Move the enemy left downwards
            Vector3 tempPos = pos;
            tempPos.x -= speed * Time.deltaTime;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
        else
        {
            //Move enemy right downwards
            Vector3 tempPos = pos;
            tempPos.x += speed * Time.deltaTime;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
        
    }
}
