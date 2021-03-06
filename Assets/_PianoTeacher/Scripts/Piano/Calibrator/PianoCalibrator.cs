using System;
using UnityEngine;

namespace PianoTeacher.Piano
{
    public enum Markers
    {
        Left = 0,
        Right = 1
    }

    public class PianoCalibrator : MonoBehaviour
    {
        private bool initialized;
        internal bool IsInitialized => initialized;

        [Header("Calibration")]
        private bool _markedLeft, _markedRight;
        private Vector3 _leftMarkedPos, _rightMarkedPos;
        private Vector2 _keyCount;
        private float _keyOffset;

        // Events
        public static event Action<CalibrationData> CalibrationComplete;

        /// <summary>
        /// Initialization of the calibrator
        /// </summary>
        /// <param name="keyCount">Amount of keys that the piano will have</param>
        /// <param name="offset">Offset between the white keys</param>
        internal void Initialize(Vector2 keyCount, float offset)
        {
            _keyCount = keyCount;
            _keyOffset = offset;

            initialized = true;
        }

        /// <summary>
        /// Method to set the positions of the keys in virtual space
        /// </summary>
        /// <param name="marker">Marker that will be set</param>
        internal void SetMarkerPos(Transform marker, Markers type)
        {
            if (type == Markers.Left)
            {
                Debug.Log("Calibrator: Left marker has been set");
                _leftMarkedPos = marker.position;
            }
            else if (type == Markers.Right)
            {
                Debug.Log("Calibrator: Right marker has been set");
                _rightMarkedPos = marker.position;
            }

            OnMarkerSet(type);
        }

        /// <summary>
        /// Method which checks if both markers have been set and if calibration can begin
        /// </summary>
        /// <param name="marker">Marker that has been set</param>
        private void OnMarkerSet(Markers type)
        {
            if (type == Markers.Left) { _markedLeft = true; }
            if (type == Markers.Right) { _markedRight = true; }

            if (_markedLeft && _markedRight)
            {
                StartCalibration();
            }
        }

        /// <summary>
        /// Method to start the calibration
        /// </summary>
        private void StartCalibration()
        {
            Debug.Log("Calibrator: Starting calibration...");

            // Calculate angle
            double angle = GetAngle(_leftMarkedPos, _rightMarkedPos);

            // Calculate width
            double width = GetWidth(_leftMarkedPos, _rightMarkedPos);
            float offset = (_keyOffset * (_keyCount.x - 1));
            float whiteScaleX = ((float)width - offset) / (_keyCount.x - 1);

            float whiteKeyScaleX = whiteScaleX;
            float blackKeyScaleX = whiteScaleX * 0.75f;

            // Set data
            CalibrationData data = new CalibrationData(_leftMarkedPos, angle, whiteKeyScaleX, blackKeyScaleX);
            Debug.Log("Calibrator: Calibration complete!");
            CalibrationComplete?.Invoke(data);
        }

        /// <summary>
        /// Method to get the angle between two condinates
        /// </summary>
        /// <param name="a">Object A</param>
        /// <param name="b">Object B</param>
        /// <returns>Angle between the two points in radiants</returns>
        private double GetAngle(Vector3 a, Vector3 b) { return Math.Atan2(a.z - b.z, a.x - b.x); }

        /// <summary>
        /// Method to get the width between two condinates
        /// </summary>
        /// <param name="a">Object A</param>
        /// <param name="b">Object B</param>
        /// <returns>Width between the two points</returns>
        private double GetWidth(Vector3 a, Vector3 b) 
        {
            double xDiff = Math.Abs(a.x - b.x);
            double zDiff = Math.Abs(a.z - b.z);
            return Math.Sqrt(((xDiff * xDiff) + (zDiff * zDiff)));
        }
    }
}