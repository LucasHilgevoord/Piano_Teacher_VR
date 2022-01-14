using Leap.Unity;
using UnityEngine;

namespace PianoTeacher.Leap
{
    public class LeapManager : MonoBehaviour
    {
        public LeapHand LeftHand, RightHand;

        internal bool IsTracked(LeapHand hand) { return hand.gameObject.activeInHierarchy; }

        /// <summary>
        /// Get the position of the tip from the index finger
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        internal Vector3 GetIndexFingerTip(LeapHand hand)
        {
            if (!IsTracked(hand)) { return Vector3.zero; }
            return Hands.GetIndex(hand.HandModelBase.GetLeapHand()).TipPosition.ToVector3();
        }
    }
}