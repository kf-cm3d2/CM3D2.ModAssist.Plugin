using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CM3D2.Util
{
    class GUIUtil
    {
        public static int FixPx(int px)
        {
            return (int)((1.0f + (Screen.width / 1280.0f - 1.0f) * 0.6f) * px);
        }

        public static float DrawModValueSlider(Rect outRect, float value, float min, float max, string label, GUIStyle lstyle)
        {
            float conWidth = outRect.width;

            outRect.width = conWidth * 0.3f;
            GUI.Label(outRect, label, lstyle);
            outRect.x += outRect.width;

            outRect.width = conWidth * 0.7f;
            outRect.y += FixPx(5);

            return GUI.HorizontalSlider(outRect, value, min, max);
        }

        public static string DrawTextField(Rect outRect, string value, string label, GUIStyle lstyle, GUIStyle tstyle)
        {
            Rect rect = new Rect(outRect);
            rect.width = outRect.width * 0.3f;
            GUI.Label(rect, label, lstyle);
            rect.x += rect.width;
            rect.width = outRect.width * 0.7f;
            return GUI.TextField(rect, value);
        }
    }
}
