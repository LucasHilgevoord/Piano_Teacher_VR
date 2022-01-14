using MidiPlayerTK;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTeacher.Display
{
    public class DisplayManager : MonoBehaviour
    {
        public event Action IsInitialized;
        public event Action<DisplayNote> OnNoteTriggered;
        private bool isInitialized;

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
        [SerializeField] private Color _displayColor;
        private float midiVelocity;

        //TODO: Figure ot a cleaner way
        private float noteVelocity = 0.0015f;
        private float noteDuration = 0.000058f;

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

            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized) return;
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
            note.SetScale(width, (ev.Duration * noteDuration) * 2);
            note.SetPosition(xPos, note.GetHeight());
            note.SetLabel(""); //TODO: Set key
            note.SetColor(_displayColor);
            _activeNotes.Add(note);

            // Set the velocity to the velocity of the first note to keep a constant velocity
            midiVelocity = note.data.Velocity;
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
                note.MoveNote(Vector3.down * ((midiVelocity * noteVelocity) * Time.deltaTime));

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