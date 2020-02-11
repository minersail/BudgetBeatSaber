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
    public GameObject cuttedPrefab;
    float offsetSpeed; //Speed modifier so that the total distance travelled is the same as 1 second
    StatTracker stat;

    public ScoreCollider score;
    [HideInInspector]
    public bool hitFlag;

    private void Start()
    {
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
        
        ContactPoint contact = collision.GetContact(0);
        GameObject left, right;
        float forceLeft = Random.Range(400f, 1200f);
        float forceRight = Random.Range(400f, 1200f);
        if (tag == "Bomb")
        {
            left = Instantiate(cuttedPrefab, transform.position, transform.rotation);
            left.GetComponent<Rigidbody>().AddExplosionForce(forceLeft, contact.point, 3f);
            
            right = Instantiate(cuttedPrefab, transform.position, transform.rotation);
            right.transform.localScale = new Vector3(0.4f, -0.4f, 0.4f);
        }
        else
        {
            left = Instantiate(cuttedPrefab, transform.position + transform.right * -0.25f, transform.rotation);
            left.GetComponent<Rigidbody>().AddExplosionForce(forceLeft, contact.point, 3f);
            
            right = Instantiate(cuttedPrefab, transform.position + transform.right * 0.25f, transform.rotation);
            right.transform.localScale = new Vector3(-0.5f, 1f, 1f);
        }
        right.GetComponent<Rigidbody>().AddExplosionForce(forceRight, contact.point, 3f);
        Destroy(left, 0.5f);
        Destroy(right, 0.5f);
        
        Destroy(gameObject);
    }
}
