using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureData : MonoBehaviour {

    public int id;
    public string fixture;
    public string optics;
    public string wattage;
    public string unit;
    public string circuit;
    public int channel;
    public string patch;
    public string dmxmode;
    public string dmxchannels;
    public string layer;
    public string focus;
    public string filters;
    public string gobos;
    public string accessories;
    public string note;
    public string weight;
    public float posX;
    public float posY;
    public float posZ;
    public float focusPan;
    public float focusTilt;

    public float colorR;
    public float colorG;
    public float colorB;

    public ImportLight importLight;

    public void UpdateImportLight()
    {
        UpdateImportLight(id);
    }
    public void UpdateImportLight(int newId)
    {
        importLight.id = newId;
        importLight.fixture = fixture;
        importLight.optics = optics;
        importLight.wattage = wattage;
        importLight.unit = unit;
        importLight.circuit = circuit;
        importLight.channel = channel;
        importLight.patch = patch;
        importLight.dmxmode = dmxmode;
        importLight.dmxchannels = dmxchannels;
        importLight.layer = layer;
        importLight.focus = focus;
        importLight.filters = filters;
        importLight.gobos = gobos;
        importLight.accessories = accessories;
        importLight.note = note;
        importLight.weight = weight;
        importLight.posX = posX;
        importLight.posY = posY;
        importLight.posZ = posZ;
        importLight.focusPan = focusPan;
        importLight.focusTilt = focusTilt;
        importLight.colorR = colorR;
        importLight.colorG = colorG;
        importLight.colorB = colorB;
    }

    public void UpdatePosition(Vector3 position)
    {
        posX = position.x;
        posY = position.y;
        posZ = position.z;
    }

}
