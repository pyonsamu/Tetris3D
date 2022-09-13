using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBlock : MonoBehaviour
{
    public bool overlap;

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
        Debug.Log("Enter");
        overlap = true;
    }

    // 重なり中の判定
    void OnTriggerStay(Collider other)
    {
    }

    // 重なり離脱の判定
    public void OnTriggerExit(Collider other)
    {
        overlap = false;
        Debug.Log("Exit");
    }
}
