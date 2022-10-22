using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    public List<int> finalDistance = new List<int>();
    //public List<int[]> allDNAs = new List<int[]>();

    bool didFinishGen = false;

    public int DNANum = 100;


    public int[] parent1DNA;
    public int[] parent2DNA;

    int generation = 0;
    public int maxGeneration = 10;

    public GameObject[] players;

    public bool doNextGen = false;

    public int fastestToReachLength48;

    void Start() {
        //Debug.Log(decimalToBinary(0));
            
        parent1DNA = new int[DNANum];
        parent2DNA = new int[DNANum];

        players = new GameObject[16];

        for (int i = 0; i < 16; i++) {
            players[i] = GameObject.Find(("Player (" + i + ")")).gameObject;
        }

        fastestToReachLength48 = DNANum + 1;
    }

    void Update() {
        /*if (doNextGen && Input.GetKey(KeyCode.Space)) // delete this later
        {
            transitionGen();
            Debug.Log("New Generation - Gen: " + generation);
            doNextGen = false;
        } */
        bool isAllDoneMoving = true;

        for (int i = 0; i < 16; i++) {
            if (!players[i].GetComponent<PlayerMazeMovement>().isDoneMoving) {
                isAllDoneMoving = false;
            }
        }

        if (isAllDoneMoving && !didFinishGen /*&& !doNextGen*/) {
            for (int i = 0; i < 16; i++) {
                players[i].GetComponent<PlayerMazeMovement>().evaluate();
                if (players[i].GetComponent<PlayerMazeMovement>().cornerHitTime[4] != -1 &&
                    players[i].GetComponent<PlayerMazeMovement>().cornerHitTime[4] < fastestToReachLength48) {
                    fastestToReachLength48 = players[i].GetComponent<PlayerMazeMovement>().cornerHitTime[4];
                }
            }

            int longest1 = -1;
            int longestIndex1 = -1;
            int longest2 = -1;
            int longestIndex2 = -1;

            for (int i = 0; i < 16; i++) {
                if (finalDistance[i] >= longest1) {
                    longestIndex1 = i;
                    longest1 = finalDistance[i];
                }
            }

            for (int i = 0; i < 16; i++) {
                if (finalDistance[i] >= longest2 && i != longestIndex1) {
                    longestIndex2 = i;
                    longest2 = finalDistance[i];
                }
            }

            // store the best DNA as parentDNAs
            for (int i = 0; i < DNANum; i++) {
                //parent1DNA[i] = allDNAs[longestIndex1][i];
                //parent2DNA[i] = allDNAs[longestIndex2][i];

                parent1DNA[i] = players[longestIndex1].GetComponent<PlayerMazeMovement>().DNA[i];
                parent2DNA[i] = players[longestIndex2].GetComponent<PlayerMazeMovement>().DNA[i];
            }

            string s1 = "";
            string s2 = "";

            for (int i = 0; i < DNANum; i++) {
                s1 += parent1DNA[i].ToString() + " ";
                s2 += parent2DNA[i].ToString() + " ";
            }

            string str;

            for (int i = 0; i < 16; i++) {
                str = "";
                for (int j = 0; j < DNANum; j++) {
                    str += players[i].GetComponent<PlayerMazeMovement>().DNA[j] + " ";
                }
                // Debug.Log("Player " + i + ":    " + str + "---- length: " + finalDistance[i]);
            }


            // Debug.Log("length: " + longest2/* + " gene: " + s2*/);

            //allDNAs.Clear();

            if (generation < maxGeneration - 1) {
                doNextGen = true;
                transitionGen();
                finalDistance.Clear();
                Debug.Log("length: " + longest1 + " gene: " + s1);
                Debug.Log("New Generation - Gen: " + generation);
            }
            else {
                didFinishGen = true;
                Debug.Log("Done with " + maxGeneration + " generations");
                Debug.Log("Fastest time to reach the end: " + fastestToReachLength48 + " indices");

                // switch scene and test the best gene
            }
        }
    }
    void transitionGen() {
        string DNACrossOverInfo;

        string printstr;

        bool isParentCopy;

        for (int i = 0; i < 16; i++) {
            isParentCopy = false;
            if (i == 0) {
                isParentCopy = true;
                for (int j = 0; j < DNANum; j++) {
                    players[i].GetComponent<PlayerMazeMovement>().DNA[j] = parent1DNA[j];
                }
            }
            else if (i == 1) {
                isParentCopy = true;
                for (int j = 0; j < DNANum; j++) {
                    players[i].GetComponent<PlayerMazeMovement>().DNA[j] = parent2DNA[j];
                }
            }
            else {
                DNACrossOverInfo = decimalToBinary(i - 1);

                for (int j = 0; j < 4; j++) {
                    for (int k = 0; k < DNANum / 4; k++) // give the players a new gene based on the parents
                    {
                        if (DNACrossOverInfo[j] == '0') {
                            players[i].GetComponent<PlayerMazeMovement>().DNA[(j * DNANum / 4) + k] = parent1DNA[(j * DNANum / 4) + k];
                        }
                        else {
                            players[i].GetComponent<PlayerMazeMovement>().DNA[(j * DNANum / 4) + k] = parent2DNA[(j * DNANum / 4) + k];
                        }
                    }
                }
            }

            printstr = "";

            for (int j = 0; j < DNANum; j++) {
                printstr += players[i].GetComponent<PlayerMazeMovement>().DNA[j] + " ";
            }

            //Debug.Log("Before: " + printstr);

            if (!isParentCopy) {
                GetComponent<MutationGenerator>().mutate(players[i].GetComponent<PlayerMazeMovement>().DNA, finalDistance[i],
                                                           players[i].GetComponent<PlayerMazeMovement>().cornerHitTime);
            }

            printstr = "";

            for (int j = 0; j < DNANum; j++) {
                printstr += players[i].GetComponent<PlayerMazeMovement>().DNA[j] + " ";
            }

            //Debug.Log("After: " + printstr);

            // reset the variables for the gameobject

            players[i].transform.position = new Vector3(players[i].GetComponent<PlayerMazeMovement>().originalPosition.x,
                                                         players[i].GetComponent<PlayerMazeMovement>().originalPosition.y,
                                                         players[i].GetComponent<PlayerMazeMovement>().originalPosition.z);
            players[i].GetComponent<PlayerMazeMovement>().index = 0; // start moving the gameobject again
            players[i].GetComponent<PlayerMazeMovement>().beforeEvaluation = true;
            players[i].GetComponent<PlayerMazeMovement>().isDoneMoving = false;

            players[i].GetComponent<PlayerMazeMovement>().cornerHitTime[0] = -1;
            players[i].GetComponent<PlayerMazeMovement>().cornerHitTime[1] = -1;
            players[i].GetComponent<PlayerMazeMovement>().cornerHitTime[2] = -1;
            players[i].GetComponent<PlayerMazeMovement>().cornerHitTime[3] = -1;
        }

        generation++;
    }

    string decimalToBinary(int num) {
        string binary = "";

        while (num > 0) {
            binary = (num % 2).ToString() + binary;
            num /= 2;
        }

        while (binary.Length < 4) {
            binary = "0" + binary;
        }

        return binary;
    }

}
