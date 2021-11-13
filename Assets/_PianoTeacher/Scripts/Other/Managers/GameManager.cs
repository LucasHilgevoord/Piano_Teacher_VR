using PianoTeacher.Piano;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTeacher
{
    public class GameManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private PianoManager pianoManager;

        [Header("Modes")]
        [SerializeField] private object displayManager; // Not yet implemented
        [SerializeField] private object sheetManager; // Not yet implemented

        // Start is called before the first frame update
        void Start()
        {
            pianoManager.CreatePiano();
        }
    }
}
