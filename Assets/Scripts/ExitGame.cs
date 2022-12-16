using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitGame : MonoBehaviour, IPointerClickHandler
{
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }
}
