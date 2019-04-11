using UnityEngine;
using ZXing;//引入库.
using ZXing.QrCode;

/// <summary>
/// 二维码创建脚本.
/// </summary>
public class BarcodeCam : MonoBehaviour
{
    [HideInInspector]
    public Texture2D m_ErWeuMaImg = null;
    //定义方法生成二维码
    private Color32[] Encode(string textForEncoding, int width, int height)
    {
        //var writer = new BarcodeWriter
        //{
        //    Format = BarcodeFormat.QR_CODE,
        //    Options = new QrCodeEncodingOptions
        //    {
        //        Height = height,
        //        Width = width
        //    }
        //};
        //return writer.Write(textForEncoding);
        return null;
    }

    /// <summary>
    /// 获取二维码图片.
    /// </summary>
    public Texture2D CreateErWeiMaImg(string url)
    {
        Debug.Log("Unity: CreateErWeiMaImg -> url == " + url);
        Texture2D encoded = new Texture2D(256, 256);
        var textForEncoding = url;
        if (textForEncoding != null)
        {
            //二维码写入图片
            var color32 = Encode(textForEncoding, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
        }
        return encoded;
    }
}