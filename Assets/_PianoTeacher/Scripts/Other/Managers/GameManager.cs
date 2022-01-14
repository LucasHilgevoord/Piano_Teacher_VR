using MidiPlayerTK;
using PianoTeacher.Display;
using PianoTeacher.Midi;
using PianoTeacher.Piano;
using PianoTeacher.Piano.Keys;
using UnityEngine;

namespace PianoTeacher
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PianoManager _pianoManager;
        [SerializeField] private MidiController _midiController;
        [SerializeField] private CalibrationManager _calibrationManager;

        [Header("Modes")]
        [SerializeField] private DisplayManager _displayManager;
        [SerializeField] private object _sheetManager; // Not yet implemented

        [Header("Customization")]
        [SerializeField] private Color _color;

        private void Awake()
        {
            _midiController.OnNoteCreated += OnNoteCreated;
            _displayManager.OnNoteTriggered += TriggerNote;
        }
        
        private void OnDestroy()
        {
            _midiController.OnNoteCreated -= OnNoteCreated;
            _displayManager.OnNoteTriggered -= TriggerNote;
        }

        // Start is called before the first frame update
        void Start()
        {
            _pianoManager.IsInitialized += OnPianoInitialized;
            _pianoManager.Initialize();

            // Check if we use the calibrator is succesfully initialized
            if (_pianoManager.Calibrator.IsInitialized)
            {
                _calibrationManager.Initialize();
            }
        }

        /// <summary>
        /// Method after the piano has been created so we can initialize the display
        /// </summary>
        private void OnPianoInitialized()
        {
            _pianoManager.IsInitialized -= OnPianoInitialized;

            _displayManager.IsInitialized += OnDisplayInitialized;
            PianoManager p = _pianoManager;
            _displayManager.Initialize(p.GetFirstKeyWorldPosition(), p.GetPianoRotation(), p.GetPianoSize());
        }

        /// <summary>
        /// Method after the display has been initialized
        /// </summary>
        private void OnDisplayInitialized()
        {
            _displayManager.IsInitialized -= OnDisplayInitialized;
            _midiController.Initialize();
        }

        /// <summary>
        /// Method after a note has been created by the midiController
        /// </summary>
        /// <param name="note">Created note</param>
        private void OnNoteCreated(MPTKEvent note)
        {
            int octaveOffset = _pianoManager.OctaveOffset * _pianoManager.KeyLayout.Length;
            Key[] keys = _pianoManager.GetKeys();
            if (note.Value + octaveOffset > keys.Length)
            {
                Debug.LogWarning("Not enough keys to play key with index: " + note.Value);
                return;
            }

            Debug.Log("NOTE CREATED ON KEY: [" + note.Value + "] | " + note);
            Key key = keys[note.Value + octaveOffset];
            _displayManager.CreateDisplayKey(note, key.transform.localScale.x, key.transform.localPosition.x);
        }

        /// <summary>
        /// Called whenever a note has reached the point that is should be played
        /// </summary>
        private void TriggerNote(DisplayNote note)
        {
            _midiController.PlayNote(note.data);
            note.EnableTriggered();
        }
    }
}
