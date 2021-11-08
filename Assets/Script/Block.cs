using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int answerNum;
    [SerializeField] private GameObject[] platforms;
    public void MakeRandomPlatforms(int exceptNum)
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].SetActive(false);
        }
        Random.InitState((int)(System.DateTime.Now.Ticks * System.DateTime.Now.Second));
        answerNum = Random.Range(0, 5);
        while(answerNum==exceptNum)
        {
            Random.InitState((int)(System.DateTime.Now.Ticks * System.DateTime.Now.Second));
            answerNum = Random.Range(0, 5);
        }
        platforms[answerNum].SetActive(true);
    }
}
