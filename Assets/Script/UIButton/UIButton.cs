using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;
[RequireComponent(typeof(ObservablePointerDownTrigger))]
[RequireComponent(typeof(ObservablePointerUpTrigger))]
public class UIButton : MonoBehaviour
{
    public IObservable<UnityEngine.EventSystems.PointerEventData> OnPointerDown;
    public IObservable<UnityEngine.EventSystems.PointerEventData> OnPointerUp;
    private void Awake()
    {
        ObservablePointerDownTrigger pointerDown = GetComponent<ObservablePointerDownTrigger>();
        ObservablePointerUpTrigger pointerUp = GetComponent<ObservablePointerUpTrigger>();
        OnPointerDown = pointerDown.OnPointerDownAsObservable();
        OnPointerUp = pointerUp.OnPointerUpAsObservable();
    }
}
