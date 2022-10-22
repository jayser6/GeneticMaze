using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationGenerator : MonoBehaviour
{

    public int mutationPercentage = 7;
    public int DNANum = 100;

    public void mutate (int [] DNA, int distance, int [] cornerHitTime)
    {
        //Debug.Log(distance);

        int RNG;
        int RNGresult;
        int pastDNA;
        int cornerNum;

        for (int i = 0;i<DNANum;i++)
        {
            pastDNA = DNA[i];

            RNG = Random.Range(1, 101);            
            if (RNG <= mutationPercentage)
            {
                //DNA[i] = Random.Range(0, 4); // original mutation algorithm

                if (i < cornerHitTime[0] || cornerHitTime[0] == -1)
                {
                    cornerNum = 0;
                    RNGresult = Random.Range(0, 2);
                    if (RNGresult == 0 || DNA[i] == 3)
                    {
                        DNA[i] = 2;
                    }
                    else
                    {
                        DNA[i] = Random.Range(0, 4);
                    }
                }
                else if (i < cornerHitTime[1] || cornerHitTime[1] == -1)
                {
                    cornerNum = 1;
                    RNGresult = Random.Range(0, 2);
                    if (RNGresult == 0 || DNA[i] == 0)
                    {
                        DNA[i] = 1;
                    }
                    else
                    {
                        DNA[i] = Random.Range(0, 4);
                    }
                }
                else if (i < cornerHitTime[2] || cornerHitTime[2] == -1)
                {
                    cornerNum = 2;
                    RNGresult = Random.Range(0, 2);
                    if (RNGresult == 0 || DNA[i] == 2)
                    {
                        DNA[i] = 3;
                    }
                    else
                    {
                        DNA[i] = Random.Range(0, 4);
                    }
                }
                else if (i < cornerHitTime[3] || cornerHitTime[3] == -1)
                {
                    cornerNum = 3;
                    RNGresult = Random.Range(0, 2);
                    if (RNGresult == 0 || DNA[i] == 0)
                    {
                        DNA[i] = 1;
                    }
                    else
                    {
                        DNA[i] = Random.Range(0, 4);
                    }
                }
                else
                {
                    cornerNum = 4;
                    RNGresult = Random.Range(0, 2);
                    if (RNGresult == 0 || DNA[i] == 3)
                    {
                        DNA[i] = 2;
                    }
                    else
                    {
                        DNA[i] = Random.Range(0, 4);
                    }
                }

                //Debug.Log("changed DNA from " + pastDNA + " to " + DNA[i] + " before corner " + cornerNum);
            }
        }
    }
}
