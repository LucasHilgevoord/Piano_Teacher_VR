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
        public double angle;
        public float whiteKeyScaleX;
        public float blackKeyScaleX;

        public CalibrationData(double angle, float whiteKeyScaleX, float blackKeyScaleX)
        {
            this.angle = angle;
            this.whiteKeyScaleX = whiteKeyScaleX;
            this.blackKeyScaleX = blackKeyScaleX;
        }
    }
}