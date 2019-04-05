using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PaletteSwapperDirty : IComponentData
{
    public int index;
}

public class PaletteSwapperDirtyComponent : ComponentDataProxy<PaletteSwapperDirty> { } 
