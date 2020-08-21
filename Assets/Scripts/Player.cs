using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] float moveSpeed = 10f; // change this in the Inspector to get the Player move speed to what feels best
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        //in Unity go to Edit - Project Settings - Input Manager (chose Horizontal and Vertical keys and mouse axis for this one)
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed; //Time.delta.Time ensures framerate independent whether on fast or slow computer
        //Debug.Log(deltaX);
        var newXPos = transform.position.x + deltaX; //new Player position equal to current position plus the key press above

        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        //Debug.Log(deltaY);
        var newYPos = transform.position.y + deltaY;

        transform.position = new Vector2(newXPos, newYPos); //Move Player to new position
    }
}
