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
public class PlayerStat
{
    public ReactiveProperty<int> Life { get; private set; }
    public ReactiveProperty<int> CurPos { get; private set; }
    public ReactiveProperty<int> CurHeight { get; private set; }
    public ReactiveProperty<int> Cnt { get; private set; }

    public PlayerStat()
    {
        Init();
    }

    public void Init()
    {
        Life = new ReactiveProperty<int>(5);
        CurPos = new ReactiveProperty<int>(2);
        CurHeight = new ReactiveProperty<int>(0);
        Cnt = new ReactiveProperty<int>(1);
    }

    public void CalculateLife(int i)
    {
        Life.Value += i;
    }

    public void ResetLife()
    {
        Life.Value = 0;
    }

    public void CalculateCurPos(int i)
    {
        CurPos.Value += i;
    }

    public void ResetCurPos()
    {
        CurPos.Value = 0;
    }

    public void CalulateCurHeight(int i)
    {
        CurHeight.Value += i;
    }

    public void CalculateCnt(int i)
    {
        Cnt.Value += i;
    }

    public void ResetCnt()
    {
        Cnt.Value = 1;
    }


}
public class Player : MonoBehaviour
{
    [HideInInspector] public NoteState noteState = NoteState.isWaitingForNext;
    [SerializeField] private UIButton leftButton;
    [SerializeField] private UIButton rightButton;
    [SerializeField] private CircleMove circleMove;
    [SerializeField] private ScoreManager scoreManagerPresenter;
    [SerializeField] private BlockPool blockPool;
    public PlayerStat playerStat = new PlayerStat();
    private ScoreManagerStat scoreManager;
    /// <summary>
    /// 변하지 않는 rect값 경우들에는 일부러 reactiveproperty 선언을 안해줌
    /// </summary>
    private Vector2 timingX;
    private Vector2 timingY;
    private Transform circleMoveTr;

    private bool canMove = true;
    private void Start()
    {
        circleMoveTr = circleMove.transform;
        scoreManager = scoreManagerPresenter.scoreManager;
        timingX = circleMove.timingX;
        timingY = circleMove.timingY;
        leftButton.OnPointerDown.Subscribe(_ => CheckTiming(Direction.left)).AddTo(this);
        rightButton.OnPointerDown.Subscribe(_ => CheckTiming(Direction.right)).AddTo(this);
        leftButton.OnPointerUp.Subscribe(_ => StopHolding(Direction.left)).AddTo(this);
        rightButton.OnPointerUp.Subscribe(_ => StopHolding(Direction.right)).AddTo(this);
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
        if (circleMove.timingTr.localPosition.x >= timingX.x
            && circleMoveTr.localPosition.x <= timingX.y + 75
            && circleMoveTr.localPosition.y >= timingY.x - 75
            && circleMoveTr.localPosition.y <= timingY.y
            && canMove
            && noteState == NoteState.isHolding)
        {
            noteState = NoteState.isWaitingForNext;
            if(direction==Direction.left)
            {
                if (blockPool.CheckAnswer(playerStat.CurPos.Value - playerStat.Cnt.Value))
                {
                    playerStat.CalculateCurPos(-playerStat.Cnt.Value);
                    transform.DOJump(new Vector2(transform.position.x - playerStat.Cnt.Value * 1.3f, transform.position.y + 2), 0.5f, 1, 0.3f)
                    .OnStart(() => canMove = false)
                    .OnComplete(() => canMove = true);
                    playerStat.CalulateCurHeight(1);
                    blockPool.GoUp(1);
                    scoreManager.CalculateCombo(1);
                }
                else
                {
                    Debug.Log("틀림");
                    Debug.Log(playerStat.CurPos.Value + "cnt" + playerStat.Cnt.Value);
                    scoreManager.CalculateComboAttempt(-1);
                    if (scoreManager.ComboAttempt.Value == 0)
                    {
                        scoreManager.ResetCombo();
                        scoreManager.ResetComboAttempt();
                    }
                }

            }
            else if(direction==Direction.right)
            {
                if (blockPool.CheckAnswer(playerStat.CurPos.Value + playerStat.Cnt.Value))
                {
                    playerStat.CalculateCurPos(playerStat.Cnt.Value);
                    transform.DOJump(new Vector2(transform.position.x + playerStat.Cnt.Value * 1.3f, transform.position.y + 2), 0.5f, 1, 0.3f)
                    .OnStart(() => canMove = false)
                    .OnComplete(() => canMove = true);
                    playerStat.CalulateCurHeight(1);
                    blockPool.GoUp(1);
                    scoreManager.CalculateCombo(1);
                }
                else
                {
                    Debug.Log("틀림");
                    Debug.Log(playerStat.CurPos.Value+"cnt"+ playerStat.Cnt.Value);
                    scoreManager.CalculateComboAttempt(-1);
                    if (scoreManager.ComboAttempt.Value == 0)
                    {
                        scoreManager.ResetCombo();
                        scoreManager.ResetComboAttempt();
                    }
                }
            }
        }
        else
        {
            Debug.Log("실패");
            scoreManager.CalculateComboAttempt(-1);
            if (scoreManager.ComboAttempt.Value == 0)
            {
                scoreManager.ResetCombo();
                scoreManager.ResetComboAttempt();
            }
        }
        playerStat.ResetCnt();
    }
}
