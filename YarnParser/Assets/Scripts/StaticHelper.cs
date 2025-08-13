using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHelper
{
    public static void ExecuteCoroutine(IEnumerator coroutine)
    {
        CoroutineRunner.Instance.StartCoroutine(coroutine);
    }

    public static IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Waited " + waitTime + " seconds");
    }
}