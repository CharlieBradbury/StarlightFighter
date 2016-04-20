using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    //The time this proyectile exists in-game before it's destroyed
    public float fTimeToLive = 2.0f;

    //The speed of the bullet
    public int iBulletSpeed = 3;

    // Use this for initialization
    void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {

        //Update the bullet
        UpdateBullet();
	
	}


    void FixedUpdate ()
    {
        //Decrease the time to live of the bullet with time
        fTimeToLive -= Time.deltaTime;  
    }

    /// <summary>
    /// This function updates the position of the bullet and destroys such
    /// bullet if neccesary
    /// </summary>
    void UpdateBullet ()
    {
        //Store the initial position of the bullet
        Vector3 vc3BulletPos = this.transform.position;

        //Update the position of the bullet
        if(this.gameObject.layer == 11) //11 -> playershot
        {
            vc3BulletPos.x += iBulletSpeed;
        }
        else if (this.gameObject.layer == 12) //12 ->enemyshot
        {
            vc3BulletPos.x -= iBulletSpeed;
        }
            

        //Assign the new position to the bullet
        this.transform.position = vc3BulletPos;

        //Check if the bullet lifespan is over, if it is, destroy it
        if (fTimeToLive <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
