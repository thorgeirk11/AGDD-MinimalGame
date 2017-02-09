using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
    public static class Utils
    {
        public static IEnumerator Wait(float sec, Action action)
        {
            yield return new WaitForSeconds(sec);
            action();
        }
        public static void Wait(this UnityEngine.MonoBehaviour obj, Func<bool> wait, Action action)
        {
            obj.StartCoroutine(WaitUntil(wait, action));
        }
        private static System.Collections.IEnumerator WaitUntil(Func<bool> wait, Action action)
        {
            yield return new WaitUntil(wait);
            action();
        }
    }
}
