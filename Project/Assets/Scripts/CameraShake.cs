using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    

    // How long the object should shake for.
    public float shake = 0f;
    public float shakeForDragonAttack;
    public bool isSpecialAttackShake = false;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;
    private GameController gameController;
    private Transform camTransform;


    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    
    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void FixedUpdate()
    {
        if (isSpecialAttackShake)
        {
            if ((shake > 0) && (!gameController.gameEnded))
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
                shake -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shake = 0f;
                shakeForDragonAttack = 0f;
                isSpecialAttackShake = false;
                camTransform.localPosition = originalPos;
            }
        }
        else
        {
            if ((shakeForDragonAttack > 0) && (!gameController.gameEnded))
            {
                Vector3 newPos = camTransform.localPosition;
                newPos.x = originalPos.x + Random.insideUnitSphere.x * shakeForDragonAttack;
                camTransform.localPosition = newPos;
                shakeForDragonAttack -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeForDragonAttack = 0f;
                camTransform.localPosition = originalPos;
            }
        }
        

        
    }
}
