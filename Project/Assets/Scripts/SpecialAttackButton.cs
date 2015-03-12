using UnityEngine;
using System.Collections;

public class SpecialAttackButton : MonoBehaviour {

    public enum SpecialAttackType { Earthquake, Tail, Fireball };

    public SpecialAttackType attackType;
    public int damage;
    public int numberOfAttacksLeft = 1;

    private CameraShake camShake;
    
    // Use this for initialization
	void Start () {

        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setButtonPosition(Vector3 position)
    {

        transform.position = position;

    }
    
    public void StartAttack(Vector3 attackPosition)
    {

        if ((attackType == SpecialAttackType.Earthquake) && (numberOfAttacksLeft > 0))
        {
            camShake.shake = 1.0f;
            numberOfAttacksLeft--;
        }
            

    }
}
