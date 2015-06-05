using UnityEngine;
using System.Collections;

public class Swipper : MonoBehaviour {

    public static Swipper swipper;

    private Vector3 startPosition;

    public float scrollSpeed;
    public float tileSizeX;

    void Awake()
    {
        swipper = this;
    }

    public void loadScene(string oldSceneName, string newSceneName, eDirectionOfSwipe directionToSwipe)
    {
        Application.LoadLevelAdditive(newSceneName);


        while (Application.isLoadingLevel)
        {
            Debug.Log("loading");
        } 

        string nameOfNewRoot = "root" + newSceneName;
        string nameOfOldRoot = "root" + oldSceneName;

        GameObject newRoot = GameObject.Find(nameOfNewRoot);
        Vector3 startPositionNewRoot = newRoot.transform.position;

        GameObject oldRoot = GameObject.Find(nameOfOldRoot);
        Vector3 startPositionOldRoot = oldRoot.transform.position;

        if (directionToSwipe == eDirectionOfSwipe.right)
        {
            newRoot.transform.position = new Vector3(newRoot.transform.position.x - 20, newRoot.transform.position.y);

            moveScenesRight(startPositionOldRoot, startPositionNewRoot, oldRoot, newRoot);
        }
        else
        {
            newRoot.transform.position = new Vector3(newRoot.transform.position.x + 20, newRoot.transform.position.y);

            moveScenesLeft(startPositionOldRoot, startPositionNewRoot, oldRoot, newRoot);
        }

        GameObject.Destroy(oldRoot);
    }

    private void moveScenesLeft(Vector3 firstRootPosition, Vector3 secondRootPosition, GameObject firstRoot, GameObject secondRoot)
    {
        //while(Time.timeSinceLevelLoad < 60) {
        //    if (secondRoot.transform.position.x != 0)
        //    {
        //        firstRoot.transform.position = new Vector3(firstRoot.transform.position.x - 1 / 3, firstRoot.transform.position.y - 1 / 3);
        //        secondRoot.transform.position = new Vector3(secondRoot.transform.position.x - 1 / 3, secondRoot.transform.position.y - 1 / 3);
        //    }
        //}

        while (true)
        {
            if (secondRoot.transform.position == new Vector3(0, 0, 0))
            {
                break;
            }

            float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeX);
            firstRoot.transform.position = firstRootPosition + Vector3.left * newPosition;

            secondRoot.transform.position = secondRootPosition + Vector3.left * newPosition;
        }

    }

    private void moveScenesRight(Vector3 firstRootPosition, Vector3 secondRootPosition, GameObject firstRoot, GameObject secondRoot)
    {
        //while (Time.timeSinceLevelLoad < 60)
        //{
        //    if (secondRoot.transform.position.x != 0)
        //    {
        //        firstRoot.transform.position = new Vector3(firstRoot.transform.position.x + 1 / 3, firstRoot.transform.position.y + 1 / 3);
        //        secondRoot.transform.position = new Vector3(secondRoot.transform.position.x + 1 / 3, secondRoot.transform.position.y + 1 / 3);
        //    }
        //}
        while (true)
        {
            if (secondRoot.transform.position == new Vector3(0, 0, 0))
            {
                break;
            }
            
            float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeX);
            firstRoot.transform.position = firstRootPosition + Vector3.right * newPosition;

            secondRoot.transform.position = secondRootPosition + Vector3.right * newPosition;
        }
    }
}
