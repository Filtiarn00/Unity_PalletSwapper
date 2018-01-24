using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PaletteSwaper : MonoBehaviour
{
    private static Dictionary<string,Material> materials;
    private static string shaderName = "Custome/PaletteSwap";

    private Material material;
    public Texture2D texture;
    public ColorPaletteSet colorPaletteSet;
    public int colorPaletteIndex;

    //Used For Individual objects
    private void Update()
    {
        if (colorPaletteSet == null || texture == null)
            return;

        colorPaletteIndex = Mathf.Clamp(colorPaletteIndex,0,colorPaletteSet.colorPallets.Count - 1);
        material =  GetMaterial(texture,colorPaletteSet,colorPaletteIndex);

        var colors = colorPaletteSet.colorPallets[colorPaletteIndex].colors;
        PaletteSwaper.PalletSwapOnMaterial(colors, material, texture);
        PaletteSwaper.PalletSwapOnSpriteRenderer(colors, material, texture,GetComponent<SpriteRenderer>());
    }

    void OnApplicationQuit()
    {
        material = null;
        materials.Clear();
    }

    //Used For Materials
    public static void PalletSwapOnMaterial(Color[] pallet,Material material, Texture2D texture)
    {
        if (pallet == null || pallet.Length == 0 || texture == null)
            return;

        material.SetTexture("_MainTex", texture);
        material.SetColorArray("_Out", pallet);
    }

    //Used For Sprite Renderers
    public static void PalletSwapOnSpriteRenderer(Color[] pallet,Material material, Texture2D texture, SpriteRenderer spriteRenderer)
    {
         if (pallet == null || pallet.Length == 0 || texture == null && spriteRenderer == null)
            return;

        material.SetTexture("_MainTex", texture);
        material.SetColorArray("_Out", pallet);
        spriteRenderer.material = material;
    }

    public static Material GetMaterial(Texture2D texture,ColorPaletteSet colorPaletteSet,int i)
    {
        if (materials == null)
            materials = new Dictionary<string, Material>();

        string key = texture.name + colorPaletteSet.name + i.ToString();
        if (!materials.ContainsKey(key))
        {
            Material material = new Material(Shader.Find(shaderName));
            materials.Add(key,material);
        }
        return materials[key]; 
    }
}