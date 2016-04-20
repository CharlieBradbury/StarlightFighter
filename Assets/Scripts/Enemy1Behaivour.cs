using UnityEngine;
using System.Collections;

public class Enemy1Behaivour : MonoBehaviour {

    //References the playership
    private Transform trsPlayer;

    //Reference to the bullet that this unit shoots
    public GameObject gbjBullet;

    //Reference the bulletContainer to clean our hierarchy
    public GameObject gbjBulletContainer;

    //The range from where this unit starts attacking
    public static int iAttackRange = 130;

    //The Attack Cooldown for this enemy
    public static float fAttackCooldown = 1.0f;

    //Timer to destroy the unit after it has retreated, to avoid overcharging
    //the level (TTL = Time to live)
    private float fTTLAfterRetreate = 3.0f;

    //Flag to know if this unit just shoot
    private bool bJustShot;

    //Flag to know if the unit is retreating
    private bool bisRetreating;

    //Flag to make the unit rotate only once, and avoid making it spin
    private bool bRotateOnce;

    //Int to know the retreat direction
    private int iRetreatDirection;


	// Use this for initialization
	void Start () {

        //Initialize flags
        bJustShot = false;
        bisRetreating = false;
        bRotateOnce = false;

        //Locate the player in Unity space
        trsPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame, and in this script it handles the
    // Movement, the shooting and rotation of ship 1
	void Update () {

        //Update the shooting behaviour of this ship
        UpdateShooting();

        //Update the Movement of the ship
        UpdateMovement();

        //Update ship rotation
        UpdateRotation();
    }

    //Fixed Update handles all the timers this unit requires
    void FixedUpdate ()
    {
        //If the unit shooting is on cooldown
        if(bJustShot)
        {
            //reduce the timer
            fAttackCooldown -= Time.deltaTime;
        }

        //If the attack timer runs out, the unit can shoot again, reset the 
        //timer and the flag
        if (fAttackCooldown <= 0)
        {
            bJustShot = false;
            fAttackCooldown = 1.0f;
        }

        //If the unit is retreating
        if(bisRetreating)
        {
            //Reduce its time to live timer
            fTTLAfterRetreate -= Time.deltaTime;
        }

        //If the time to live timer runs out, destroy the unit
        if(fTTLAfterRetreate <=0)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Update shooting makes the unit shoot, depending on several conditions, 
    /// how far away is the player, if the player is still to this units left, 
    /// and also shoots only when the cooldown for shooting is down
    /// </summary>
    void UpdateShooting ()
    {
        //if this enemy is to the right of the player and they are relatively
        //close to each other and this unit hasn't just shooted
        if (trsPlayer.position.x < this.transform.position.x && this.transform.
            position.x - trsPlayer.position.x < iAttackRange && !bJustShot)

        {
            //Shoot, turn on the shooting flag
            bJustShot = true;
            
            //Instantiate the bullets, for each cannon the ship has.
            foreach (Transform child in this.transform)
            {
                //Create the bullet
                GameObject gbjBulletInstace = (GameObject) Instantiate(gbjBullet
                    , child.position, Quaternion.identity);

                //Store the bullet in the container to keep the heraichy clean
                gbjBulletInstace.transform.parent = 
                    gbjBulletContainer.transform;
            }
        }
    }

    void UpdateMovement ()
    {
        //Store the position of the unit
        Vector3 vc3UnitPos = this.transform.position;

        //Move to the right at a constant rate, all the time
        vc3UnitPos.x -= 0.5f;

        //If the unit isn't retreating
        if (!bisRetreating)
        {
            //Align its Y-Axis
            if (trsPlayer.position.y > this.transform.position.y)
            {
                //Move up
                vc3UnitPos.y += 0.1f;

            }
            else if (trsPlayer.position.y < this.transform.position.y)
            {
                //Move down
                vc3UnitPos.y -= 0.1f;
            }


            //If this ship has reached the player
            if (trsPlayer.position.x > this.transform.position.x)
            {
                //After you reach the player, go out of bounds and leave the 
                //playing field, decide whether to retreate up or down randomly
                iRetreatDirection = (int) Random.Range(0, 2);

                //Turn the retreat flag on
                bisRetreating = true;
            }
        }
        
        //If the unit is retreating
        if(bisRetreating)
        {
            //Check the retreate direction
            if(iRetreatDirection == 1)
            {
                //Retreate down
                vc3UnitPos.y -= 1.0f;
            }
            else if(iRetreatDirection == 0)
            {
                //Retreate up
                vc3UnitPos.y += 1.0f;
            }
        }

        //Update the position
        this.transform.position = vc3UnitPos;
    }

    void UpdateRotation ()
    {
        //Rotate Only if the unit is retreating
        if(bisRetreating)
        {
             //If the flag to rotate once is still off
            if(!bRotateOnce)
            {
                //Retreate corresponding to the correct direction
                // 1 is retreate up, 0 is retreate down
                if (iRetreatDirection == 1)
                {
                    transform.RotateAround(transform.position, transform.forward
                        , 20);
                }
                else if (iRetreatDirection == 0)
                {
                    transform.RotateAround(transform.position, transform.forward
                        , -20);
                }

                //Regardless of where the ship exited the screen turn the
                //rotate once flag on, because it already rotated somewhere
                bRotateOnce = true;
            }  
        }
     
    }
}
