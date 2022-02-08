using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonsExtraScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, ISelectHandler
{
    //TMPro_EventManager
    [SerializeField] private TextMeshProUGUI text;

    public void Awake()
    {
         text = GetComponentInChildren<TextMeshProUGUI>();
    }
    // When highlighted 
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.textStyle = TMP_Style.NormalStyle;
        text.fontStyle = FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.textStyle = TMP_Style.NormalStyle;
        text.fontStyle = FontStyles.Normal;
    }

    /*// When selected.
    public void OnSelect(BaseEventData eventData)
    {
        // Do something.
        Debug.Log("<color=red>Event:</color> Completed selection.");
    }*/
}
