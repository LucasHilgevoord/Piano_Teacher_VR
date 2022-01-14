using Leap;
using Leap.Unity;
using UnityEngine;

namespace PianoTeacher.Leap
{
    public class PinchGesture : Gesture
    {
        [Header("Pinch Gesture")]
        [SerializeField] private Finger.FingerType _pinchFinger = Finger.FingerType.TYPE_INDEX;
        [SerializeField] private float _pinchDistance = .02f;

        protected override void Start()
        {
            base.Start();
            _gesture = Gestures.Pinch;
        }

        protected override void CheckGestureState()
        {
            base.CheckGestureState();
            UpdateGesturestate(GetDistance() < _pinchDistance);
        }

        /// <summary>
        /// Calculate the distance of the thumb and index finger
        /// </summary>
        /// <returns></returns>
        private float GetDistance()
        {
            Vector3 thumbPos = Hands.GetThumb(_hand).TipPosition.ToVector3();
            Vector3 indexPos = _hand.Fingers[(int)_pinchFinger].TipPosition.ToVector3();
            return Vector3.Distance(thumbPos, indexPos);
        }
    }
}
