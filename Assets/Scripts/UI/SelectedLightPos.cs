using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectedLightPos : MonoBehaviour {

    // TODO: Delete this whole script -- legacy
    /*
    public GameManager gameManager;

	public LightMount lightMount;

	public Text name;

	public Dropdown lightRail;

	public Toggle lightEnabled;
	public Slider lightStrength;
	public Slider lightRange;

	public Slider lightColorR;
	public Slider lightColorG;
	public Slider lightColorB;

	public Slider lightPosition;
	public Slider lightPan;
	public Slider lightTilt;
	public Slider lightAngle;

    public Text lightTargetName;
	public Toggle lightTargetLocked;

	public Button lightTargetSelect;

	public Dropdown lightCookie;
	private int lightCookieIndex;	// Quick workaround, to store the previous dropdown index
			
	void Start () {

		lightRail.onValueChanged.AddListener( delegate { ValueChangeCheck(); });

		lightEnabled.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		lightStrength.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		lightRange.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		lightColorR.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		lightColorG.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		lightColorB.onValueChanged.AddListener (delegate {ValueChangeCheck ();});

		lightPosition.onValueChanged.AddListener (delegate {ValueChangeCheck ();});

		lightPan.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		lightTilt.onValueChanged.AddListener (delegate {ValueChangeCheck ();});

        lightAngle.onValueChanged.AddListener ( delegate { ValueChangeCheck ();});

		lightTargetLocked.onValueChanged.AddListener (delegate {ValueChangeCheck();});

		lightTargetSelect.onClick.AddListener (delegate {TargetSelectCheck();});

		lightCookie.onValueChanged.AddListener ( delegate { ValueChangeCheck(); } );

		// Default starting values
		lightCookieIndex = 0;				// No cookies selected
											// TODO: No target selected
		lightTargetLocked.isOn = false;		// No target locked
		//lightTilt.value = lightMount.tiltValue;

		// Fire a forced update, to enable these default values
		ValueChangeCheck();

	}
	
	void Update () {
	
	}

	public void Setup() {

        //lightTilt.value = lightMount.tiltValue;
        //lightPan.value = lightMount.panValue;

        //float tempTilt = ;
        lightTilt.value = lightMount.transform.rotation.eulerAngles.x;

        float tempPan = lightMount.transform.rotation.eulerAngles.y;
        tempPan = (tempPan > 90) ? tempPan - 180 : tempPan;
        lightPan.value = tempPan;

        Debug.LogWarning("Currently, at setup: " + lightMount.transform.rotation.eulerAngles);

        name.text = lightMount.gameObject.name;
		lightStrength.value = lightMount.light.intensity;
		lightRange.value = lightMount.light.range;
		lightColorR.value = lightMount.light.color.r;
		lightColorG.value = lightMount.light.color.g;
		lightColorB.value = lightMount.light.color.b;

		//Vector2 railBounds = lightMount.rail.GetBounds();
        //lightPosition.minValue = railBounds.x;
        //lightPosition.maxValue = railBounds.y;

        lightPosition.minValue = lightMount.rail.transform.localScale.x / 2 * -1;
        lightPosition.maxValue = lightMount.rail.transform.localScale.x / 2;
        lightPosition.value = lightMount.transform.localPosition.x;

		lightTargetName.text = lightMount.GetTargetName();
		lightTargetLocked.isOn = lightMount.followTarget;

		lightAngle.value = lightMount.light.spotAngle;

		foreach (Texture cookie in gameManager.cookies) {
			lightCookie.options.Add(new Dropdown.OptionData() {text = cookie.name});
		}
		if (lightMount.cookieIndex != 0) {
			lightCookie.value = lightMount.cookieIndex;
			ValueChangeCheck();
		}

	}

	
	public void ValueChangeCheck()
	{
		//if (lightMount.rail != gameManager.rails[lightRail.value]) {
		//	Vector3 tempPos = lightMount.transform.localPosition;
		//	lightMount.transform.SetParent(gameManager.rails[lightRail.value].transform);
		//	lightMount.transform.localPosition = tempPos;
		//	lightMount.rail = gameManager.rails[lightRail.value];
		//}

		lightMount.light.enabled = lightEnabled.isOn;
		lightMount.light.intensity = lightStrength.value;
		lightMount.light.range = lightRange.value;
		lightMount.light.color = new Color(lightColorR.value, lightColorG.value, lightColorB.value);
		lightMount.light.spotAngle = lightAngle.value;

		lightMount.transform.localPosition = new Vector3(lightPosition.value, lightMount.transform.localPosition.y, lightMount.transform.localPosition.z);
        //lightMount.mountRig.eulerAngles = new Vector3(lightMount.mountRig.eulerAngles.x, lightPan.value, lightMount.mountRig.eulerAngles.z);
        //lightMount.lightRig.eulerAngles = new Vector3(lightTilt.value, lightMount.lightRig.eulerAngles.y, lightMount.lightRig.eulerAngles.z);

        Debug.LogWarning(new Vector3(lightTilt.value, lightPan.value, 0));
        lightMount.transform.localRotation = Quaternion.Euler(lightTilt.value, lightPan.value, 0);

		lightMount.followTarget = lightTargetLocked.isOn;

		if (lightCookie.value == 0) {
			lightMount.light.cookie = null;
		} else {
			lightMount.light.cookie = gameManager.cookies[lightCookie.value-1];
			lightMount.cookieIndex = lightCookie.value;
		}

        //lightMount.tiltValue = lightTilt.value;
        //lightMount.panValue = lightPan.value;
        
    }

	public void TargetSelectCheck() {

		//gameManager.selectingTarget = true;

	}
    */
}
