using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterNote : MonoBehaviour
{
    public float speed;
    public float range; //How far it should go before self-destruct
    float currentDistance; //How far the cube has moved from it's initial point
    Vector3 initPosition; //Initial position of the note
    Vector3 moveDirection;
    public Vector3 offset;
    public float offsetTimer;
    float offsetSpeed; //Speed modifier so that the total distance travelled is the same as 1 second

    public void initNote(float speed, float range, Vector3 moveDirection, Vector3 offset, float offsetTimer) {
        initPosition = transform.position - offset;
        this.speed = speed;
        this.range = range;
        this.moveDirection = moveDirection;
        this.offset = offset;
        this.offsetTimer = offsetTimer;
        offsetSpeed = offsetTimer != 0 ? 1f / offsetTimer : 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Calculate current travel distance, if it equals range then despawn the note
        currentDistance = (transform.position - initPosition).magnitude;
        if (currentDistance >= range) {
            Destroy(gameObject);
        }
        
        //Move the note
        transform.position += speed * moveDirection * Time.fixedDeltaTime;

        //Adjust the offset of the note
        if (offsetTimer > 0f) {
            transform.position -= offset * offsetSpeed * Time.fixedDeltaTime;
            offsetTimer -= Time.fixedDeltaTime;
        }
    }
}
