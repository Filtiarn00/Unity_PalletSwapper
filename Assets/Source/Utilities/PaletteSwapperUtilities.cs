using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Rendering;

public class PaletteSwapperUtilities
{
    private static Dictionary<string, Material> materials = new Dictionary<string, Material>();
    private static MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
    private static string shaderName = "Custome/PaletteSwap";

    public static Material GetMaterial(PaletteSwapper palletSwapper)
    {
        string key = palletSwapper.sprite.name + palletSwapper.colorPaletteSet.name;

        if (!materials.ContainsKey(key))
        {
            Material material = new Material(Shader.Find(shaderName));
            material.SetTexture("_MainTex", palletSwapper.sprite.texture);
            material.SetColorArray("_Out", palletSwapper.colorPaletteSet.colors);
            material.enableInstancing = true;
            materials.Add(key, material);
        }
        return materials[key];
    }

    public static void UpdatePalette(PaletteSwapper palletSwapper, SpriteRenderer spriteRenderer ,int index)
    {
        materialPropertyBlock.SetInt("_Offset",palletSwapper.colorPaletteSet.colorPalletSetSize * index);      
        spriteRenderer.sprite = palletSwapper.sprite;
        spriteRenderer.material = GetMaterial(palletSwapper);
        spriteRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}