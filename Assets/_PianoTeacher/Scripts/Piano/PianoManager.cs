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
        [Header("Keyboard layout")]
        [SerializeField] private int keyCount;
        [SerializeField] private KeyPitches startingKeyPitch;
        [SerializeField] private Transform keyParent;
        private int[] _keyLayout = {0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0};
        private List<Key> _keys;

        [Header("Keyboard scaling")]
        [SerializeField] private Vector3 whiteKeyScale;
        [SerializeField] private Vector3 blackKeyScale;
        [SerializeField] private float keyOffset;

        [Header("Symbols")]
        private Dictionary<KeyAccidentals, char> accidentalSymbols = new Dictionary<KeyAccidentals, char>()
        {
            {KeyAccidentals.None, '\0'},
            {KeyAccidentals.Natural, '\u266E'},
            {KeyAccidentals.Sharp, '\u266F'},
            {KeyAccidentals.Flat, '\u266D'}
        };

        [Header("Customization")]
        [SerializeField] private float volume = 1;
        [SerializeField] private int octaveFactor = 0;
        [SerializeField] private PianoTimbres timbre;
        [SerializeField] private PianoKeyPrefab keyPrefabs;

        /// <summary>
        /// Method to create the piano
        /// </summary>
        public void CreatePiano()
        {
            CreateKeys();
        }

        private void CreateKeys()
        {
            int keyPitch = ((int)startingKeyPitch) - 1;
            int currentKey = GetStartingKeyIndex(startingKeyPitch) - 1;
            
            int pitchLenght = Enum.GetNames(typeof(KeyPitches)).Length;
            _keys = new List<Key>();

            for (int i = 0; i < keyCount; i++)
            {
                // Itterating through the next values
                currentKey++;
                if (currentKey > _keyLayout.Length - 1) { currentKey = 0; }

                int keyType = _keyLayout[currentKey];

                GameObject keyPrefab;
                KeyAccidentals accidental;
                Vector3 pos = Vector3.zero;

                switch (keyType)
                {
                    case 0:
                    default:
                        keyPitch++;
                        if (keyPitch > pitchLenght - 1) { keyPitch = 0; }

                        keyPrefab = keyPrefabs.whiteKey;
                        accidental = KeyAccidentals.None;

                        if (i != 0) { pos.x = GetPreviousWhiteKey(i).transform.position.x + whiteKeyScale.x + keyOffset; }
                        break;
                    case 1:
                        keyPrefab = keyPrefabs.blackKey;
                        accidental = KeyAccidentals.Sharp;

                        if (i != 0) { pos.x = _keys[i - 1].transform.position.x + whiteKeyScale.x / 2; }
                        pos.z = pos.z + (whiteKeyScale.z - blackKeyScale.z) / 2;
                        break;
                }

                GameObject keyObj = Instantiate(keyPrefab, pos, Quaternion.identity, keyParent);
                Key key = keyObj.GetComponent<Key>();
                key.Setup((KeyPitches)keyPitch, accidental, 0);
                _keys.Add(key);

                keyObj.name = "Key_" + ((KeyPitches)keyPitch).ToString() + accidentalSymbols[key.Accidental];
                Debug.Log("Creating key: " + i + " | Type: " + (keyType == 0 ? "white" : "black") + " | Pitch: " + ((KeyPitches)keyPitch).ToString() + "(" + accidental.ToString()+ ")");
            }
        }

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

        private int GetStartingKeyIndex(KeyPitches pitch)
        {
            int keyIndex = -1;
            Debug.Log("Searching for white key: " + (int)pitch);
            for (int i = 0; i < _keyLayout.Length; i++)
            {
                if (_keyLayout[i] == 0)
                {
                    keyIndex++;
                    if (keyIndex == (int)pitch) 
                    { 
                        Debug.Log("Found the " + (int)pitch + "th white key with index: " + i);
                        return i; 
                    }
                }
            }

            Debug.LogError("Unable to find a white key that matches the assigned pitch: " + pitch.ToString()) ;
            return 0;
        }
    }
}
