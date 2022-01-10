using UnityEngine;
using TMPro;
using DG.Tweening;
using UniRx;
using UnityEngine.SceneManagement;

public class GameStartScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pressText;
    [SerializeField] UIButton startButton;
    private void Awake()
    {
        Sequence textSequence = DOTween.Sequence();
        textSequence
            .Append(pressText.DOFade(0, 0.5f))
            .Append(pressText.DOFade(1, 0.5f))
            .SetLoops(-1, LoopType.Restart);
        startButton.OnPointerUp.Subscribe(_ => 
        SceneManager.LoadScene("MusicSelect")).AddTo(gameObject);
    }
}
