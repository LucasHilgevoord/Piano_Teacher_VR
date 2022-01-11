using MidiPlayerTK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PianoTeacher.Display
{
    public class DisplayNote : MonoBehaviour
    {
        public MPTKEvent data;

        private Image _img;
        private RectTransform _rect;
        public RectTransform Rect => _rect;
        [SerializeField] private Text _noteLabel;

        private bool _isPlayed;
        public bool IsPlayed { get { return _isPlayed; } set { _isPlayed = value; } }

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _img = GetComponent<Image>();
        }

        /// <summary>
        /// Move the note towards a direction
        /// </summary>
        /// <param name="dir"></param>
        internal void MoveNote(Vector3 dir) { transform.localPosition += dir; }

        /// <summary>
        /// 
        /// </summary>
        internal void EnableTriggered() { }

        /// <summary>
        /// 
        /// </summary>
        internal void DisableTriggered() { }

        internal void SetScale(float width, float height) { _rect.sizeDelta = new Vector2(width, height); }

        /// <summary>
        /// Set the position of the display note
        /// </summary>
        /// <param name="x">x pos</param>
        /// <param name="y">y pos</param>
        internal void SetPosition(float x, float y = 0) { _rect.anchoredPosition = new Vector2(x, y); }

        /// <summary>
        /// Get the height of the display note
        /// </summary>
        /// <returns>height of the display note</returns>
        internal float GetHeight() { return _rect.sizeDelta.y;}

        /// <summary>
        /// Set text of the note label to the corresponding note
        /// </summary>
        internal void SetLabel(string t) { _noteLabel.text = t; }

        internal void SetColor(Color c) { _img.color = c; }

        /// <summary>
        /// Despawn the note to be re-used again
        /// </summary>
        internal void KilNote()
        {
            // TODO: Despawn in pool
            this.gameObject.SetActive(false);
            _isPlayed = false;
        }

    }
}
