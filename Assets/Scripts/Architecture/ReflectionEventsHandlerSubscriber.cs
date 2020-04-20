using UnityEngine;

public class ReflectionEventsHandlerSubscriber : MonoBehaviour
{
    protected virtual void Awake()
    {
        ReflectionEventsHandler.onUpdate += OnUpdate;
        ReflectionEventsHandler.onFixedUpdate += OnFixedUpdate;
        ReflectionEventsHandler.onLateUpdate += OnLateUpdate;
    }

    protected virtual void OnUpdate()
    {
        //base class does nothing
    }

    protected virtual void OnLateUpdate()
    {
        //base class does nothing
    }

    protected virtual void OnFixedUpdate()
    {
        //base class does nothing
    }


    private void OnDestroy()
    {
        //Unsubscribe events on Destroy

        ReflectionEventsHandler.onUpdate -= OnUpdate;
        ReflectionEventsHandler.onFixedUpdate -= OnFixedUpdate;
        ReflectionEventsHandler.onLateUpdate -= OnLateUpdate;
    }
}
