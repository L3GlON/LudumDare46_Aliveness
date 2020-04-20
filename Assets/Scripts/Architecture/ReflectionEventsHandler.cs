using UnityEngine;
using System;

public class ReflectionEventsHandler : MonoBehaviour
{
    public static Action onUpdate;
    public static Action onFixedUpdate;
    public static Action onLateUpdate;


    void Update()
    {
        onUpdate?.Invoke();
    }


    void LateUpdate()
    {
        onLateUpdate?.Invoke();
    }


    void FixedUpdate()
    {
        onFixedUpdate?.Invoke();
    }

}
