using MidiPlayerTK;
using PianoTeacher.Display;
using PianoTeacher.Piano;
using PianoTeacher.Piano.Keys;
using UnityEngine;

namespace PianoTeacher
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PianoManager _pianoManager;
        [SerializeField] private MidiController _midiController;

        [Header("Modes")]
        [SerializeField] private DisplayManager _displayManager;
        [SerializeField] private object _sheetManager; // Not yet implemented

        private void Awake()
        {
            _midiController.OnNoteCreated += OnNoteCreated;
            _displayManager.OnNoteTriggered += TriggerNote;
        }

        // Start is called before the first frame update
        void Start()
        {
            PianoManager p = _pianoManager;
            p.Initialize();
            _displayManager.Initialize(p.GetFirstKeyPosition(), p.GetPianoRotation(), p.GetPianoSize());
        }

        /// <summary>
        /// Method after a note has been created by the midiController
        /// </summary>
        /// <param name="note">Created note</param>
        private void OnNoteCreated(MPTKEvent note)
        {
            // Testing
            //_midiController.PlayNote(note);

            int octaveOffset = _pianoManager.OctaveOffset * _pianoManager.KeyLayout.Length;
            Key[] keys = _pianoManager.GetKeys();
            if (note.Value + octaveOffset > keys.Length)
            {
                Debug.LogWarning("Not enough keys to play key with index: " + note.Value);
                return;
            }

            Debug.Log("NOTE CREATED ON KEY: [" + note.Value + "] | " + note);
            Key key = keys[note.Value + octaveOffset];
            _displayManager.CreateDisplayKey(note, key.transform.localScale.x, key.transform.position.x);
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
