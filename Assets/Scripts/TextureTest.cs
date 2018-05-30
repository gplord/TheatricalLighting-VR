using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTest : MonoBehaviour {

    public Texture2D texT;
    public Texture2D texR;
    public Texture2D texB;
    public Texture2D texL;

    public Light spotlight;

    public Texture2D combinedTexture;

    //// Use this for initialization
    //void Start() {
    //    combinedTexture = new Texture2D(128, 12, TextureFormat.ARGB32, false);
    //    //CombineTextures(combinedTexture, texT, texR, texB, texL);

    //    combinedTexture = texT.AlphaBlend(texR);
    //    combinedTexture = combinedTexture.AlphaBlend(texB);
    //    combinedTexture = combinedTexture.AlphaBlend(texL);

    //    combinedTexture.alphaIsTransparency = true;
    //    combinedTexture.wrapMode = TextureWrapMode.Clamp;

    //    spotlight.cookie = combinedTexture;
    //}

    //void CombineTextures(Texture2D output, Texture2D t, Texture2D r, Texture2D b, Texture2D l)
    //{
    //    Vector2 offset = new Vector2(output.width / 2, output.width / 2);

    //    output.SetPixels(t.GetPixels());

    //}
    //// Update is called once per frame
    //void Update()
    //{

    //}
}

//public static class ImageHelpers {
//        public static Texture2D AlphaBlend(this Texture2D aBottom, Texture2D aTop) {
//        if (aBottom.width != aTop.width || aBottom.height != aTop.height)
//            throw new System.InvalidOperationException("AlphaBlend only works with two equal sized images");
//        var bData = aBottom.GetPixels();
//        var tData = aTop.GetPixels();
//        int count = bData.Length;
//        var rData = new Color[count];
//        for (int i = 0; i < count; i++)
//        {
//            Color B = bData[i];
//            Color T = tData[i];
//            float srcF = T.a;
//            float destF = 1f - T.a;
//            float alpha = srcF + destF * B.a;
//            Color R = (T * srcF + B * B.a * destF) / alpha;
//            R.a = alpha;
//            rData[i] = R;
//        }
//        var res = new Texture2D(aTop.width, aTop.height);
//        res.SetPixels(rData);
//        res.Apply();
//        return res;
//    }

//}
