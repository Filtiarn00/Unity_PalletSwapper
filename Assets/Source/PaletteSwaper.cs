using UnityEngine;

[ExecuteInEditMode]
public class PaletteSwaper : MonoBehaviour
{
    public Color[] colorPallet;
    private Material material;
    private SpriteRenderer spriteRenderer;
    public Shader shader;

    [ExecuteInEditMode]
    private void Start()
    {
        if (shader != null)
            material = new Material(shader);
    }

    //Used For Individual objects
    [ExecuteInEditMode]
    private void Update()
    {
        if (shader == null)
            return;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.material = material;
            material.SetColorArray("_Out", colorPallet);
        }
    }

    //Used On Camera object to effect all objects
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (shader == null)
            return;
            
        material.SetColorArray("_Out", colorPallet);
        Graphics.Blit(source, destination, material);
    }
}