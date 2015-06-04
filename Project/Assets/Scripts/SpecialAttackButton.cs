using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialAttackButton : MonoBehaviour
{

    public enum SpecialAttackType { Earthquake, Tail, Fireball };

    public SpecialAttackType attackType;
    public int damage = 2;
    public int numberOfAttacksLeft = 1;
    public float attackDelay = 1.0f;

    private Text numberOfAttacksText;
    private float lastAttackTime;

    // Use this for initialization
    void Start()
    {

        numberOfAttacksText = transform.Find("NumberOfAttacksText").GetComponent<Text>();
        numberOfAttacksText.text = "" + numberOfAttacksLeft;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setButtonPosition(Vector3 position)
    {

        transform.position = position;

    }




}
