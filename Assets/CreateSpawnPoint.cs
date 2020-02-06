using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpawnPoint : MonoBehaviour
{
    GameObject[] spawn;
    public int gridSize = 5; //Should be odd for better centering
    public float speed;
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3((float)gridSize / 2, (float)gridSize / 2, 0f); //Off center the spawner by a certain amount
        initSpawn();
    }

    void initSpawn() {
        spawn = new GameObject[gridSize * gridSize];
        
        //Grid on the xy plane
        int index = 0;
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                spawn[index] = new GameObject();
                spawn[index].transform.parent = transform;
                spawn[index].transform.localPosition = new Vector3(x, y, 0f);
                index++;
            }
        }
    }

    void SpawnRandomCube() {
        int x = Random.Range(0, gridSize - 1);
        int y = Random.Range(0, gridSize - 1);
        GameObject note = GameObject.CreatePrimitive(PrimitiveType.Cube);
        note.transform.position = spawn[x * gridSize + y].transform.position;
        note.AddComponent<Note>().initNote(speed, range, transform.forward);
    }

    const float minNoteInterval = 0.1f; //Minimum interval between 2 notes
    const float maxNoteInterval = 1f;
    float timer = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f) {
            timer = Random.Range(minNoteInterval, maxNoteInterval);
            SpawnRandomCube();
        }
    }
}
