using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class ScoreManagerStat
{
    public ReactiveProperty<int> Combo { get; private set; }
    public ReactiveProperty<int> ComboAttempt { get; private set; }
    public ReactiveProperty<int> Score { get; private set; }
    public int maxCombo = 0;

    public ScoreManagerStat()
    {
        Init();
    }

    public void Init()
    {
        Combo = new ReactiveProperty<int>(0);
        ComboAttempt = new ReactiveProperty<int>(5);
        Score = new ReactiveProperty<int>(0);
    }

    public void ResetCombo()
    {
        Combo.Value = 0;
    }

    public void CalculateCombo(int i)
    {
        Combo.Value += i;
        if (Combo.Value > maxCombo)
            maxCombo = Combo.Value;
    }

    public void ResetComboAttempt()
    {
        ComboAttempt.Value = 5;
    }

    public void CalculateComboAttempt(int i)
    {
        ComboAttempt.Value += i;
        Debug.Log(ComboAttempt.Value);
    }

    public void ResetScore()
    {
        Score.Value = 0;
    }

    public void CalculateScore(int i)
    {
        Score.Value += i;
    }

    public void OnEnd()
    {
        GameManager.instance.maxCombo = maxCombo;
        GameManager.instance.finalScore=Score.Value;
    }
}

public class ScoreManager : MonoBehaviour
{
    public ScoreManagerStat scoreManager = new ScoreManagerStat();
    [SerializeField] private Text comboText;
    [SerializeField] private Text comboAttemptText;
    [SerializeField] private Text scoreText;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        scoreManager.Combo.Subscribe(x => ComboTextUpdate(x)).AddTo(gameObject);
        scoreManager.ComboAttempt.Subscribe(x => ComboAttempTextUpdate(x)).AddTo(gameObject);
        scoreManager.Score.Subscribe(x => ScoreTextUpdate(x)).AddTo(gameObject);
        GameManager.instance.onEndEvent += scoreManager.OnEnd;
    }

    private void ComboTextUpdate(int x)
    {
        comboText.text = x.ToString();
    }

    private void ComboAttempTextUpdate(int x)
    {
        comboAttemptText.text = x.ToString();
    }

    private void ScoreTextUpdate(float x)
    {
        scoreText.text = x.ToString();
    }
}
