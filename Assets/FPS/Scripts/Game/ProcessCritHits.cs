using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessCritHits : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("crit point hit");
    }

}
