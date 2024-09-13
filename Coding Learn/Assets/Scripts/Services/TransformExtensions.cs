using UnityEngine;

namespace GameLogic
{
    public static class TransformExtensions
    {
        public static void DeleteAllChildren(this Transform transform)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();
        }
    }
}
