using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class TextureEditorImage
{
    public Texture2D sourceTexture;
    private int width;
    private int height;

    public List<Color> colorPallet = new List<Color>();
    public List<Texture2D> colorPalletTextures = new List<Texture2D>();

    public void LoadSourceTexture(Texture2D texture)
    {
        if (texture == null)
            return;

		sourceTexture = new Texture2D(texture.width,texture.height);
		sourceTexture.SetPixels(texture.GetPixels());
		sourceTexture.filterMode = texture.filterMode;
		sourceTexture.Apply();
        width = sourceTexture.width;
        height = sourceTexture.height;
        LoadColorPallet();
    }

    private void LoadColorPallet()
    {
        colorPallet.Clear();
		colorPalletTextures.Clear();

        Dictionary<Color, bool> d = new Dictionary<Color, bool>();

        foreach (Color color in sourceTexture.GetPixels())
            if (!d.ContainsKey(color) && color.a != 0)
            {
                d.Add(color, false);
                colorPallet.Add(color);
				colorPalletTextures.Add(S(15,15,color));

            }
    }

    private Texture2D S(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
            pix[i] = color;
        
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    public void DrawSourceTexture()
    {
        if (sourceTexture != null)
            GUI.DrawTexture(new Rect(0, 0, sourceTexture.width, sourceTexture.height), sourceTexture);
    }

    public void DrawCanvas()
    {
        if (sourceTexture == null)
            return;

        float ratio = (float)height / (float)width;
        float w = 5 * 30;
        float h = ratio * 5 * 30;

        EditorGUI.DrawRect(new Rect(0, 0, w, h), Color.red);
    }

    public Rect GetRect()
    {
        if (sourceTexture == null)
            return new Rect(0, 0, 0, 0);

        float ratio = (float)height / (float)width;
        float w = 5 * 30;
        float h = ratio * 5 * 30;

        return new Rect(0, 0, w, h);
    }
}