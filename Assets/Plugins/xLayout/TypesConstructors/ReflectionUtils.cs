using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace xLayout.TypesConstructors
{
    internal static class ReflectionUtils
    {
        public static void ReflectionSetComponentField(IReadOnlyLayoutContext context, FieldInfo field, Component component,
            string value)
        {
            if (field.FieldType == typeof(string))
                field.SetValue(component, context.ParseString(value));
            else if (field.FieldType == typeof(int))
                field.SetValue(component, context.ParseInt(value));
            else if (field.FieldType == typeof(float))
                field.SetValue(component, context.ParseFloat(value));
            else if (field.FieldType == typeof(Vector2))
                field.SetValue(component, context.ParseVector2(value));
            else if (field.FieldType == typeof(Vector3))
                field.SetValue(component, context.ParseVector3(value));
            else if (field.FieldType == typeof(Vector4))
                field.SetValue(component, context.ParseVector4(value));
            else if (field.FieldType == typeof(Color))
                field.SetValue(component, context.ParseColor(value));
            else if (field.FieldType.IsEnum)
            {
                var reValue = context.ParseString(value);
                if (reValue.Length == 0)
                    return;

                if (char.IsDigit(reValue[0]))
                {
                    if (!int.TryParse(reValue, out var integer))
                        Debug.LogError(
                            $"Trying to set field {field.Name} of enum type {field.FieldType}, but `{reValue}` is not a number");
                    else
                        field.SetValue(component, Enum.ToObject(field.FieldType, integer));
                }
                else
                {
                    field.SetValue(component, Enum.Parse(field.FieldType, reValue));
                }
            }
            else if (field.FieldType.IsSubclassOf(typeof(Object)))
            {
                var asset = context.GetAsset<Object>(value);
                field.SetValue(component, asset);
            }
            else
                Debug.LogError($"Don't know how to set value of field type {field.FieldType} in {component}/{field.Name}");
        }
    }
}