using MidiPlayerTK;
using PianoTeacher.Piano;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace PianoTeacher.Display
{
    public class DisplayManager : MonoBehaviour
    {
        public event Action<DisplayNote> OnNoteTriggered;

        [Header("Canvas")]
        [SerializeField] private Canvas _noteCanvas;
        private RectTransform _noteCanvasRect;
        [SerializeField] private float _canvasAngle;
        [SerializeField] private Transform _noteParent;

        [Header("Display")]
        [SerializeField] private float _noteSpeed;
        [SerializeField] private DisplayNote _notePrefab;
        private List<DisplayNote> _activeNotes;

        [Header("Preset")]
        private float noteVelocity = 0.00001f;
        private float noteDuration = 0.00013f;

        //[SerializeField, Range(0, 1), Tooltip("Determines if the note scaling is using scale or speed as reference, or a mix in between")] 
        //private float _match = 0.5f;

        /// <summary>
        /// Initialize the display manager
        /// </summary>
        /// <param name="pos">Canvas position</param>
        /// <param name="rot">Canvas rotation</param>
        /// <param name="size">Canvas size</param>
        internal void Initialize(Vector3 pos, Vector3 rot, Vector2 size)
        {
            _activeNotes = new List<DisplayNote>();
            _noteCanvasRect = _noteCanvas.GetComponent<RectTransform>();
            SetupCanvas(pos, rot, size);
        }

        private void Update()
        {
            UpdateDisplayKeys();
        }

        /// <summary>
        /// Setup the canvas so it can be created
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="rot">Rotation</param>
        /// <param name="size">Size</param>
        private void SetupCanvas(Vector3 pos, Vector3 rot, Vector2 size)
        {
            RectTransform rect =_noteCanvas.GetComponent<RectTransform>();

            // Set the size of the canvas
            float width = size.x;
            rect.sizeDelta = new Vector2(width, width / 2); // TODO: Custom height

            // Set the position of the canvas
            _noteCanvas.transform.position = pos;

            // Set the rotation of the canvas to match the rotation of the piano
            rot.x += _canvasAngle;
            _noteCanvas.transform.eulerAngles = rot;
        }

        /// <summary>
        /// Create a key
        /// </summary>
        /// <param name="note"></param>
        internal void CreateDisplayKey(MPTKEvent ev, float width, float xPos)
        {
            // TODO: Filter all the data we need instead of passing the whole MPTKEvent

            // TODO: Make a object pool
            DisplayNote note = Instantiate(_notePrefab, _noteParent);

            note.data = ev;
            note.SetScale(width, ev.Duration * noteDuration);
            note.SetPosition(xPos, note.GetHeight());
            note.SetNoteLabel(""); //TODO: Set key
            _activeNotes.Add(note);
        }

        /// <summary>
        /// Update all current active keys
        /// </summary>
        private void UpdateDisplayKeys()
        {
            // Update all the notes in the active note list
            foreach (DisplayNote note in _activeNotes)
            {
                // Temp
                if (!note.isActiveAndEnabled) continue;

                // Move the note down
                note.MoveNote(Vector3.down * (note.data.Velocity * noteVelocity));

                // Check the state of the note
                if (note.IsPlayed && note.Rect.anchoredPosition.y < -_noteCanvasRect.rect.height)
                {
                    // Stop playing
                    note.KilNote();
                }
                else if (!note.IsPlayed && note.Rect.anchoredPosition.y < -_noteCanvasRect.rect.height + note.GetHeight())
                {
                    // Start playing
                    note.IsPlayed = true;
                    OnNoteTriggered?.Invoke(note);
                } else if (note.Rect.anchoredPosition.y < -_noteCanvasRect.rect.height + note.GetHeight())
                {
                    // Is busy playing
                }
            }
        }
    }
}