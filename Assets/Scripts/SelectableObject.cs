using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {

    [SerializeField] // TODO: Serialized for debug display in inspector, can be removed
    protected bool highlighted = false;
    [SerializeField]
    public bool highlightedLinked = false;
    [SerializeField]
    protected bool increasedVisibility = false;

    protected Color highlightedColor = new Color(0, 0.35f, 0.75f);
    protected Color highlightedLinkedColor = new Color(0, 0.75f, 0.35f);
    protected Color increasedVisibilityColor = new Color(0.25f, 0.25f, 0.25f);

    protected List<Material> defaultEmissiveMaterials;
    protected Color currentColor = Color.white;
    
    public virtual void Awake () {

        defaultEmissiveMaterials = new List<Material>();

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.IsKeywordEnabled("_EMISSION")) {
                    defaultEmissiveMaterials.Add(mat);
                }
            }
        }
	}

	void Update () {
		
	}

    public virtual void IncreaseVisibility(bool onOff)
    {
        if (onOff)
        {
            if (!highlighted)
            {
                if (!highlightedLinked)
                {
                    Renderer[] renderers = GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer in renderers)
                    {
                        foreach (Material mat in renderer.materials)
                        {
                            if (!defaultEmissiveMaterials.Contains(mat))
                            {
                                mat.EnableKeyword("_EMISSION");
                                mat.SetColor("_EmissionColor", increasedVisibilityColor);
                            }
                        }
                    }
                }
            }
            increasedVisibility = true;
        }
        else
        {
            if (!highlighted)
            {
                if (!highlightedLinked)
                {
                    Renderer[] renderers = GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer in renderers)
                    {
                        foreach (Material mat in renderer.materials)
                        {
                            if (defaultEmissiveMaterials.Contains(mat))
                            {
                                mat.SetColor("_EmissionColor", currentColor);
                            }
                            else
                            {
                                mat.DisableKeyword("_EMISSION");
                            }
                        }
                    }
                }
            }
            increasedVisibility = false;
        }
    }

    public virtual void Highlight (bool onOff) {

        if (onOff) {

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                foreach (Material mat in renderer.materials)
                {
                    if (!defaultEmissiveMaterials.Contains(mat))
                    {
                        mat.EnableKeyword("_EMISSION");
                        mat.SetColor("_EmissionColor", highlightedColor);
                    }
                }
            }
            highlighted = true;

        } else {

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                foreach (Material mat in renderer.materials) {
                    if (increasedVisibility)
                    {
                        mat.EnableKeyword("_EMISSION");
                        mat.SetColor("_EmissionColor", increasedVisibilityColor);
                    } else
                    {
                        if (defaultEmissiveMaterials.Contains(mat))
                        {
                            mat.SetColor("_EmissionColor", currentColor);
                        }
                        else
                        {
                            mat.DisableKeyword("_EMISSION");
                        }
                    }
                }
            }
            highlighted = false;

        }

    }

    public virtual void HighlightLinked (bool onOff)
    {
        if (onOff)
        {
            if (!highlighted)
            {

                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    foreach (Material mat in renderer.materials)
                    {
                        if (!defaultEmissiveMaterials.Contains(mat))
                        {
                            mat.EnableKeyword("_EMISSION");
                            mat.SetColor("_EmissionColor", highlightedLinkedColor);
                        }
                    }
                }
                highlightedLinked = true;

            }

        }
        else
        {

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (increasedVisibility)
                    {
                        mat.EnableKeyword("_EMISSION");
                        mat.SetColor("_EmissionColor", increasedVisibilityColor);
                    }
                    else
                    {
                        if (defaultEmissiveMaterials.Contains(mat))
                        {
                            mat.SetColor("_EmissionColor", currentColor);
                        }
                        else
                        {
                            mat.DisableKeyword("_EMISSION");
                        }
                    }
                }
            }
            highlightedLinked = false;

        }
    }

    public virtual void SetEmissionColor(float newIntensity, Color newColor)
    {
        if (defaultEmissiveMaterials.Count > 0)
        {
            if (newIntensity > 0.05f)
            {
                foreach (Material mat in defaultEmissiveMaterials)
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", newColor);
                    currentColor = newColor;
                }
            }
            else
            {
                foreach (Material mat in defaultEmissiveMaterials)
                {
                    mat.SetColor("_EmissionColor", Color.black);
                    currentColor = Color.black;
                }
            }
        }
    }

}
