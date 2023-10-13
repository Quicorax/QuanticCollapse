using System.Threading.Tasks;
using UnityEngine;

namespace QuanticCollapse
{
    public static class ExtensionMethods
    {
        public static Vector2Int[] GetCrossCoords(this Vector2Int originVector)
        {
            return new[]
            {
                originVector + Vector2Int.left,
                originVector + Vector2Int.up,
                originVector + Vector2Int.right,
                originVector + Vector2Int.down,
            };
        }

        public static Vector2Int[] GetSplashCoords(this Vector2Int originVector)
        {
            return new[]
            {
                originVector + Vector2Int.left,
                originVector + Vector2Int.left * 2,
                originVector + Vector2Int.up,
                originVector + Vector2Int.up * 2,
                originVector + Vector2Int.up * 3,
                originVector + Vector2Int.right,
                originVector + Vector2Int.right * 2,
                originVector + Vector2Int.down,
                originVector + Vector2Int.down * 2,
                originVector + Vector2Int.up + Vector2Int.left,
                originVector + Vector2Int.up + Vector2Int.right,
                originVector + Vector2Int.down + Vector2Int.left,
                originVector + Vector2Int.down + Vector2Int.right,
            };
        }

        public static void ManageTaskException(this Task task) =>
            task.ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
    }
}