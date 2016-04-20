using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour {

    //The default layer of this Gameobject
    private int iDefaultLayer;

    //The health of this unit
    private int iHp;

    //The Amount of invulnerability after getting hit of this unit
    private float fInvTimer;

    //The fixed amount of time a unit blinks when it gets hit
    private float fBlinkTimer = 0.10f;

	// Use this for initialization
	void Start () {

        //Assign the Default Layer of this object as the one it starts with
        iDefaultLayer = this.gameObject.layer;

        //Assign the correct hp depending on its layer, proyectiles have hp of 1
        setHp(this.gameObject.layer);

        //Assign the correct invulnerability timer depending on its layer
        setInvTimer(this.gameObject.layer);
        
        
	}
	
	// Update is called once per frame
	void Update () {

        //If the unit has no hitpoints left
        if(iHp <= 0)
        {
            //Destroy it
            Destroy(this.gameObject);
        }

        //If the timer of invulnerability has run out, make the unit vunerable
        //again, and reset the timer
        if(fInvTimer <= 0)
        {
            //Make vulnerable
            this.gameObject.layer = iDefaultLayer;

            //if the unit ended its blink cycle with its sprite renderer 
            //disabled, enable it
            if(this.GetComponent<SpriteRenderer>().enabled == false)
            {
                this.GetComponent<SpriteRenderer>().enabled = true;
            }

            //Reset Invensibility timer
            setInvTimer(iDefaultLayer);
        }
	
	}

    void FixedUpdate()
    {
        //If the unit is invulnerable
        if (this.gameObject.layer == 10)
        {

            //Reduce it's invulnerability timer
            fInvTimer -= Time.deltaTime;

            //Reduce the blink timer
            fBlinkTimer -= Time.deltaTime;

            //Disable and enable the sprite renderer to make it look like the
            //image is blinking

            //If the unit is allowed to blink, then blink
            if(fBlinkTimer<=0)
            {
                //To blink if the sprite renderer is enabled, we disable it
                if (this.GetComponent<SpriteRenderer>().enabled == true)
                {
                    //Disable the sprite renderer
                    this.GetComponent<SpriteRenderer>().enabled = false;
                }
                else //if the sprite renderer is off however, then we enable it
                {
                    //Enable the sprite renderer
                    this.GetComponent<SpriteRenderer>().enabled = true;
                }

                //Reset the blink timer
                fBlinkTimer = 0.10f;
            }
        }
    }

    //If this unit collides with something it should interact with
    void OnTriggerEnter2D ()
    {
        //Reduce the Health of this Unit by 1       //Note: I don't like this, cause we can't control how much health something losses
        iHp--;                                      // i guess we can eventually change it to check with what exactly did you collided
                                                    // for now everything takes 1 hp though...

        //Make the unit invulnerable by changing its layer
        this.gameObject.layer = 10; // 10 -> invulnerable
    }

    /// <summary>
    /// This function assigns the correct number of hp depending on its layer
    /// </summary>
    /// <param name="iLayer"> the layer of the gameobject </param>
    void setHp (int iLayer)
    {
        switch (iLayer)
        {
            case 8: //Player
                iHp = 10;
                break;
            case 9: // NormalEnemyShips
                iHp = 2;
                break;
            case 11: // Friendly Proyectiles
                iHp = 1;
                break;
            case 12: //Enemy Proyectiles
                iHp = 1;
                break;
            default:
                Debug.LogError("Unit " + this.transform.name + " has no layer");
                break;
        }

    }

    /// <summary>
    /// This function assigns the correct number for the invulnerability 
    /// depending on its layer
    /// </summary>
    /// <param name="iLayer"> the layer of the gameobject </param>
    void setInvTimer (int iLayer)
    {
        switch (iLayer)
        {
            case 8: //Player
                fInvTimer = 2.0f;
                break;
            case 9: // NormalEnemyShips
                fInvTimer = 1.0f;
                break;
            case 11: //Friendly Proyectiles
                fInvTimer = 0.0f;
                break;
            case 12: //Enemy Proyectiles
                fInvTimer = 0.0f;
                break;
            default:
                Debug.LogError("Unit " + this.transform.name + " has no layer");
                break;
        }
    }
}
