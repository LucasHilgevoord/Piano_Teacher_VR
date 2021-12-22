using MidiPlayerTK;
using PianoTeacher.Piano;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTeacher.Display
{
    public class DisplayManager : MonoBehaviour
    {
        [SerializeField] private PianoManager _pianoManager;
        [SerializeField] private MidiController _midiController;

        [Header("Canvas")]
        [SerializeField] private Canvas _noteDisplay;
        [SerializeField] private float _canvasAngle;

        [Header("Display")]
        [SerializeField] private float _noteSpeed;
        [SerializeField] DisplayNote notePrefab;
        private List<DisplayNote> _activeNotes;

        [Header("Preset")]
        [SerializeField] private bool _usePresetValues = false;

        private void Awake()
        {
            _midiController.OnNoteCreated += CreateDisplayKey;
        }

        internal void Initialize()
        {
            SetupCanvas();
        }

        private void SetupCanvas()
        {
            RectTransform rect =_noteDisplay.GetComponent<RectTransform>();
            Vector2 pianoSize = _pianoManager.GetPianoSize();

            // Set the size of the canvas
            float width = pianoSize.x;
            rect.sizeDelta = new Vector2(width, width / 2); // TODO: Custom height

            // Set the position of the canvas
            _noteDisplay.transform.position = _pianoManager.GetFirstKeyPosition();

            // Set the rotation of the canvas to match the rotation of the piano
            Vector3 rot = _pianoManager.GetPianoRotation();
            rot.x += _canvasAngle;
            _noteDisplay.transform.eulerAngles = rot;
        }

        private void CreateDisplayKey(MPTKEvent note)
        {
            Debug.Log("NOTE CREATED: " + note);
        }

        private void UpdateDisplayKeys()
        {

        }
    }
}