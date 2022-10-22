using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject teleportEntrance;
    public GameObject teleportExit;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collision c)
    { 
        Vector3 pos = c.contacts[0].point;
        Debug.Log(pos);
        if (c.gameObject.CompareTag("Player"))
        {
            player.transform.position = teleportExit.transform.position;
        }
    }

}
