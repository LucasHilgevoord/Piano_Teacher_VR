using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PianoTeacher.Piano.Keys;
using System;

namespace PianoTeacher.Piano
{
    [Serializable]
    public class PianoKeyPrefab
    {
        public GameObject whiteKey;
        public GameObject blackKey;
    }

    public class PianoManager : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField] private PianoCalibrator _calibrator;

        [Header("Layout")]
        [SerializeField] private int _totalKeyCount;
        [SerializeField] private KeyPitches _startingKeyPitch;
        [SerializeField] private Transform _pianoParent;
        [SerializeField] private Transform _keyParent;
        private Vector3 _startingPos = Vector3.zero;
        private int[] _keyLayout = {0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0};
        private List<Key> _keys;

        [Header("Scaling")]
        [SerializeField] private Vector3 _whiteKeyScale;
        [SerializeField] private Vector3 _blackKeyScale;
        [SerializeField] private float _keyOffset;
        [SerializeField] private double _angle;

        [Header("Symbols")]
        private Dictionary<KeyAccidentals, char> _accidentalSymbols = new Dictionary<KeyAccidentals, char>()
        {
            {KeyAccidentals.None, '\0'},
            {KeyAccidentals.Natural, '\u266E'},
            {KeyAccidentals.Sharp, '\u266F'},
            {KeyAccidentals.Flat, '\u266D'}
        };

        [Header("Customization")]
        [SerializeField] private bool _useCalibrator = true;
        [SerializeField] private PianoKeyPrefab _keyPrefabs;
        [SerializeField] private float _volume = 1;
        [SerializeField] private int _octaveOffset = 0;
        [SerializeField] private PianoTimbres _timbre;

        /// <summary>
        /// Method to initialize the piano
        /// </summary>
        internal void Initialize()
        {
            if (_useCalibrator)
            {
                PianoCalibrator.CalibrationComplete += OnCalibrationComplete;
                _calibrator.Initialize(GetKeyCount(), _keyOffset);
            } else
            {
                CreatePiano();
            }
        }

        /// <summary>
        /// Method which starts the creation of the piano after the calibration is complete
        /// </summary>
        /// <param name="calibration"></param>
        private void OnCalibrationComplete(CalibrationData calibration)
        {
            PianoCalibrator.CalibrationComplete -= OnCalibrationComplete;

            // Assign the calibration values
            _startingPos = calibration.startPos;
            _angle = calibration.angle;
            _whiteKeyScale.x = calibration.whiteKeyScaleX;
            _blackKeyScale.x = calibration.blackKeyScaleX;

            // Start the creation of the piano
            CreatePiano();
        }

        /// <summary>
        /// Method to create the piano
        /// </summary>
        private void CreatePiano()
        {
            CreateKeys();
            _pianoParent.position = _startingPos;
            SetPianoRotation();
        }

        /// <summary>
        /// Method to create the keys of the piano
        /// </summary>
        private void CreateKeys()
        {
            int keyPitch = ((int)_startingKeyPitch) - 1;
            int currentKeyInLayout = GetStartingKeyIndex(_startingKeyPitch) - 1;
            
            int pitchLenght = Enum.GetNames(typeof(KeyPitches)).Length;
            _keys = new List<Key>();

            for (int i = 0; i < _totalKeyCount; i++)
            {
                // Itterating through the next values
                currentKeyInLayout++;
                if (currentKeyInLayout > _keyLayout.Length - 1) { currentKeyInLayout = 0; }

                // Starting values
                int keyType = _keyLayout[currentKeyInLayout];
                GameObject keyPrefab;
                KeyAccidentals accidental;
                Vector3 pos = Vector3.zero;
                Vector3 scale;

                switch (keyType)
                {
                    case 0:
                    default:
                        // Create a white key
                        keyPitch++;
                        if (keyPitch > pitchLenght - 1) { keyPitch = 0; }

                        keyPrefab = _keyPrefabs.whiteKey;
                        accidental = KeyAccidentals.None;
                        scale = _whiteKeyScale;

                        // Set the correct position to spawn on
                        if (i != 0) { pos.x = GetPreviousWhiteKey(i).transform.position.x + _whiteKeyScale.x + _keyOffset; }
                        break;
                    case 1:
                        // Create a black key
                        keyPrefab = _keyPrefabs.blackKey;
                        accidental = KeyAccidentals.Sharp;
                        scale = _blackKeyScale;

                        // Set the correct position to spawn on
                        if (i != 0) { pos.x = _keys[i - 1].transform.position.x + _whiteKeyScale.x / 2; }
                        pos.z = pos.z + (_whiteKeyScale.z - _blackKeyScale.z) / 2;
                        break;
                }

                // Add an offset so that the key will be anchored at the bottom
                pos.z += _whiteKeyScale.z / 2;

                // Create the key
                GameObject keyObj = Instantiate(keyPrefab, pos, Quaternion.identity, _keyParent);
                Key key = keyObj.GetComponent<Key>();
                key.Setup((KeyPitches)keyPitch, accidental, 0);
                key.SetScale(scale);
                _keys.Add(key);

                keyObj.name = "Key_" + ((KeyPitches)keyPitch).ToString() + _accidentalSymbols[key.Accidental];
            }
        }

        /// <summary>
        /// Method to set the rotation of the piano
        /// </summary>
        private void SetPianoRotation()
        {
            float deg = (float)(-_angle * 180 / Math.PI) - 180;
            Quaternion rotation = Quaternion.Euler(0, deg, 0);
            _pianoParent.rotation = rotation;
        }

        /// <summary>
        /// Method to find a white key that has been created before the assigned index
        /// </summary>
        /// <param name="myIndex">Index to start searching on</param>
        /// <returns>Previous white key</returns>
        private Key GetPreviousWhiteKey(int myIndex)
        {
            for (int i = myIndex - 1; i >= 0; i--)
            {
                if (_keys[i].Accidental == KeyAccidentals.None)
                {
                    return _keys[i];
                }
            }

            Debug.LogError("Unable to find a white key that has been assigned before index: " + myIndex);
            return null;
        }

        /// <summary>
        /// Method to find which key the piano should start with in the array of _keyLayout
        /// </summary>
        /// <param name="pitch">The pitch that the piano should start with</param>
        /// <returns>The index of the keytype</returns>
        private int GetStartingKeyIndex(KeyPitches pitch)
        {
            int keyIndex = -1;
            //Debug.Log("Searching for white key: " + (int)pitch);
            for (int i = 0; i < _keyLayout.Length; i++)
            {
                if (_keyLayout[i] == 0)
                {
                    keyIndex++;
                    if (keyIndex == (int)pitch) 
                    { 
                        //Debug.Log("Found the " + (int)pitch + "th white key with index: " + i);
                        return i; 
                    }
                }
            }

            Debug.LogError("Unable to find a white key that matches the assigned pitch: " + pitch.ToString()) ;
            return 0;
        }

        /// <summary>
        /// Get the amount of black and white keys that will be instantiated
        /// </summary>
        /// <returns>Vector2(whiteKeys, blackKeys)</returns>
        private Vector2 GetKeyCount()
        {
            Vector2 keyCount = Vector2.zero;
            int currentKeyInLayout = GetStartingKeyIndex(_startingKeyPitch) - 1;

            for (int i = 0; i < _totalKeyCount; i++)
            {
                currentKeyInLayout++;
                if (currentKeyInLayout > _keyLayout.Length - 1) { currentKeyInLayout = 0; }

                switch (_keyLayout[currentKeyInLayout])
                {
                    case 0:
                        keyCount.x++;
                        break;
                    case 1:
                        keyCount.y++;
                        break;
                }
            }

            return keyCount;
        }

        /// <summary>
        /// Get the width/height of the piano
        /// </summary>
        /// <returns>Vector2(width, height)</returns>
        public Vector2 GetPianoSize()
        {
            float width = (this._whiteKeyScale.x + _keyOffset) * GetKeyCount().x - _keyOffset;
            float height = this._whiteKeyScale.z;
            return new Vector2(width, height);
        }

        /// <summary>
        /// Get the keys which have been created
        /// </summary>
        /// <returns>Array of keys</returns>
        internal Key[] GetKeys() { return _keys.ToArray(); }

        /// <summary>
        /// Get the most left and top position of the first key
        /// </summary>
        /// <returns></returns>
        internal Vector3 GetFirstKeyPosition() 
        {
            Vector3 pos = _pianoParent.position;
            double angle = (-_pianoParent.eulerAngles.y + 90) * Math.PI / 180;
            pos.x = (float)(_whiteKeyScale.z * Math.Cos(angle)) - (_whiteKeyScale.x / 2);
            pos.z = (float)(_whiteKeyScale.z * Math.Sin(angle));
            return pos;
        }

        internal Vector3 GetPianoRotation() { return _pianoParent.eulerAngles; }
    }
}
