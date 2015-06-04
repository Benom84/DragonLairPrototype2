using UnityEngine;
using System.Collections;

public class RotateTowards : MonoBehaviour {
    [HideInInspector]
    public Vector3 destinationPosition;
    public float speed = 1.0f;

    private float maxAngleChange = 30;
    private Player playerScript;
    private Transform mouthTransform;
    
    // Use this for initialization
	void Start () {
        destinationPosition = this.transform.position;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mouthTransform = transform.FindChild("Mouth");
	}

    void Update()
    {
        Vector3 vectorToTarget = destinationPosition - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        angle = angle > 0 ? Mathf.Min(maxAngleChange, angle) : Mathf.Max(-maxAngleChange, angle);
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        mouthTransform.rotation = transform.rotation;
        playerScript.attackRotation = transform.rotation;
    }
}
