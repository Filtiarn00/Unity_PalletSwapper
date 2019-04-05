using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine;

public class PaletteSwapperSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentArray<SpriteRenderer> SpriteRenderer;
        public ComponentDataArray<PaletteSwapperDirty> PaletteSwapperDirty;
        [ReadOnly] public SharedComponentDataArray<PaletteSwapper> PalletSwapper;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        for (var i = 0; i < data.Length; i++)
        {
            var entity = data.Entity[i];
            var spriteRenderer = data.SpriteRenderer[i];
            var palletSwapperDirty = data.PaletteSwapperDirty[i];
            var palletSwapper = data.PalletSwapper[i];

            PaletteSwapperUtilities.UpdatePalette(palletSwapper, spriteRenderer, palletSwapperDirty.index);
            PostUpdateCommands.RemoveComponent(entity,typeof(PaletteSwapperDirty));
        }
    }
}