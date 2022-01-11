using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTeacher.Leap
{
    public class LeapManager : MonoBehaviour
    {
        public event Action GestureRecognized;
        [SerializeField] private CapsuleHand leftHand, rightHand;

        private void OnGestureRecognized()
        {

        }
    }
}