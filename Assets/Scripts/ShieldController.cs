using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour {

   float fShieldDuration;
    
	void Start () {

        fShieldDuration = 1.0f;

	}
	
	// Update is called once per frame
	void Update () {
	    
        if(fShieldDuration<=0)
        {
            if(this.transform.parent.tag == "Player")
            {
                Debug.Log("Shield is down");
                MainShipController.mscInstance.bShieldActive = false;
            }
            Destroy(this.gameObject);
        }
	}

    void FixedUpdate ()
    {
        fShieldDuration -= Time.deltaTime;
    }
}
