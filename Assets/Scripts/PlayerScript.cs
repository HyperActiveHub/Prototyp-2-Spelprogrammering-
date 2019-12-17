using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public static PlayerScript Instance;

    Rigidbody2D rb;
    [Tooltip("The base hight the player jumps on each platform. This can be increased with power-ups.")]
    [SerializeField]
    float jumpHeight = 500;
    [Tooltip("How fast the player can move sideways.")]
    [SerializeField]
    float strafeSpeed = 20;
    Vector2 topRight, bottomLeft;
    Camera mainCam;
    float jumpForce;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpForce = jumpHeight;
        mainCam = Camera.main;
        topRight = mainCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeft = mainCam.ScreenToWorldPoint(Vector2.zero);
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
#if UNITY_EDITOR
        topRight = mainCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeft = mainCam.ScreenToWorldPoint(Vector2.zero);
        //jumpForce = jumpHeight;
#endif

        float h = Input.GetAxis("Horizontal");
        transform.position = new Vector3(h * strafeSpeed * Time.deltaTime + transform.position.x, transform.position.y, transform.position.z);

        //Ändringsbarhet: skärmstorlek/aspect-ratio kan förändras fritt.
        ScreenWrapping(bottomLeft.x, topRight.x);

        DeathFall(bottomLeft.y);

    }


    void ScreenWrapping(float left, float right)
    {
        if (transform.position.x < left)
        {
            transform.position = new Vector3(right, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > right)
        {
            transform.position = new Vector3(left, transform.position.y, transform.position.z);
        }
    }

    void DeathFall(float bottom)
    {
        if(transform.position.y < bottom)
        {
            Debug.Log("Game Over");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.usedByEffector)
        {
            if (rb.velocity.y <= 0)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpForce);
            }
        }
        //else hit enemy = die/get hit

    }

    public void AddJumpHeight(float addedHeight)
    {
        jumpForce += addedHeight;
    }

    public void DecreaseJumpHeight(float decreacedHeight)
    {
        jumpForce -= decreacedHeight;
    }

    public void ResetJumppHeight()
    {
        jumpForce = jumpHeight;
    }





}
