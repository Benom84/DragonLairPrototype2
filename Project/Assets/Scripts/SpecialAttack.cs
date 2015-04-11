using UnityEngine;
using System.Collections;

public class SpecialAttack : MonoBehaviour {

    public ArrayList enemiesInDamageArea;

    void Awake()
    {

        enemiesInDamageArea = new ArrayList();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Special Attack: Someone entered me!!");
        
        if (other.gameObject.tag == "Enemy")
        {
            enemiesInDamageArea.Add(other.gameObject);
            Debug.Log("Added an enemy to attack");
        }
            
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
            if (enemiesInDamageArea.Contains(other.gameObject))
            {
                Debug.Log("Removod an enemy to attack");
                enemiesInDamageArea.Remove(other.gameObject);
            }
                

    }
}
