using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;

[System.Serializable]
public struct PaletteSwapper: ISharedComponentData
{
    public Sprite sprite;
    public ColorPaletteSet colorPaletteSet;
    private SpriteRenderer spriteRenderer;
}

public class PaletteSwapperComponent : SharedComponentDataProxy<PaletteSwapper> { }