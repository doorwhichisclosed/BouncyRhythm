using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public PlayerMove player;
    // Start is called before the first frame update
    void Start()
    {
        var update = Observable.EveryUpdate();
        update.Subscribe(_ => transform.DOMove(new Vector3(0, player.transform.position.y, -10), 0.3f));
    }
}
