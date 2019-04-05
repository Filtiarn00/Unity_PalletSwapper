using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Color Palette/Color Palette Set")]
public class ColorPaletteSet : ScriptableObject
{
	public int colorPalletSetSize;
	public Color[] colors;
}

[System.Serializable]
public class ColorPalette
{
	public string id;
}