using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchGesture : Gesture
{
    private float _pinchDistance = .015f;

    protected override void CheckGestureState()
    {
        base.CheckGestureState();
        UpdateGesturestate(GetDistance() < _pinchDistance);
    }

    private float GetDistance()
    {
        var thumbPos = Hands.GetThumb(_hand).TipPosition.ToVector3();
        var indexPos = Hands.GetIndex(_hand).TipPosition.ToVector3();
        return Vector3.Distance(thumbPos, indexPos);
    }
}
