using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Helper : MonoBehaviour
{
    public static Helper Instance;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static IEnumerator WaitDelayAsync(float delay, UnityAction executeAction)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        executeAction();
    }

    public static void WaitDelay(float delay, UnityAction action)
    {
        RunCoroutine(WaitDelayAsync(delay, action));
    }

    public static void RunCoroutine(IEnumerator function)
    {
        Instance.StartCoroutine(function);
    }
}
