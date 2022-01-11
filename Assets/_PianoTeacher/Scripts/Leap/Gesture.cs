using Leap;
using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gesture : MonoBehaviour
{
    public event Action GestureActivated;
    public event Action GestureDeactivated;

    [SerializeField] private bool _isEnabled = true;
    [SerializeField] protected HandModelBase _handBase;
    [SerializeField] private float _activationDelay;

    protected Hand _hand;
    private float _lastTimeUsed;
    protected bool _isActive;
    public bool IsActive => _isActive;

    protected virtual void Start()
    {
        _hand = _handBase.GetLeapHand();
    }

    protected virtual void OnDisable()
    {
        UpdateGesturestate(false);
    }

    protected virtual void Update() 
    { 
        if (!_isEnabled) return; 
        CheckGestureState(); 
    }

    protected virtual void CheckGestureState() { }

    protected virtual void UpdateGesturestate(bool active)
    {
        // Only activate if it is not already active and if the delay is over
        if (_isActive == active || (active && Time.time - _lastTimeUsed >= _activationDelay)) return;
        _isActive = active;

        if (active)
        {
            GestureActivated?.Invoke();
            Debug.Log("Gesture activated");
        } else
        {
            Debug.Log("Gesture deactivated");
            _lastTimeUsed = Time.time;
            GestureDeactivated?.Invoke();
        }
    }
}
