using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class VRManager : MonoBehaviour {

        public CuesManager cuesManager;
        public GameObject setDesign;

        public Player player = null;

        // Use this for initialization
        void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {

            if (player.leftHand != null)
            {
                if (player.leftHand.GetStandardInteractionButtonDown())
                {
                    //Debug.Log("Previous");
                    cuesManager.GoToPrevious();
                }
                if (((player.leftHand.controller != null) && player.leftHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
                {
                    if (setDesign != null) setDesign.SetActive(!setDesign.activeInHierarchy);
                }
            }
            if (player.rightHand != null)
            {
                if (player.rightHand.GetStandardInteractionButtonDown())
                {
                    //Debug.Log("Next");
                    cuesManager.GoToNext();
                }
                if (((player.rightHand.controller != null) && player.rightHand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
                {
                    if (setDesign != null) setDesign.SetActive(!setDesign.activeInHierarchy);
                }
            }

        }
    }
}
