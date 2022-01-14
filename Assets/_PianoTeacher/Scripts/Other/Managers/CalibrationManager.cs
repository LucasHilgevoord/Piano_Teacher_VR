using PianoTeacher.Leap;
using PianoTeacher.UI;
using UnityEngine;

namespace PianoTeacher.Piano
{
    public class CalibrationManager : MonoBehaviour
    {
        private bool isInitialized;
        private int calibrationStep = -1;
        [Header("Helpers")]
        [SerializeField] private WorldUIHandler _worldUIHandler;
        [SerializeField] private LeapManager _leapManager;

        [Header("Status")]
        private bool _checkingStatus;
        private bool _leftHandVisible, _rightHandVisible;

        [Header("Calibration")]
        [SerializeField] private bool _useLeap;
        [SerializeField] PianoCalibrator _calibrator;
        [SerializeField] private Transform _leftMarker, _rightMarker;
        private bool _isCalibrating;
        private bool _setLeftMarker, _setRightMarker;

        /// <summary>
        /// Initialize the calibration manager
        /// </summary>
        internal void Initialize()
        {
            isInitialized = true;
            if (_useLeap)
            {
                NextCalibrationStep();
            } else
            {
                _calibrator.SetMarkerPos(_leftMarker, Markers.Left);
                _calibrator.SetMarkerPos(_rightMarker, Markers.Right);
            }
        }

        private void Update()
        {
            if (!isInitialized) { return; }
            if (_checkingStatus) { UpdateHandStatus(); }
            if (_isCalibrating) { UpdateMarkerPosition(); }
        }

        /// <summary>
        /// Method to go to the next step of the setup
        /// </summary>
        private void NextCalibrationStep()
        {
            calibrationStep++;
            switch (calibrationStep)
            {
                case 0:
                    StartHandStatus();
                    break;
                case 1:
                    StartCalibration();
                    break;
                case 2:

                    break;
            }
        }

        /// <summary>
        /// Start showing the leap hand status
        /// </summary>
        private void StartHandStatus()
        {
            _worldUIHandler.ShowLeapStatus();
            _checkingStatus = true;
        }

        /// <summary>
        /// Update the UI with the correct status of both hands
        /// </summary>
        private void UpdateHandStatus()
        {
            // Check the status of the left hand
            bool leftStatus = _leapManager.IsTracked(_leapManager.RightHand);
            if (_leftHandVisible != leftStatus)
            {
                _worldUIHandler.UpdateLeapStatus(_worldUIHandler.LeftHand, leftStatus);
                _leftHandVisible = leftStatus;
            }

            // Check the status of the right hand
            bool rightStatus = _leapManager.IsTracked(_leapManager.LeftHand);
            if (_rightHandVisible != rightStatus)
            {
                _worldUIHandler.UpdateLeapStatus(_worldUIHandler.RightHand, rightStatus);
                _rightHandVisible = rightStatus;
            }

            // Disable the window if both hands are visible
            if (_leftHandVisible && _rightHandVisible) 
            {
                _checkingStatus = false;
                _worldUIHandler.HideLeapStatus();
                NextCalibrationStep();
            }
        }

        /// <summary>
        /// Start the calibration sequence
        /// </summary>
        private void StartCalibration()
        {
            _worldUIHandler.ToggleCalibrating(true);
            _setLeftMarker = _setRightMarker = false;
            _isCalibrating = true;

            // Set up the left marker first with the pinch gesture of the right hand
            _leftMarker.gameObject.SetActive(true);
            _leapManager.RightHand.GestureHandler.GestureActivated += SetCalibrationMarker;
        }

        /// <summary>
        /// Update the markers on the correct finger spot
        /// </summary>
        private void UpdateMarkerPosition()
        {
            if (!_setLeftMarker)
            {
                _leftMarker.position = _leapManager.GetIndexFingerTip(_leapManager.LeftHand);
            } else
                _rightMarker.position = _leapManager.GetIndexFingerTip(_leapManager.RightHand);
        }

        /// <summary>
        /// Set the marker on a fixed spot
        /// </summary>
        private void SetCalibrationMarker(Gestures gesture)
        {
            if (gesture != Gestures.Pinch) return;

            if (!_setLeftMarker)
            {
                Debug.Log("Set Left marker");
                // Finalize the left marker position
                _leapManager.RightHand.GestureHandler.GestureActivated -= SetCalibrationMarker;
                _calibrator.SetMarkerPos(_leftMarker, Markers.Left);
                _setLeftMarker = true;

                // Set up the right marker with the pinch gesture of the left hand
                _rightMarker.gameObject.SetActive(true);
                _leapManager.LeftHand.GestureHandler.GestureActivated += SetCalibrationMarker;
            }
            else if (!_setRightMarker)
            {
                Debug.Log("Set Right marker");
                // Finalize the right marker position
                _leapManager.LeftHand.GestureHandler.GestureActivated -= SetCalibrationMarker;
                _calibrator.SetMarkerPos(_rightMarker, Markers.Right);
                _setRightMarker = true;
            }

            if (_setLeftMarker && _setRightMarker)
            {
                // Both markers have been set!
                _isCalibrating = false;
                _rightMarker.gameObject.SetActive(false);
                _leftMarker.gameObject.SetActive(false);

                _worldUIHandler.ToggleCalibrating(false);
                NextCalibrationStep();
            }
        }
    }
}