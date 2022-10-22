using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMazeMovement : MonoBehaviour {

    public GameObject player;
    public Vector3 originalPosition; // start position of the gameobject
    public int playerIndex;

    public float timer = 0.1f; // interval of time between each move
    float timeCount = 0;

    public int[] DNA; // "DNA" that determines which direction the player moves to
    public int DNANum = 100; // the total number of DNAs
    public int index = 0; // current index in the DNA array

    public int[] cornerHitTime;

    public float speed = 0.01f;

    public bool isDoneMoving = false;
    public bool beforeEvaluation = true;

    public GameObject dataManager;

    public int segmentNum = 48; // number of segments in DistanceMeasure
    public Transform distanceMeasure; // the transform of the parent gameobject is required to find the child gameobject 
    public GameObject[] segments;

    // Start is called before the first frame update
    void Start() {
        originalPosition = player.transform.position;

        DNA = new int[DNANum];

        for (int i = 0; i < DNANum; i++) {
            DNA[i] = UnityEngine.Random.Range(0, 4);
        }

        segments = new GameObject[segmentNum];

        for (int i = 0; i < 48; i++) {
            segments[i] = distanceMeasure.Find(("DistanceSegment (" + i.ToString() + ")")).gameObject;

            //Debug.Log(segments[i].GetComponent<SegmentNumber>().number);
        }

        cornerHitTime = new int[] { -1, -1, -1, -1, -1 };
    }

    // Update is called once per frame
    void Update() {
        timeCount += Time.deltaTime;

        if (timeCount > timer && index < DNANum) {
            if (index == 0) {
                string DNAstring = "";

                for (int i = 0; i < DNANum; i++) {
                    DNAstring += DNA[i].ToString() + " ";
                }

                //Debug.Log("player " + playerIndex + "- DNA: " + DNAstring);
            }

            if (DNA[index] == 0) // 0 = right
            {
                Vector3 v = new Vector3(-1 * speed, 0, 0);
                player.transform.Translate(v);
            }
            else if (DNA[index] == 1) // 1 = left
            {
                Vector3 v = new Vector3(speed, 0, 0);
                player.transform.Translate(v);
            }
            else if (DNA[index] == 2) // 2 = up
            {
                Vector3 v = new Vector3(0, 0, -1 * speed);
                player.transform.Translate(v);
            }
            else if (DNA[index] == 3) // 3 = down
            {
                Vector3 v = new Vector3(0, 0, speed);
                player.transform.Translate(v);
            }

            int firstCorner = 14;
            int secondCorner = 20;
            int thirdCorner = 32;
            int fourthCorner = 38;
            int fifthCorner = 48;

            int distance = findDistance();

            if (distance > firstCorner && cornerHitTime[0] == -1) {
                cornerHitTime[0] = index;
            }
            else if (distance > secondCorner && cornerHitTime[1] == -1) {
                cornerHitTime[1] = index;
            }
            else if (distance > thirdCorner && cornerHitTime[2] == -1) {
                cornerHitTime[2] = index;
            }
            else if (distance > fourthCorner && cornerHitTime[3] == -1) {
                cornerHitTime[3] = index;
            }
            else if (distance > fifthCorner && cornerHitTime[4] == -1) {
                cornerHitTime[4] = index;
            }

            index++;
            timeCount = 0;
        }
        else if (index >= DNANum) // if indedx >= DNANum, that means that this object is done moving according to the DNA
        {
            isDoneMoving = true;
        }

        if (isDoneMoving && beforeEvaluation) {
            beforeEvaluation = false;
        }
    }

    public void evaluate() {
        // calculate the segment that the player is closest to

        int score = findDistance();

        dataManager.GetComponent<DataManager>().finalDistance.Add(score);

        int[] DNAcopy = new int[DNANum];

        for (int i = 0; i < DNANum; i++) {
            DNAcopy[i] = DNA[i];
        }

        //dataManager.GetComponent<DataManager>().allDNAs.Add(DNAcopy);

        // make the next generation players move again
        //index = 0;
    }

    public int findDistance() {
        float x1 = player.transform.position.x;
        float z1 = player.transform.position.z;

        float x2, z2;
        double distance;

        int shortestIndex = -1;
        double shortestDistance = 100000000000;

        for (int i = 0; i < 48; i++) {
            x2 = segments[i].transform.position.x;
            z2 = segments[i].transform.position.z;

            distance = Math.Sqrt((x2 - x1) * (x2 - x1) + (z2 - z1) * (z2 - z1));

            if (distance < shortestDistance) {
                shortestDistance = distance;
                shortestIndex = i;
            }
        }

        int score = 48 - shortestIndex;

        return score;
    }
}
