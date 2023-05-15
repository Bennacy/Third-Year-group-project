using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class uiAudio : MonoBehaviour
{
    public EventTrigger eventTrigger;
    
    void Start()
    {
        eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(delegate{AudioManager.Instance.PlayUINavigation();});
        eventTrigger.triggers.Add(entry);

        entry.callback.RemoveAllListeners();
        // entry.eventID = EventTriggerType.cli;
        // entry.callback.AddListener(delegate{AudioManager.Instance.PlayUINavigation();});
        // eventTrigger.triggers.Add(entry);
    }
}
