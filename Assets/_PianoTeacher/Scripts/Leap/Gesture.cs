using Leap;
using System;
using UnityEngine;

namespace PianoTeacher.Leap
{
    public enum Gestures 
    { 
        Pinch,
        Grab
    }
    
    public class Gesture : MonoBehaviour
    {
        public event Action<Gestures> GestureActivated;
        public event Action<Gestures> GestureDeactivated;

        [Header("General")]
        protected LeapHand _handBase;
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private float _activationDelay;

        protected Hand _hand;
        protected Gestures _gesture;

        private float _lastTimeUsed;
        protected bool _isActive;
        public bool IsActive => _isActive;

        protected virtual void Awake()
        {
            _handBase = GetComponentInParent<LeapHand>();
        }

        protected virtual void Start()
        {
            _hand = _handBase.HandModelBase.GetLeapHand();
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

        /// <summary>
        /// Check the conditions of the gesture
        /// </summary>
        protected virtual void CheckGestureState() { }

        /// <summary>
        /// Update the state of the gesture
        /// </summary>
        /// <param name="active">if the gesture is currently active</param>
        protected virtual void UpdateGesturestate(bool active)
        {
            // Only activate if it is not already active and if the delay is over
            if (_isActive == active || (active && Time.time - _lastTimeUsed <= _activationDelay)) return;
            _isActive = active;

            if (active)
            {
                GestureActivated?.Invoke(_gesture);
                Debug.Log("Gesture activated");
            }
            else
            {
                Debug.Log("Gesture deactivated");
                _lastTimeUsed = Time.time;
                GestureDeactivated?.Invoke(_gesture);
            }
        }
    }
}