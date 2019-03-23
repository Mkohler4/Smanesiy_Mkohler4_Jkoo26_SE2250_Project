using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;
    public bool offRight, offLeft, offUp, offDown;

    void Awake()
    {
        //Gets the width and height of the game screen
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offUp = false;
        offLeft = offDown = false;

        //if the x position is off the screen to the right
        if (pos.x > camWidth - radius)
        {
            //set isoncreen to false and declare witch direction its off the screen
            pos.x = camWidth - radius;
            isOnScreen = false;
            offRight = true;
        }
        //if the x position is off the screen to the left
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            isOnScreen = false;
            offLeft = true;
        }
        //if the y position is off the sceen at the top
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            isOnScreen = false;
            offUp = true;
        }
        //if the y position is off the screen at the bottom
        if (pos.y < -camHeight + radius + 3)
        {
            pos.y = -camHeight + radius + 3;
            isOnScreen = false;
            offDown = true;
        }
        isOnScreen = !(offRight || offLeft || offUp || offDown);
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
        }
        void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
            Gizmos.DrawWireCube(Vector3.zero, boundSize);
        }
    }
}
