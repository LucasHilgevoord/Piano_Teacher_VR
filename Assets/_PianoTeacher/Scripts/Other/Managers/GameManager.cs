using PianoTeacher.Display;
using PianoTeacher.Piano;
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

        // Start is called before the first frame update
        void Start()
        {
            _pianoManager.Initialize();
            _displayManager.Initialize();
        }
    }
}
