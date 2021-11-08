using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
public enum NoteState
{
    isHolding,
    isNotHolding,
    isWaitingForNext
};
public enum Direction
{
    left,
    right
};
public class PlayerMove : MonoBehaviour
{
    public NoteState noteState = NoteState.isWaitingForNext;
    public int curPos;
    [SerializeField] private UIButton leftButton;
    [SerializeField] private UIButton rightButton;
    [SerializeField] private CircleMove circleMove;
    [SerializeField] private ScoreManager scoreManager;

    public int cnt = 1;
    private Vector2 timingX;
    private Vector2 timingY;
    private Transform circleMoveTr;

    private bool canMove = true;
    private void Start()
    {
        curPos = 2;
        circleMoveTr = circleMove.transform;
        timingX = circleMove.timingX;
        timingY = circleMove.timingY;
        leftButton.OnPointerDown.Subscribe(_ => CheckTiming(Direction.left));
        rightButton.OnPointerDown.Subscribe(_ => CheckTiming(Direction.right));
        leftButton.OnPointerUp.Subscribe(_ => StopHolding(Direction.left));
        rightButton.OnPointerUp.Subscribe(_ => StopHolding(Direction.right));
        var rightArrow = Observable.EveryUpdate().Where(_=>Input.GetKeyDown(KeyCode.RightArrow)&&canMove);
        var leftArrow = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.LeftArrow)&&canMove);
    }

    private void CheckTiming(Direction direction)
    {
        if (circleMove.timingTr.localPosition.x >= timingX.x && circleMoveTr.localPosition.x <= timingX.y && circleMoveTr.localPosition.y >= timingY.x && circleMoveTr.localPosition.y <= timingY.y)
        {
            Debug.Log(direction + " " + "눌림");
            noteState = NoteState.isHolding;
        }
    }

    private void StopHolding(Direction direction)
    {
        if(cnt>1)
        {
            noteState = NoteState.isNotHolding;
        }
        if (circleMove.timingTr.localPosition.x >= timingX.x && circleMoveTr.localPosition.x <= timingX.y+75 && circleMoveTr.localPosition.y >= timingY.x-75 && circleMoveTr.localPosition.y <= timingY.y)
        {
            scoreManager.combo++;
            Debug.Log(direction + ": " + cnt + " 콤보: " + scoreManager.combo);
            noteState = NoteState.isWaitingForNext;
            if(direction==Direction.left)
            {
                curPos -= cnt;
                transform.DOJump(new Vector2(transform.position.x - cnt, transform.position.y + 1), 0.5f, 1, 0.3f)
                .OnStart(() => canMove = false)
                .OnComplete(() => canMove = true);
            }
            else if(direction==Direction.right)
            {
                curPos += cnt;
                transform.DOJump(new Vector2(transform.position.x + cnt, transform.position.y + 1), 0.5f, 1, 0.3f)
                .OnStart(() => canMove = false)
                .OnComplete(() => canMove = true);
            }
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
        cnt = 1;
    }
}
