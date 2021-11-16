using PianoTeacher.Display;
using PianoTeacher.Piano;
using UnityEngine;

namespace PianoTeacher
{
    public class GameManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private PianoManager _pianoManager;

        [Header("Modes")]
        [SerializeField] private DisplayManager _displayManager;
        [SerializeField] private object _sheetManager; // Not yet implemented

        // Start is called before the first frame update
        void Start()
        {
            _pianoManager.Initialize();
            _displayManager.Initialize(Vector3.zero, Vector3.zero, 0);
        }
    }
}
