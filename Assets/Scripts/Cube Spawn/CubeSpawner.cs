using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public float gridSize = 5f;
    public float speed;
    public float range;
    public float minNoteInterval = 0.02f; //Minimum interval between 2 notes
    public float maxNoteInterval = 1f;    //Maximum interval between 2 notes
    public Material matRightHand, matLeftHand;

    void SpawnRandomCube() {
        //Generate spawn position
        float halfGridSize = gridSize / 2f;
        float x = Random.Range(-halfGridSize, halfGridSize) + transform.position.x;
        float y = Random.Range(-halfGridSize, halfGridSize) + transform.position.y;
        float z = transform.position.z;
        
        //Adjusting position
        GameObject note = GameObject.CreatePrimitive(PrimitiveType.Cube);
        note.transform.position = new Vector3(x, y, z);
        
        //Changing tag and material
        bool hand = Random.Range(0f, 1f) > 0.5f ? true : false; //True = right hand. False = left hand
        if (hand) {
            note.tag = "RightHand";
            note.GetComponent<Renderer>().material = matRightHand;
        } else {
            note.tag = "LeftHand";
            note.GetComponent<Renderer>().material = matLeftHand;
        }
        
        note.AddComponent<Note>().initNote(speed, range, transform.forward);
    }

    float timer = 0f;

    void FixedUpdate()
    {
        //Spawn note randomly
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f) {
            timer = Random.Range(minNoteInterval, maxNoteInterval);
            SpawnRandomCube();
        }
    }
}
