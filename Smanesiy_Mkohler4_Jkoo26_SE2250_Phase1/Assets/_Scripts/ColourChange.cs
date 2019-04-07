using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChange : MonoBehaviour
{
    bool blue;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(204, 204, 204);  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
           GetComponent<Renderer>().material.color = new Color(255, 0, 0);
               
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<Renderer>().material.color = new Color(204, 204, 204);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<Renderer>().material.color = new Color(75, 0, 130);
        }
        else if(Input.GetKeyDown(KeyCode.N))
        {
            GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            GetComponent<Renderer>().material.color = new Color(255, 255, 0);
        }





    }
}
