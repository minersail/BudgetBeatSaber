using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollider : MonoBehaviour
{
    private bool hit;
    public bool Hit { get { return hit; } }
    // Start is called before the first frame update
    void Start()
    {
        hit = false;
    }

    //If any saber hits this collider, check if its the right hand
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tag)
            hit = true;
    }
}
