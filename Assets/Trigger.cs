using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool overlap = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        overlap = true;
        Debug.Log(other.name + "Enter");
    }

    // 重なり中の判定
    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name + "Stay");
    }

    // 重なり離脱の判定
    void OnTriggerExit(Collider other)
    {
        overlap = false;
        Debug.Log(other.name + "Exit");
    }
}
