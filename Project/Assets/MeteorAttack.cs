using UnityEngine;
using System.Collections;

public class MeteorAttack : MonoBehaviour {

    public float lifeTime = 2.0f;
    public float changeAngleFactor = 3f;
    public float speed;
    public GameObject explosion;

    private CameraShake camShake;
    private float startTime;
    private float shakeFactor = 0.6f;
   
    
    // Use this for initialization
	void Start () {

        startTime = Time.time;
        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
	
	}
    
	
	// Update is called once per frame
	void Update () {

        if (Time.time > startTime + lifeTime)
        {
            explode();
        }

        float angle = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        Debug.Log("Meteor velocity: " + (transform.forward * speed));
 
	
	}

    private void explode()
    {
        camShake.shakeForDragonAttack = shakeFactor;
        GameObject explosionEffect = (GameObject)Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, -1), transform.rotation);
        GameObject.Destroy(gameObject);
    }
}
