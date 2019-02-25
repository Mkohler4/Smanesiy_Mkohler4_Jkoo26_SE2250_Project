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

        if (pos.x > (Camera.main.orthographicSize* Camera.main.aspect)/2)
        {
            rndnumber = 1;
        }
      
    }
    public override void Move()
    {
        if (rndnumber == 1)
        {
            Vector3 tempPos = pos;
            tempPos.x -= speed * Time.deltaTime;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
        else
        {
            Vector3 tempPos = pos;
            tempPos.x += speed * Time.deltaTime;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
        
    }
}
