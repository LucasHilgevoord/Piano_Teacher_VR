using Leap.Unity;
using System;
using UnityEngine;

namespace PianoTeacher.Leap
{
    [RequireComponent(typeof (LeapHand))]
    public class GestureHandler : MonoBehaviour
    {
        public event Action<Gestures> GestureActivated, GestureDeactivated;
        private Gesture[] gestures;

        private void Awake()
        {
            gestures = this.gameObject.GetComponentsInChildren<Gesture>();

            if (gestures != null)
            {
                foreach (Gesture g in gestures)
                {
                    Debug.Log("Listening to gesture: " + g);
                    g.GestureActivated += OnGestureActivated;
                    g.GestureDeactivated += OnGestureDeactivated;
                }
            }
        }

        private void OnDestroy()
        {
            if (gestures != null)
            {
                foreach (Gesture g in gestures)
                {
                    g.GestureActivated -= OnGestureActivated;
                    g.GestureDeactivated -= OnGestureDeactivated;
                }
            }
        }

        private void OnGestureActivated(Gestures type) { Debug.Log("GestureHandler: Gesture: " + type); GestureActivated?.Invoke(type); }

        private void OnGestureDeactivated(Gestures type) { GestureDeactivated?.Invoke(type); }

    }
}