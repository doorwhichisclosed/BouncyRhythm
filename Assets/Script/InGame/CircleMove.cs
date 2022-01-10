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
    [SerializeField] private Player playerMove;
    [SerializeField] private ScoreManager scoreManagerPresenter;
    private ScoreManagerStat scoreManager;
    private float runningTime = 0;
    private bool init = false;
    private bool isFirstRotation = true;
    public RectTransform timingTr;
    public Vector2 timingX;
    public Vector2 timingY;

    private void Awake()
    {
        BPM = SoundManager.instance.BPM;
        scoreManager = scoreManagerPresenter.scoreManager;
        timingX = new Vector2(timingRect.localPosition.x - timingRect.rect.width / 2, timingRect.localPosition.x + timingRect.rect.width / 2);
        timingY = new Vector2(timingRect.localPosition.y - timingRect.rect.height / 2, timingRect.localPosition.y + timingRect.rect.height / 2);
        IObservable<long> update = Observable.EveryUpdate();
        var onTriggerEnter = this.OnTriggerEnter2DAsObservable();
        update.Subscribe(_ => Move()).AddTo(gameObject).AddTo(this);
        onTriggerEnter
            .Where(collision => collision.gameObject.CompareTag("Timing"))
            .Subscribe(_ => OnTriggerEnterTimingRect()).AddTo(this);
    }

    private void Move()
    {
        runningTime += (360*(BPM/60)) * Time.deltaTime;
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
        if (isFirstRotation&&playerMove.noteState!=NoteState.isWaitingForNext)
        {
            SoundManager.instance.PlayMusic();
            isFirstRotation = false;
        }

        if (playerMove.noteState == NoteState.isHolding)
            playerMove.playerStat.CalculateCnt(1);
        else if (playerMove.noteState == NoteState.isWaitingForNext)
        {
            Debug.Log("쉬어가기");
            playerMove.noteState = NoteState.isNotHolding;
        }
        else
        {
            Debug.Log("실패");
            scoreManager.CalculateComboAttempt(-1);
            if (scoreManager.ComboAttempt.Value == 0)
            {
                playerMove.playerStat.CalculateLife(-1);
                scoreManager.ResetCombo();
                scoreManager.ResetComboAttempt();
            }
        }
    }
}
