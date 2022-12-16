using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InstructionClick : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] GameObject instruction;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        instruction.SetActive(!instruction.activeSelf);
    }


}
