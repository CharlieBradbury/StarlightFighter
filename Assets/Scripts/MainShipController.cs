using UnityEngine;
using System.Collections;

public class MainShipController : MonoBehaviour {

    //Reference to the blue ship sprite for teleporting
    public Sprite sprBlueTeleportSprite;

    //Reference to the default sprite
    public Sprite sprDefaultSprite;

    //Reference to the shooting proyectile of the ship
    public GameObject gbjProyectile;

    //Reference to the shield of this unit
    public GameObject gbjPlayerShield;

    //Reference to an empty gameobject that stores all the bullets from the
    //the PlayerShip, we do this to mantain our heirarchy in the unity editor
    //clean
    public GameObject gbjContainer;

    //Height of the Ship
    private static int iMainShipHeight = 10;

    //Width of the Ship
    private static int iMainShipWidth = 20;

    //Flag for the Ships Shield
    public bool bShieldActive;

    //This is an instance of this script, use it to access public variables of 
    //this script in other scripts
    public static MainShipController mscInstance { get; protected set; }

	// Use this for initialization
	void Start () {

        //Assign the MainController script as this
        mscInstance = this;

        //Initialize flag
        bShieldActive = false;
	}
	
	// Update is called once per frame, and it handles all the movement, the
    //rotations and the shooting
	void Update () {

        //Update the position of the MainShip
        UpdateMovement();

        //Update the shooting of the MainShip
        UpdateShooting();

        //Update the behaviour of special abilities
        UpdateSpecial();
	}

    //Fixed update handles all the timers
    void FixedUpdate ()
    {

    }

    /// <summary>
    /// This Function Updates the position of the player ship, a player can use
    /// the arrows to move left, right, up and down.
    /// </summary>
    void UpdateMovement()
    {
        //Store the current position of the Player
        Vector3 vc3PlayerPos = this.transform.position;

        //If the player presses the up arrow
        if(Input.GetKey("up"))
        {
            //Move him Up
            vc3PlayerPos.y += 1;
        }

        //If the player presses the up key, tilt the ship 20 degrees up
        if (Input.GetKeyDown("up"))
        {
            //Tilt the ship up by 20 degrees, upwards
            transform.RotateAround(transform.position, transform.forward, 20);
        }

        //When the player releases the up key return the ship to its normal
        //rotation
        if (Input.GetKeyUp("up"))
        {
            //Return the ship to its normal rotation
            transform.RotateAround(transform.position, transform.forward, -20);
        }

        //If the player presses the down arrow
        if (Input.GetKey("down"))
        {
            //Move him down
            vc3PlayerPos.y -= 1;
        }

        //If the player pressed the down key, tilt the ship 20 degrees down
        if (Input.GetKeyDown("down"))
        {
            //Tilt the ship up by 20 degrees, downwards
            transform.RotateAround(transform.position, transform.forward, -20);
        }

        //When the player releases the down key return the ship to its normal
        //rotation
        if (Input.GetKeyUp("down"))
        {
            //Return the ship to its normal rotation
            transform.RotateAround(transform.position, transform.forward, 20);
        }

        //If the player presses the right arrow
        if (Input.GetKey("right"))
        {
            //Move him right
            vc3PlayerPos.x += 1;
        }

        //If the player presses the left arrow
        if (Input.GetKey("left"))
        {
            //Move him left
            vc3PlayerPos.x -= 1;
        }

        //Make sure the new position is valid
        KeepInbound(ref vc3PlayerPos);

        //Assign the new position to the MainShip
        this.transform.position = vc3PlayerPos;
    }

    /// <summary>
    /// This Function creates the proyectiles that are being shot by the main
    /// space ship
    /// </summary>
    void UpdateShooting()
    {
        if(Input.GetKeyDown("space"))
        {
            //Create the GameObject
             GameObject gbjInstance = (GameObject) Instantiate(gbjProyectile, 
                 this.transform.position, Quaternion.identity);

            //Set the proyectile as son of the Bullet Container
            gbjInstance.transform.parent = gbjContainer.transform;

        }
    }

    /// <summary>
    /// This Function updates all the special abilites, shields, jumps, rockets
    /// etc
    /// </summary>
    void UpdateSpecial()
    {
        //If the user pressed the shield button and there isn't an active shield already
        if(Input.GetButtonDown("Fire1") && !bShieldActive) //Fire 1 is left-ctrl, for now...
        {
            Debug.Log("Shield is Up");

            //Create the shield on top of the ship, and grab store it
            GameObject gbjShield = (GameObject) Instantiate(gbjPlayerShield, 
                this.transform.position, Quaternion.identity);


            //Make the shield child of the ship, so that it moves with him
            gbjShield.transform.parent = this.transform;

            //Turn the flag on
            bShieldActive = true;
        }

        //If the user pressed the teleport button
        if(Input.GetButtonDown("Fire2")) //Fire 2 is Left-Alt, for now...
        {
            if(Input.GetKeyDown("down"))
            {
                Debug.Log("Blink Down!");

                this.transform.GetComponent<SpriteRenderer>().sprite = 
                    sprBlueTeleportSprite;

                Vector3 vc3NewPos = this.transform.position;

                vc3NewPos.y -= 8;

                this.transform.position = vc3NewPos;

                this.transform.GetComponent<SpriteRenderer>().sprite =
                    sprDefaultSprite;
            }
            
        }
    }

    /// <summary>
    /// This Function restrains the position of the player, to keep him inbound
    /// inbound means staying inside the orthographic proyection of the main
    /// camera, While the boundaries are static in our y-coordinates, the
    /// x-coordinates are dynamic, and therefore we must calculate a screen
    /// ratio ourselfs
    /// </summary>
    /// <param name="vc3PlayerPos"> The position to check </param>
    void KeepInbound (ref Vector3 vc3PlayerPos)
    {
        //Check Upper Bound and Lower Bound first.

        //if the position is greater than the upper bound
        if(vc3PlayerPos.y + iMainShipHeight/2 > Camera.main.orthographicSize)
        {
            //Make the position the upperbound
            vc3PlayerPos.y = Camera.main.orthographicSize - iMainShipHeight/2;
        }

        //if the position is less than the lower bound
        if (vc3PlayerPos.y - iMainShipHeight/2 < -Camera.main.orthographicSize)
        {
            //Make the position the lowerbound
            vc3PlayerPos.y = -Camera.main.orthographicSize + iMainShipHeight/2;
        }

        //Now Calculate the horizontal ratio of the current camera proyection
        float fScreenRatio = (float) Screen.width / (float) Screen.height;

        //The new orthographic proyection we are looking for is the screen ratio
        //times the camera's orthograhic size
        float fOrthographicWidth = fScreenRatio * Camera.main.orthographicSize;

        //With this, we can set the left and right boundaries

        //if the position is greater than the right bound
        if (vc3PlayerPos.x + iMainShipWidth / 2 > fOrthographicWidth)
        {
            //Make the position the right bound
            vc3PlayerPos.x = fOrthographicWidth - iMainShipWidth/2;
        }

        //if the position is less than the left bound
        if (vc3PlayerPos.x - iMainShipWidth / 2 < -fOrthographicWidth)
        {
            //Make the position the lowerbound
            vc3PlayerPos.x = -fOrthographicWidth + iMainShipWidth / 2;
        }
    }

}
