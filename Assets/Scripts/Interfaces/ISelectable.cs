using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ISelectable : MonoBehaviour
{
    public GameObject firstSelected;
    public EventSystem eventSystem;
    
    void Awake()
    {
        eventSystem = EventSystem.current;

    }

    void OnEnable()
    {
        SetSelected();
    }

    public void SetSelected(){
        eventSystem.SetSelectedGameObject(firstSelected);
    }
}
