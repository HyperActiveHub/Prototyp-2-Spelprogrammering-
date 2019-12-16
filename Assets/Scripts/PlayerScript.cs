using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float jumpForce = 500;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Make powerups using existing functions and attributes (maybe in the form of Scriptable Objects?) to enable 
        //easy creation of new power-ups through drag 'n drop. 
        //List of functions the power-up 'activates'
        //Should all functions run for the same amount of time? (bool)
        //Time until power-up runs out (list of time values if above is false)
        //Attributes for each function (if needed, ex. Extra jump height - float)
        //Sprite for the pwr-up
        //Listen for events and execute a function when called (ex. player hit by enemy - kill enemy, destroy power-up)


        //Make Enemies in the same way
        //Make it easy to change collider shape
        //Rarity value (spawn chance)
        //Can be stomped? - bool
        //Warning sound (sound that goes off some seconds before the player reaches (sees) the enemy

        //Platformns 
        //Is moveable (can player move this platform using input?)
        //is moving, offset min - max, movespeed (if platform moves out of screen - left or right, behaviour should be same as player, i.e it appears on the opposite side)
        //Is breakable, break sprite, health (jumps until break, min = 0 i.e player falls through)
        //Listen for events and execute a function when called (ex. player jumped on platform - break platform, move platform down)


    }

    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.usedByEffector)
        {
            if(rb.velocity.y <= 0)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpForce);

            }
        }
        
    }





}
