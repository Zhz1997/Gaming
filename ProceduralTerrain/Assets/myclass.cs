// Demonstration of RuntimeInitializeOnLoadMethod and the argument it can take.
using UnityEngine;

class MyClass
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Debug.Log("Before first Scene loaded");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoadRuntimeMethod()
    {
        Debug.Log("After first Scene loaded");
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("RuntimeMethodLoad: After first Scene loaded");
    }
}