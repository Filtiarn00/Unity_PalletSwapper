using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Color Palette/Color Palette Set")]
public class ColorPaletteSet : ScriptableObject
{
    public List<ColorPalette> colorPallets;
}

[System.Serializable]
public class ColorPalette
{
	public string id;
	public Color[] colors;
}