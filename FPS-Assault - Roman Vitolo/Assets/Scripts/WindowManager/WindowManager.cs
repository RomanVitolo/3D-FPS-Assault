using UnityEditor;
using UnityEngine;

namespace WindowManager
{
    public class WindowManager : MonoBehaviour
    {
        [MenuItem("My Methods/New Method")]
        public static void AssignNewMethod()
        {
            Debug.Log("A new Method was created");
        }
    }
}