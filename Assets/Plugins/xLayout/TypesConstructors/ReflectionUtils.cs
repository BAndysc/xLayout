using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace xLayout.TypesConstructors
{
    internal static class ReflectionUtils
    {
        public static void ReflectionSetComponentField(IReadOnlyLayoutContext context, string fieldName, Type fieldType, Action<object> setter, Component component,
            string value)
        {
            if (fieldType == typeof(string))
                setter(context.ParseString(value));
            else if (fieldType == typeof(int))
                setter(context.ParseInt(value));
            else if (fieldType == typeof(float))
                setter(context.ParseFloat(value));
            else if (fieldType == typeof(Vector2))
                setter(context.ParseVector2(value));
            else if (fieldType == typeof(Vector3))
                setter(context.ParseVector3(value));
            else if (fieldType == typeof(Vector4))
                setter(context.ParseVector4(value));
            else if (fieldType == typeof(Color))
                setter(context.ParseColor(value));
            else if (fieldType.IsEnum)
            {
                var reValue = context.ParseString(value);
                if (reValue.Length == 0)
                    return;

                if (char.IsDigit(reValue[0]))
                {
                    if (!int.TryParse(reValue, out var integer))
                        Debug.LogError(
                            $"Trying to set field {fieldName} of enum type {fieldType}, but `{reValue}` is not a number");
                    else
                        setter(Enum.ToObject(fieldType, integer));
                }
                else
                {
                    setter(Enum.Parse(fieldType, reValue));
                }
            }
            else if (fieldType.IsSubclassOf(typeof(Object)))
            {
                var asset = context.GetAsset<Object>(value);
                setter(asset);
            }
            else
                Debug.LogError($"Don't know how to set value of field type {fieldType} in {component}/{fieldName}");
        }
    }
}