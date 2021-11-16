using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTeacher.Display
{
    public class DisplayManager : MonoBehaviour
    {
        [SerializeField] private Canvas _noteDisplay;

        [Header("Customization")]
        [SerializeField] private bool _usePresetValues = false;
        [SerializeField] private float _noteSpeed;

        // Canvas
        [SerializeField] private Vector3 _startPos;
        [SerializeField] private float _canvasAngle;
        [SerializeField] private Vector2 _canvasScale;
        [SerializeField] private Vector3 _whiteKeyScale;
        [SerializeField] private int _totalWhiteKeys;
        internal void Initialize(Vector3 startPos, Vector3 whiteKeyScale, int totalWhiteKeys)
        {
            if (!_usePresetValues)
            {
                _startPos = startPos;
                _whiteKeyScale = whiteKeyScale;
                _totalWhiteKeys = totalWhiteKeys;
            }

            SetupCanvas();
        }

        private void SetupCanvas()
        {
            _startPos.x -= _whiteKeyScale.x / 2;
            _startPos.z += _whiteKeyScale.z;
            _noteDisplay.transform.position = _startPos;

            RectTransform rect =_noteDisplay.GetComponent<RectTransform>();
            if (!_usePresetValues)
            {
                float width = _whiteKeyScale.x * _totalWhiteKeys + (_whiteKeyScale.x * 1.5f);
                _canvasScale = new Vector2(width, width);
            }

            rect.sizeDelta = _canvasScale;


            // Set angle
            float deg = (float)(-_canvasAngle * 180 / Math.PI);
            Quaternion rotation = Quaternion.Euler(deg, 0, 0);
            _noteDisplay.transform.rotation = rotation;
        }

        private void SetAngle()
        {

        }
    }
}