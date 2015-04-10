using UnityEngine;
using System.Collections;

public class DragonAttack : MonoBehaviour {

    public enum AttackType { Fire, Water, Air, Earthquake, Scream };

    public AttackType attackType;
    [HideInInspector]
    public int attackDamage = 1;

    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D collider)
    {

        if (collider.tag == "Border")
            GameObject.Destroy(gameObject);

        if (collider.tag == "Enemy")
        {

            collider.GetComponent<Enemy>().Hit(attackDamage, attackType);
            GameObject.Destroy(gameObject);
        }

    }
}
