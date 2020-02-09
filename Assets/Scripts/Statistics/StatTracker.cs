using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    private int score;
    public int Score
    {
        get { return score; }
    }
    private int currentStreak;
    public int CurrentStreak
    {
        get { return currentStreak; }
    }
    private int maxStreak;
    public int MaxStreak
    {
        get { return maxStreak; }
    }
    private int miss;
    public int Miss { get { return miss; } }
    private int badHit;
    public int BadHit { get { return badHit; } }
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        currentStreak = 0;
        maxStreak = 0;
    }

    //Positive for correct hit, negative for incorrect hit, zero for bad hit
    public void addScore(int add)
    {
        if (add > 0)
        {
            score += add;
            currentStreak++;
            if (currentStreak > maxStreak)
                maxStreak = currentStreak;
        }
        else if (add == 0)
        {
            currentStreak = 0;
            badHit++;
        }
        else
        {
            currentStreak = 0;
            miss++;
        }
    }
}
