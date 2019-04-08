using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChange : MonoBehaviour
{
    // Start is called before the first frame update
    //Set the first colour to white
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(204, 204, 204);  
    }

    // Update is called once per frame
    void Update()
    {
        //If R is pressed change the ships colour to red
        if (Input.GetKeyDown(KeyCode.R))
        {
           GetComponent<Renderer>().material.color = new Color(255, 0, 0);
               
        }
        //If W is predded change the ships colour to white
        else if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<Renderer>().material.color = new Color(204, 204, 204);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
        //If P is pressed change the ships colour to purple
        else if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<Renderer>().material.color = new Color(75, 0, 130);
        }
        //If B is pressed change the ships colour to black
        else if(Input.GetKeyDown(KeyCode.B))
        {
            GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        //If Y is pressed change the ships colour to yello
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            GetComponent<Renderer>().material.color = new Color(255, 255, 0);
        }





    }
}
