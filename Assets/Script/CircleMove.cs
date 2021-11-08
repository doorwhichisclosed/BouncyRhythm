using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CircleMove : MonoBehaviour
{
    public float BPM;
    public float circleRad;
    [SerializeField] private RectTransform timingRect;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private ScoreManager scoreManager;
    public float runningTime = 0;
    private bool init = false;
    public RectTransform timingTr;
    public Vector2 timingX;
    public Vector2 timingY;

    private void Awake()
    {
        timingX = new Vector2(timingRect.localPosition.x - timingRect.rect.width / 2, timingRect.localPosition.x + timingRect.rect.width / 2);
        timingY = new Vector2(timingRect.localPosition.y - timingRect.rect.height / 2, timingRect.localPosition.y + timingRect.rect.height / 2);
        IObservable<long> update = Observable.EveryUpdate();
        var onTriggerEnter = this.OnTriggerEnter2DAsObservable();
        update.Subscribe(_ => Move()).AddTo(gameObject);
        onTriggerEnter
            .Where(collision => collision.gameObject.CompareTag("Timing"))
            .Subscribe(_ => OnTriggerEnterTimingRect());
    }

    private void Move()
    {
        runningTime += 3*BPM * Time.deltaTime;
        if(runningTime>3600)
        {
            runningTime = 0;
        }
        float x = Mathf.Cos(Mathf.Deg2Rad * runningTime) * (circleRad);
        float y = Mathf.Sin(Mathf.Deg2Rad * runningTime) * (circleRad);
        if (Mathf.Abs(y/circleRad) <= 0.1f && !init)
            init = true;
        else if(init)
            gameObject.transform.localPosition = new Vector2(y, x-700);
    }

    private void OnTriggerEnterTimingRect()
    {
        if (playerMove.noteState == NoteState.isHolding)
            playerMove.cnt++;
        else if (playerMove.noteState == NoteState.isWaitingForNext)
        {
            Debug.Log("쉬어가기");
            playerMove.noteState = NoteState.isNotHolding;
        }
        else
        {
            Debug.Log("실패");
            scoreManager.comboAttempt--;
            if (scoreManager.comboAttempt == 0)
            {
                scoreManager.combo = 0;
                scoreManager.comboAttempt = 5;
            }
        }
    }
}
