using UnityEngine;

namespace xLayout
{
    public static class Extensions
    {
        public static T EnsureComponent<T>(this GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp == null)
                return go.AddComponent<T>();
            return comp;
        }
    }
}