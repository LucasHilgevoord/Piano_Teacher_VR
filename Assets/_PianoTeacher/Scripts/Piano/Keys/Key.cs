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

        /// <summary>
        /// Method to setup the key with the properties of this key
        /// </summary>
        /// <param name="pitch">Pitch</param>
        /// <param name="accidental">Accidental</param>
        /// <param name="octave">Octave</param>
        public void Setup(KeyPitches pitch, KeyAccidentals accidental, int octave)
        {
            _pitch = pitch;
            _accidental = accidental;
            _octave = octave;
        }

        /// <summary>
        /// Method to set the scale of the key
        /// </summary>
        /// <param name="localScale">Scale to apply</param>
        public void SetScale(Vector3 localScale)
        {
            this.gameObject.transform.localScale = localScale;
        }
    }
}
