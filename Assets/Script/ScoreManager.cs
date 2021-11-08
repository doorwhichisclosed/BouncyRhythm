using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int combo;
    public int comboAttempt;
    public int life;
    public float score;

    public void CalculateScore()
    {
        if (combo < 10)
            score += 1;
        else
            score += (1 + 0.2f * (int)(combo / 10));
    }
}
