using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public StatTracker stat;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " +stat.Score + "\nStreak: " + stat.CurrentStreak + "\nMax Streak: " + stat.MaxStreak + "\nBad Hit: " + stat.BadHit +"\nMisses: " + stat.Miss;
    }
}
