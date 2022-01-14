using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

namespace PianoTeacher.Leap
{
    public class LeapHand : MonoBehaviour
    {
        internal HandModelBase HandModelBase;
        internal GestureHandler GestureHandler;

        private void Awake()
        {
            HandModelBase = GetComponent<HandModelBase>();
            GestureHandler = GetComponent<GestureHandler>();
        }
    }
}