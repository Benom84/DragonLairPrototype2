using UnityEngine;
using System.Collections;

public class Swipper : MonoBehaviour
{

    public static Swipper swipper;

    private Vector3 startPosition;

    public float scrollSpeed;
    public float tileSizeX;

    private bool startedLoading;
    private bool finishedLoading;

    private string nameOfNewRoot;
    private string nameOfOldRoot;
    private eDirectionOfSwipe swipeDirection;

    GameObject newRoot;
    GameObject oldRoot;

    Vector3 startPositionNewRoot;
    Vector3 startPositionOldRoot;

    void Awake()
    {
        if (swipper == null)
        {
            DontDestroyOnLoad(gameObject);
            swipper = this;
        }
        else if (swipper != this)
        {
            Destroy(gameObject);
        }
        startedLoading = false;
        finishedLoading = false;
    }

    public void loadScene(string oldSceneName, string newSceneName, eDirectionOfSwipe directionToSwipe)
    {
        Application.LoadLevelAdditive(newSceneName);
        startedLoading = true;

        nameOfNewRoot = "root" + newSceneName;
        nameOfOldRoot = "root" + oldSceneName;

        swipeDirection = directionToSwipe;
    }

    //private void moveScenesLeft()
    //{
    //    GameObject newRoot = GameObject.Find(nameOfNewRoot);
    //    Vector3 startPositionNewRoot = newRoot.transform.position;

    //    GameObject oldRoot = GameObject.Find(nameOfOldRoot);
    //    Vector3 startPositionOldRoot = oldRoot.transform.position;

    //    newRoot.transform.position = new Vector3(newRoot.transform.position.x + 20, newRoot.transform.position.y);

    //    while (true)
    //    {
    //        if (newRoot.transform.position == new Vector3(0, 0, 0))
    //        {
    //            break;
    //        }

    //        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeX);
    //        oldRoot.transform.position = startPositionOldRoot + Vector3.left * newPosition;

    //        newRoot.transform.position = startPositionNewRoot + Vector3.left * newPosition;
    //    }

    //    GameObject.Destroy(oldRoot);
    //}

    //private void moveScenesRight()
    //{
    //    GameObject newRoot = GameObject.Find(nameOfNewRoot);
    //    Vector3 startPositionNewRoot = newRoot.transform.position;

    //    GameObject oldRoot = GameObject.Find(nameOfOldRoot);
    //    Vector3 startPositionOldRoot = oldRoot.transform.position;

    //    newRoot.transform.position = new Vector3(newRoot.transform.position.x - 20, newRoot.transform.position.y);

    //    while (true)
    //    {
    //        if (newRoot.transform.position == new Vector3(0, 0, 0))
    //        {
    //            break;
    //        }

    //        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeX);
    //        oldRoot.transform.position = startPositionOldRoot + Vector3.right * newPosition;

    //        newRoot.transform.position = startPositionNewRoot + Vector3.right * newPosition;
    //    }

    //    GameObject.Destroy(oldRoot);
    //}

    void Update()
    {
        if (finishedLoading && !(newRoot.transform.position == new Vector3(0, 0)))
        {
            //float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeX);
            //Debug.Log("trying to move");
            //switch (swipeDirection)
            //{
            //    case eDirectionOfSwipe.right:
            //        oldRoot.transform.position = startPositionOldRoot + Vector3.right * newPosition;
            //        newRoot.transform.position = startPositionNewRoot + Vector3.right * newPosition;
            //        break;
            //    case eDirectionOfSwipe.left:
            //        oldRoot.transform.position = startPositionOldRoot + Vector3.left * newPosition;
            //        newRoot.transform.position = startPositionNewRoot + Vector3.left * newPosition;
            //        break;
            //}
        }
        else if (newRoot != null)
        {
            if (newRoot.transform.position == new Vector3(0, 0, 0))
            {
                Debug.Log("trying to destroy");
                
                GameObject.Destroy(oldRoot);
            }
        }
    }

    void FixedUpdate()
    {
        if (startedLoading)
        {
            if (GameObject.Find(nameOfNewRoot))
            {
                initialzeValues();
                Debug.Log("Found new root");
                startedLoading = false;
            }
        }
    }

    void initialzeValues()
    {
        newRoot = GameObject.Find(nameOfNewRoot);
        startPositionNewRoot = newRoot.transform.position;

        oldRoot = GameObject.Find(nameOfOldRoot);
        startPositionOldRoot = oldRoot.transform.position;

        Debug.Log("initalizing values");

        switch (swipeDirection)
        {
            case eDirectionOfSwipe.left:
                newRoot.transform.position = new Vector3(newRoot.transform.position.x + 1000, newRoot.transform.position.y);
                break;
            case eDirectionOfSwipe.right:
                newRoot.transform.position = new Vector3(newRoot.transform.position.x - 1000, newRoot.transform.position.y);
                break;
        }

        finishedLoading = true;
    }
}
