using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DraggableInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    InputField inputField;
    LightFixture lightFixture;

    float dragStartX;
    float startValue;
    public float dragScale = 0.25f;

    void Awake()
    {
        inputField = GetComponent<InputField>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lightFixture = transform.parent.GetComponent<PanelSelectedLight>().lightFixture;
        if (float.TryParse(inputField.text, out startValue)) {
            startValue = float.Parse(inputField.text);
        } else
        {
            startValue = 0;
        }
        dragStartX = Input.mousePosition.x;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift)) {
            dragStartX = Input.mousePosition.x;
            startValue = float.Parse(inputField.text);
        }

        float tempDragScale;
        if (Input.GetKey(KeyCode.LeftShift)) {
            tempDragScale = dragScale / 3f;
        } else {
            tempDragScale = dragScale;
        }
        
        float dragValue = startValue - ((Input.mousePosition.x - dragStartX) * tempDragScale);
        inputField.text = dragValue.ToString();
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //lightFixture.captureData.UpdatePosition(lightFixture.transform.position);
    }

}
