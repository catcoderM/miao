using UnityEngine;
using System.Collections;

public class AirEnemy : Enemy {
	
	// Update is called once per frame
	void Update () {
        RotateTo();
        MoveTo();
        Fly();
	}

    public void Fly()
    {
        float flySpeed = 0;
        if (this.transform.position.y < 2.0f)
        {
            flySpeed = 1.0f;
        }
        this.transform.Translate(new Vector3(0,flySpeed*Time.deltaTime,0));
    }
}
