using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public DragonAttack.AttackType attackType;
    public int damage = 1;
    public float attackDelay = 1.0f;
    
    // Use this for initialization
	void Start () {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.WallHit(damage, attackType, attackDelay);
            }
        }

    }
}
