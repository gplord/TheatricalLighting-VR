using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISliderIntensity : MonoBehaviour, IPointerUpHandler {

    PanelSelectedLight panelSelectedLight;

    void Start()
    {
        panelSelectedLight = transform.parent.GetComponent<PanelSelectedLight>();
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        panelSelectedLight.IntensityChangeCommit();
    }

}
