using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{


    private Player player;
    private GameObject gameArea;
    private GameObject[] dragonAttackButtons;
    private GameObject[] specialAttackButtons;
    private GameObject specialAttackSelectd;
    private int attackTouchIndex = -1;

    // Use this for initialization
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameArea = transform.FindChild("GameArea").gameObject;
        dragonAttackButtons = GameObject.FindGameObjectsWithTag("AttackChooser");
        specialAttackButtons = GameObject.FindGameObjectsWithTag("SpecialAttack");

    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < Input.touchCount; i++)
        {

            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            Vector2 touchPos = new Vector2(wp.x, wp.y);

            // If the touch is starting
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {

                // If the touch is in the gamearea
                if (gameArea.collider2D == Physics2D.OverlapPoint(touchPos))
                    gameAreaTouchHandler(touchPos, i);

                // If the touch is on the attack chooser
                for (int j = 0; j < dragonAttackButtons.Length; j++)
                    if (dragonAttackButtons[j].collider2D == Physics2D.OverlapPoint(touchPos))
                        attackChooserTouchHandler(touchPos, j);

                // If the touch is on the special attack
                for (int j = 0; j < specialAttackButtons.Length; j++)
                    if (specialAttackButtons[j].collider2D == Physics2D.OverlapPoint(touchPos))
                    {
                        specialAttackSelectd = specialAttackButtons[j];
                        specialAttacksTouchHandler(touchPos, i);

                    }
            }

            // If the touch is a change
            if (Input.GetTouch(i).phase == TouchPhase.Moved)
            {

                // If the touch is on the special attack

                if ((specialAttackSelectd != null) && (specialAttackSelectd.collider2D == Physics2D.OverlapPoint(touchPos)))
                {
                    specialAttackSelectd.GetComponent<SpecialAttackButton>().setButtonPosition(touchPos);

                    // No need to check the game area
                    return;
                }



                // If the touch is in the gamearea
                if (gameArea.collider2D == Physics2D.OverlapPoint(touchPos))
                    gameAreaTouchHandler(touchPos, i);
            }

            // If the touch is an end
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {

                if (i == attackTouchIndex)
                    i = -1;

                if ((specialAttackSelectd != null) && (specialAttackSelectd.collider2D == Physics2D.OverlapPoint(touchPos)))
                    specialAttackSelectd.GetComponent<SpecialAttackButton>().StartAttack(touchPos);
                specialAttackSelectd = null;

            }

        }







#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);

            if (gameArea.collider2D == Physics2D.OverlapPoint(touchPos))
                player.StartAttack(touchPos);



            for (int j = 0; j < dragonAttackButtons.Length; j++)
                if (dragonAttackButtons[j].collider2D == Physics2D.OverlapPoint(Input.mousePosition))
                {
                    Debug.Log("A button was pressed miLord");
                    player.setActiveAttack(dragonAttackButtons[j].GetComponent<DragonAttackButton>().attack);
                }


        }


#endif

    }

    private void gameAreaTouchHandler(Vector2 touchPos, int touchIndex)
    {

        // If no other touch is currently generating an attack
        if ((attackTouchIndex == -1) || (attackTouchIndex == touchIndex))
        {
            player.StartAttack(touchPos);
            attackTouchIndex = touchIndex;
        }

    }
    private void attackChooserTouchHandler(Vector2 touchPos, int attackButtonIndex)
    {

        player.setActiveAttack(dragonAttackButtons[attackButtonIndex].GetComponent<DragonAttackButton>().attack);

    }
  

}
