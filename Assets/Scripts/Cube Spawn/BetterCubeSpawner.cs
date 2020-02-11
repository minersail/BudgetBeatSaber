using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCubeSpawner : MonoBehaviour
{
    [Header("General Settings")]
    public float gridSize = 5f;
    public float speed = 15f;
    public float range = 90f;
    [Range(0.1f, 2f)]
    public float offsetTimer = 1f;
    public float minNoteInterval = 0.1f; //Minimum interval between 2 notes
    public float maxNoteInterval = 1f;    //Maximum interval between 2 notes
    public float minOffset = 5f;
    public float maxOffset = 15f;
    [Header("Note Prefabs")]
    public GameObject RightNoteOmniPrefab;
    public GameObject RightNoteDirectionalPrefab;
    public GameObject LeftNoteOmniPrefab;
    public GameObject LeftNoteDirectionalPrefab;
    public GameObject BombPrefab;
    [Header("Other Stuff")]
    public StatTracker stat;
    Transform endAnchor;    //The place where all notes will end up at

    void Start() {
        endAnchor = GameObject.FindGameObjectWithTag("EndAnchor").transform;
    }

    void SpawnCube() {
        //Generate spawn position
        float halfGridSize = gridSize / 2f;
        float x = Random.Range(-halfGridSize, halfGridSize) + transform.position.x;
        float y = Random.Range(-halfGridSize, halfGridSize) + transform.position.y;
        Vector3 spawnPosition = new Vector3(x, y, transform.position.z);

        //Add offset for fun and effects
        Vector3 offset = (spawnPosition - transform.position).normalized;
        offset *= Random.Range(minOffset, maxOffset);
        spawnPosition += offset;

        /*Spawn the note
            0.00 - 0.20 = Right Note Omni-directional
            0.20 - 0.40 = Left Note Omni-drectional
            0.40 - 0.65 = Right Note Directional
            0.65 - 0.90 = Left Not Directional
            0.10 - 1.00 = Bomb      */
        float noteType = Random.Range(0f, 1f);
        int rotate = Random.Range(0, 7);
        GameObject note;
        if (noteType < 0.2f) {
            note = Instantiate(RightNoteOmniPrefab, spawnPosition, new Quaternion());
        } else if (noteType < 0.4f) {
            note = Instantiate(LeftNoteOmniPrefab, spawnPosition, new Quaternion());
        } else if (noteType < 0.65f) {
            note = Instantiate(RightNoteDirectionalPrefab, spawnPosition, new Quaternion());
        } else if (noteType < 0.9f) {
            note = Instantiate(LeftNoteDirectionalPrefab, spawnPosition, new Quaternion());
        } else {
            note = Instantiate(BombPrefab, spawnPosition, new Quaternion());
        }

        //Initiate the notes/bombs
        note.GetComponent<BetterNote>().initNote(speed, range, transform.forward, offset, offsetTimer, stat);

        //Adjust rotation of note
        Vector3 lookAtPosition = new Vector3(endAnchor.position.x, note.transform.position.y, endAnchor.position.z);
        note.transform.LookAt(lookAtPosition, Vector3.up);
        if (noteType >= 0.4f && noteType < 0.9f) {
            note.transform.Rotate(0f, 0f, (float)rotate / 8f * 360f);
        }
    }

    float timer = 0f;
    [Header("Spawn Point Control")]
    public float rotateSpeed = 10f;

    void FixedUpdate()
    {
        //TEST CODE. This is not using Oculus input system
        transform.position -= transform.right * Input.GetAxis("Horizontal") * rotateSpeed * Time.fixedDeltaTime;
        
        //Force the spawner to look at the endAnchor, but lock the rotation on the y axis
        Vector3 lookAtPosition = new Vector3(endAnchor.position.x, transform.position.y, endAnchor.position.z);
        transform.LookAt(lookAtPosition, Vector3.up);
        
        //Spawn note randomly
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f) {
            timer = Random.Range(minNoteInterval, maxNoteInterval);
            SpawnCube();
        }

        //Adjust spawner's position
        transform.position = endAnchor.transform.position - transform.forward * range;
    }

    void OnValidate() {
        if (gridSize < 0f) gridSize = 0f;
        if (speed < 0f) speed = 0f;
        if (range < 0f) range = 0f;
        if (minNoteInterval < 0f) minNoteInterval = 0.01f;
        if (maxNoteInterval < minNoteInterval) maxNoteInterval = minNoteInterval;
        if (minOffset < 0f) minOffset = 0f;
        if (maxOffset < minOffset) maxOffset = minOffset; 
    }
}