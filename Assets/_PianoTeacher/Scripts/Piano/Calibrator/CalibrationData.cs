using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTeacher.Piano
{
    /// <summary>
    /// Data which can be filled by the calibrator
    /// </summary>
    public class CalibrationData
    {
        public Vector3 startPos;
        public double angle;
        public float whiteKeyScaleX;
        public float blackKeyScaleX;

        public CalibrationData(Vector3 startPos, double angle, float whiteKeyScaleX, float blackKeyScaleX)
        {
            this.startPos = startPos;
            this.angle = angle;
            this.whiteKeyScaleX = whiteKeyScaleX;
            this.blackKeyScaleX = blackKeyScaleX;
        }
    }
}