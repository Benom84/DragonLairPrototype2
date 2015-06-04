using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour
{


    private Player player;
    private Collider2D dragonBody;
    private GameObject gameArea;
    private int attackTouchIndex = -1;
    private Animator playerAnimator;
    private float playerMouthX;
    private float playerMouthY;
    private int lastAttackDirection = 1;
    private RotateTowards rotateTowards;

    // Use this for initialization
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        gameArea = transform.FindChild("GameArea").gameObject;
        rotateTowards = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<RotateTowards>();

        // Calculate where the player mouth is
        Transform playerMouth = GameObject.FindGameObjectWithTag("Player").transform.FindChild("Head").transform.FindChild("Mouth");
        playerMouthX = playerMouth.position.x;
        playerMouthY = playerMouth.position.y;

    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < Input.touchCount; i++)
        {

            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            Vector2 touchPos = new Vector2(wp.x, wp.y);


            // If the touch is on the game area
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (gameArea.collider2D == Physics2D.OverlapPoint(touchPos, LayerMask.GetMask("TouchArea")))
                    gameAreaTouchHandler(touchPos, i);
            }

            // Making sure that no matter where the attack touch ended it is initialize
            if ((attackTouchIndex == i) && (Input.GetTouch(i).phase == TouchPhase.Ended))
            {
                attackTouchIndex = -1;
                //lastAttackDirection = -1;
                //playerAnimator.SetTrigger("endAttack");
            }


        }

#if UNITY_EDITOR
        //For the editor
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);


            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (gameArea.collider2D == Physics2D.OverlapPoint(touchPos, LayerMask.GetMask("TouchArea")))
                {
                    rotateTowards.destinationPosition = wp;
                    player.StartAttack(touchPos);
                }
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

                /*
                if (Input.GetTouch(touchIndex).phase == TouchPhase.Began)
                    playerAnimator.SetTrigger("startAttack");
                else
                { */
                int currentAttackDirection = calculateAngle(touchPos);
                if (lastAttackDirection != currentAttackDirection)
                {
                    lastAttackDirection = currentAttackDirection;
                    //playerAnimator.SetTrigger("attackChanged");
                    playerAnimator.SetInteger("attackDirection", lastAttackDirection);

                }
                //}

                player.StartAttack(touchPos);
                attackTouchIndex = touchIndex;



            }
        }
    }

    private int calculateAngle(Vector2 touchPos)
    {
        float diffX = touchPos.x - playerMouthX;
        float diffY = touchPos.y - playerMouthY;

        float result = (diffY / diffX);
        // Debug.Log("Calculate angle is: " + result);

        if (result > 0)
            result = 0;

        if (result < -0.4f)
            result = -0.4f;

        int resultInt = -(int)Mathf.Round(result * 10);

        if (resultInt > 4)
            resultInt = 4;

        if (resultInt < 0)
            resultInt = 0;
        //Debug.Log("Calculate angle after round is: " + result);


        return (resultInt + 1);
    }


}