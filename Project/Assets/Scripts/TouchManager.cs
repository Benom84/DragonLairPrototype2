using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{

    private Player player;
    private GameObject gameArea;
    private GameObject[] dragonAttackButtons;
    private GameObject[] specialAttackButtons;
    private GameObject selectedSpecialAttack;
    private int attackTouchIndex = -1;

    // Use this for initialization
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameArea = transform.FindChild("GameArea").gameObject;
        dragonAttackButtons = GameObject.FindGameObjectsWithTag("AttackChooser");
        specialAttackButtons = GameObject.FindGameObjectsWithTag("SpecialAttack");
        Debug.Log("Size of special attacks is: " + specialAttackButtons.Length);

    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < Input.touchCount; i++)
        {

            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            Vector2 touchPos = new Vector2(wp.x, wp.y);

            // If the touch is on the game area
            if (gameArea.collider2D == Physics2D.OverlapPoint(touchPos))
                gameAreaTouchHandler(touchPos, i);

            // If the touch is on the attack chooser
            for (int j = 0; j < dragonAttackButtons.Length; j++)
                if (dragonAttackButtons[j].collider2D == Physics2D.OverlapPoint(touchPos))
                    attackChooserTouchHandler(touchPos, i, j);


            // If the touch is on the special moves - TODO
            for (int j = 0; j < specialAttackButtons.Length; j++)
            {
                //Debug.Log("Checking if it is a special");
                if (specialAttackButtons[j].collider2D == Physics2D.OverlapPoint(touchPos))
                {
                    specialAttackTouchHandler(touchPos, i, j);
                    Debug.Log("The index is: " + j);
                }
                    
            }
                
                
            

            
            
            // Making sure that no matter where the attack touch ended it is initialize
            if ((attackTouchIndex == i) && (Input.GetTouch(i).phase == TouchPhase.Ended))
                attackTouchIndex = -1;


            

            // If the touch is on another UI - In the future



        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);

            if (gameArea.collider2D == Physics2D.OverlapPoint(touchPos))
                player.StartAttack(touchPos);



            for (int j = 0; j < dragonAttackButtons.Length; j++)
                if (dragonAttackButtons[j].collider2D == Physics2D.OverlapPoint(touchPos))
                {
                    Debug.Log("A button was pressed miLord");
                    player.setActiveAttack(dragonAttackButtons[j].GetComponent<DragonAttackButton>().attack);
                }

            // If the touch is on the special moves - TODO
            for (int j = 0; j < specialAttackButtons.Length; j++)
            if (specialAttackButtons[j].collider2D == Physics2D.OverlapPoint(touchPos))
            {
                specialAttackTouchHandler(touchPos, 0, j);
                Debug.Log("The index is: " + j);
            }

        }


#endif

    }

    private void gameAreaTouchHandler(Vector2 touchPos, int touchIndex)
    {

        if (Input.GetTouch(touchIndex).phase != TouchPhase.Ended)
        {


            // If no other touch is currently generating an attack
            if ((attackTouchIndex == -1) || (attackTouchIndex == touchIndex))
            {
                player.StartAttack(touchPos);
                attackTouchIndex = touchIndex;
            }
        }
    }
    private void attackChooserTouchHandler(Vector2 touchPos, int touchIndex, int attackButtonIndex)
    {

        if (Input.GetTouch(touchIndex).phase == TouchPhase.Began)
            player.setActiveAttack(dragonAttackButtons[attackButtonIndex].GetComponent<DragonAttackButton>().attack);

    }
    private void specialAttackTouchHandler(Vector2 touchPos, int touchIndex, int specialAttackIndex) 
    {
        Debug.Log("The specialAttackTouchHandler was called");
        specialAttackButtons[specialAttackIndex].GetComponent<SpecialAttackButton>().StartAttack(touchPos);
        return;


        if (Input.GetTouch(touchIndex).phase == TouchPhase.Began)
        {
            
            selectedSpecialAttack = specialAttackButtons[specialAttackIndex];
            Debug.Log("A touch has began on " + selectedSpecialAttack.name);
        }



        if ((selectedSpecialAttack == specialAttackButtons[specialAttackIndex]) && (Input.GetTouch(touchIndex).phase == TouchPhase.Moved))
        {
            selectedSpecialAttack.GetComponent<SpecialAttackButton>().setButtonPosition(touchPos);
            Debug.Log("A touch is going on on " + selectedSpecialAttack.name);
        }
            
    }

}