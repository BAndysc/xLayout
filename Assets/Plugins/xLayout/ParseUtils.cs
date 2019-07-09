using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace xLayout
{
    internal static class ParseUtils
    {
        internal static Vector4 ParsePadding(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return Vector4.zero;
            
            var split = txt.Split(',');

            if (split.Length == 1)
                return Vector4.one * float.Parse(txt);
            
            if (split.Length != 4)
                throw new Exception($"Error while parsing: {txt}, expected 4 floats");
            
            var floats = split.Select(f => float.Parse(f)).ToArray();
            return new Vector4(floats[0], floats[1], floats[2], floats[3]);
        }

        internal static Color ParseColor(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return Color.white;
            
            txt = txt.Substring(1, txt.Length - 2);
            
            var split = txt.Split(',');
            
            var floats = split.Select(f => float.Parse(f)).ToArray();
            
            if (split.Length == 3)
                return new Color(floats[0], floats[1], floats[2]);
            
            if (split.Length == 4)
                return new Color(floats[0], floats[1], floats[2], floats[3]);
            
            throw new Exception($"Invalid format of color: {txt}");
        }

        internal static Vector2 ParseVector2(string txt)
        {
            txt = txt.Substring(1, txt.Length - 2);
            var split = txt.Split(',');
            
            if (split.Length != 2)
                throw new Exception($"Error while parsing: {txt}, expected 2 floats");
            
            return new Vector2(float.Parse(split[0]), float.Parse(split[1]));
        }

        public static Vector3 ParseVector3(string txt)
        {
            txt = txt.Substring(1, txt.Length - 2);
            var split = txt.Split(',');
            
            if (split.Length != 3)
                throw new Exception($"Error while parsing: {txt}, expected 3 floats");
            
            var floats = split.Select(f => float.Parse(f)).ToArray();
            
            return new Vector3(floats[0], floats[1], floats[2]);
        }
        
        public static Vector4 ParseVector4(string txt)
        {
            txt = txt.Substring(1, txt.Length - 2);
            var split = txt.Split(',');
            
            if (split.Length != 4)
                throw new Exception($"Error while parsing: {txt}, expected 4 floats");
            
            var floats = split.Select(f => float.Parse(f)).ToArray();
            
            return new Vector4(floats[0], floats[1], floats[2], floats[3]);
        }
        
        internal static TextAlignmentOptions? ParseTextMeshProAlignment(string horizontal, string vertical)
        {
            var anchor = ParseAlignment(horizontal, vertical);

            if (!anchor.HasValue)
                return null;

            switch (anchor.Value)
            {
                case TextAnchor.UpperLeft:
                    return TextAlignmentOptions.TopLeft;
                case TextAnchor.UpperCenter:
                    return TextAlignmentOptions.Top;
                case TextAnchor.UpperRight:
                    return TextAlignmentOptions.TopRight;
                case TextAnchor.MiddleLeft:
                    return TextAlignmentOptions.Left;
                case TextAnchor.MiddleCenter:
                    return TextAlignmentOptions.Center;
                case TextAnchor.MiddleRight:
                    return TextAlignmentOptions.Right;
                case TextAnchor.LowerLeft:
                    return TextAlignmentOptions.BottomLeft;
                case TextAnchor.LowerCenter:
                    return TextAlignmentOptions.BottomLeft;
                case TextAnchor.LowerRight:
                    return TextAlignmentOptions.BottomRight;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        internal static TextAnchor? ParseAlignment(string horizontal, string vertical)
        {
            switch (horizontal)
            {
                case "left" when vertical == "top":
                    return TextAnchor.UpperLeft;
                case "left" when vertical == "middle":
                    return TextAnchor.MiddleLeft;
                case "left" when vertical == "bottom":
                    return TextAnchor.LowerLeft;
                case "left":
                    Debug.LogError("Invalid VertAlign value: " + vertical + " (use: Top, Middle or Bottom)");
                    break;
                case "middle" when vertical == "top":
                    return TextAnchor.UpperCenter;
                case "middle" when vertical == "middle":
                    return TextAnchor.MiddleCenter;
                case "middle" when vertical == "bottom":
                    return TextAnchor.LowerCenter;
                case "middle":
                    Debug.LogError("Invalid VertAlign value: " + vertical + " (use: Top, Middle or Bottom)");
                    break;
                case "right" when vertical == "top":
                    return TextAnchor.UpperRight;
                case "right" when vertical == "middle":
                    return TextAnchor.MiddleRight;
                case "right" when vertical == "bottom":
                    return TextAnchor.LowerRight;
                case "right":
                    Debug.LogError("Invalid VertAlign value: " + vertical + " (use: Top, Middle or Bottom)");
                    break;
                default:
                    Debug.LogError("Invalid Align value: " + horizontal + " (use: Left, Middle or Right)");
                    break;
            }

            return null;
        }
    }
}