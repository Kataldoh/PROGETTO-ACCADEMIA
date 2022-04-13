using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonsExtraScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, ISelectHandler
{
    //TMPro_EventManager
    //[SerializeField] private TextMeshProUGUI text;
    public Text text2;
    public FontStyle font;

    public void Awake()
    {
        text2 = GetComponentInChildren<Text>();
        //font = GetComponent<Text>();
    }
    // When highlighted 
    public void OnPointerEnter(PointerEventData eventData)
    {
        //text2.textStyle = TMP_Style.NormalStyle;

        text2.fontStyle = FontStyle.Bold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        //text.textStyle = TMP_Style.NormalStyle;
        //text.fontStyle = FontStyles.Normal;

        text2.fontStyle = FontStyle.Normal;

    }

    /*// When selected.
    public void OnSelect(BaseEventData eventData)
    {
        // Do something.
        Debug.Log("<color=red>Event:</color> Completed selection.");
    }*/
}
