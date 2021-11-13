using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTeacher.Piano.Keys
{
    public class Key : MonoBehaviour
    {
        private KeyPitches _pitch;
        public KeyPitches Pitch { get { return _pitch; } }

        private KeyAccidentals _accidental;
        public KeyAccidentals Accidental { get { return _accidental; } }

        private int _octave;
        public int Octave { get { return _octave; } }

        public void Setup(KeyPitches pitch, KeyAccidentals accidental, int octave)
        {
            _pitch = pitch;
            _accidental = accidental;
            _octave = octave;
        }

    }
}
