using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed;
    public float range; //How far it should go before self-destruct
    float currentDistance; //How far the cube has moved from it's initial point
    Vector3 initPosition; //Initial position of the note
    Vector3 moveDirection;

    public void initNote(float speed, float range, Vector3 moveDirection) {
        initPosition = transform.position;
        this.speed = speed;
        this.range = range;
        this.moveDirection = moveDirection;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentDistance = (transform.position - initPosition).magnitude;
        if (currentDistance >= range) {
            Destroy(gameObject);
        }
        transform.position += speed * moveDirection * Time.fixedDeltaTime;
    }
}
