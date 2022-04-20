using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonsExtraScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, ISelectHandler
{
    //TMPro_EventManager
    private Text text;

    public void Awake()
    {
         text = GetComponentInChildren<Text>();
    }
    // When highlighted 
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyle.Bold;
    }

    //When not highlighted from cursor
    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyle.Normal;
    }

    /*// When selected.
    public void OnSelect(BaseEventData eventData)
    {
        // Do something.
        Debug.Log("<color=red>Event:</color> Completed selection.");
    }*/
}
