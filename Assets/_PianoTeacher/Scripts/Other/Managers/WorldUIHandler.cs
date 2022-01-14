using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace PianoTeacher.UI
{
    public class WorldUIHandler : MonoBehaviour
    {
        private int _dotCount = 0;
        private int _maxDots = 3;
        private Text dotText;
        private string ogDotText;

        [Header("Leap status")]
        [SerializeField] private GameObject _leapStatus;
        [SerializeField] private Text statusHeader;
        [SerializeField] private Color active, inactive;
        public Image LeftHand, RightHand;

        [Header("Calibration")]
        [SerializeField] private GameObject _calibrating;

        /// <summary>
        /// Show the status UI
        /// </summary>
        internal void ShowLeapStatus()
        {
            _leapStatus.SetActive(true);
            UpdateLeapStatus(LeftHand, false);
            UpdateLeapStatus(RightHand, false);
            StartDotting(statusHeader);
        }

        /// <summary>
        /// Hide the status UI
        /// </summary>
        internal void HideLeapStatus()
        {
            _leapStatus.SetActive(false);
            StopDotting();
        }

        /// <summary>
        /// Update a hand icon with the corresponding status
        /// </summary>
        /// <param name="img"></param>
        /// <param name="isActive"></param>
        internal void UpdateLeapStatus(Image img, bool isActive) { img.color = isActive ? active : inactive; }

        /// <summary>
        /// Start 'animating' the text with appearing dots
        /// </summary>
        /// <param name="text">Text to animate</param>
        private void StartDotting(Text text)
        {
            _dotCount = 0;
            dotText = text;
            ogDotText = dotText.text;
            InvokeRepeating("UpdateDot", 0, 0.5f);
        }

        /// <summary>
        /// Stop 'animating' the text with appearing dots
        /// </summary>
        private void StopDotting() { CancelInvoke("UpdateDot"); }

        /// <summary>
        /// Update the text with more or less dots
        /// </summary>
        [UsedImplicitly]
        private void UpdateDot()
        {
            _dotCount++;
            if (_dotCount > _maxDots)
            {
                _dotCount = 0;
                dotText.text = ogDotText;
                return;
            }
            dotText.text += ".";
        }

        /// <summary>
        /// Toggle the calibrating UI
        /// </summary>
        /// <param name="show">show/hide</param>
        internal void ToggleCalibrating(bool show) { _calibrating.SetActive(show); }      
    }

}