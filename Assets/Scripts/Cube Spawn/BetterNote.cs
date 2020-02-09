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
    Rigidbody rb;
    StatTracker stat;

    public ScoreCollider score;
    [HideInInspector]
    public bool hitFlag;

    private void Start()
    {
        if (TryGetComponent<Rigidbody>(out rb))
            rb.isKinematic = true; //Disable at start so it doesn't fly away
        hitFlag = false;
    }

    public void initNote(float speed, float range, Vector3 moveDirection, Vector3 offset, float offsetTimer, StatTracker statTrker) {
        initPosition = transform.position - offset;
        this.speed = speed;
        this.range = range;
        this.moveDirection = moveDirection;
        this.offset = offset;
        this.offsetTimer = offsetTimer;
        offsetSpeed = offsetTimer != 0 ? 1f / offsetTimer : 0f;

        this.stat = statTrker;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Calculate current travel distance, if it equals range then despawn the note
        currentDistance = (transform.position - initPosition).magnitude;
        if (rb != null && currentDistance >= range / 10f)
        {
            rb.isKinematic = false;
        }
        if (currentDistance >= range) {
            if (tag != "Bomb")
                stat.addScore(-1); //If the player miss the note, cut the streak. Except for bombs
            Destroy(gameObject);
        }
        
        //Move the note until its been hit
        if (!hitFlag)
            transform.position += speed * moveDirection * Time.fixedDeltaTime;

        //Adjust the offset of the note
        if (offsetTimer > 0f) {
            transform.position -= offset * offsetSpeed * Time.fixedDeltaTime;
            offsetTimer -= Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitFlag = true;
        if (score != null && score.Hit)
            stat.addScore(100);
        else
            stat.addScore(0);
        
        if (TryGetComponent<Collider>(out Collider c))
        {
            c.enabled = false;
        }
        if (rb != null)
        {
            ContactPoint contact = collision.GetContact(0);
            rb.AddForceAtPosition(contact.normal * 200f, contact.point);
        }
        Destroy(gameObject, 0.5f);
    }
}
