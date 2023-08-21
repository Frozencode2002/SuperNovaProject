using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanBody
{
    public static class TransformExtensions
    {
        public static Transform FindDeepChild(this Transform parent, string name)
        {
            Transform result = parent.Find(name);
            if (result != null)
            {
                return result;
            }

            foreach (Transform child in parent)
            {
                result = child.FindDeepChild(name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
