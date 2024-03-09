using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localPosition = new Vector3(0,-602,0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localPosition = new Vector3(0,-694,0);
    }
}
