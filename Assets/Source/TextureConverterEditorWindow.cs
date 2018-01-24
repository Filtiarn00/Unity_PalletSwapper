using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

class TextureConverterEditorWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private Texture2D inputTexture;
    private Texture2D outputTexture;
    public List<ColorObject> colorObjects;
    private bool showInputArea;
    private bool showOutputArea;
    private bool showColorArea;
    private string saveLocation;
    private string fileName;

    public class ColorObject
    {
        public string name;
        public Color inColor;
        public int index;
    }

    [MenuItem("Window/Color Palette/Texture Converter")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(TextureConverterEditorWindow));
        window.titleContent = new GUIContent("Texture Converter");
    }

    void OnGUI()
    {
        if (colorObjects == null)
            colorObjects = new List<ColorObject>();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        Input();
        Output();
        ColorArea();
        EditorGUILayout.EndScrollView();
    }

    private bool AreaTitle(ref bool toggle, string title)
    {
        EditorGUILayout.BeginVertical("Box");
        toggle = EditorGUILayout.BeginToggleGroup(title, toggle);
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndVertical();
        return toggle;
    }

    private void Input()
    {
        if (!AreaTitle(ref showInputArea, "Input"))
            return;

        inputTexture = (Texture2D)EditorGUILayout.ObjectField("Input", inputTexture, typeof(Texture2D), false);
    }

    private void Output()
    {
        if (!AreaTitle(ref showOutputArea, "Output"))
            return;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Update")) UpdateOutputTexture();
        if (GUILayout.Button("Save")) Save();
        GUILayout.EndHorizontal();

        if (outputTexture != null)
        {
            GUI.enabled = false;
            outputTexture = (Texture2D)EditorGUILayout.ObjectField("Output", outputTexture, typeof(Texture2D), false);
            GUI.enabled = true;
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.TextField("Save Location", saveLocation);
        if (GUILayout.Button("..."))
            saveLocation = EditorUtility.OpenFolderPanel("", "", "");
        GUILayout.EndHorizontal();

        fileName = EditorGUILayout.TextField("File Name", fileName);
    }

    private void ColorArea()
    {
        if (!AreaTitle(ref showColorArea, "Color"))
            return;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Get Colors")) GetColorsFromTextures();
        if (GUILayout.Button("+")) colorObjects.Add(new ColorObject());
        EditorGUILayout.EndHorizontal();

        for(int i =  colorObjects.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            colorObjects[i].name = EditorGUILayout.TextField(colorObjects[i].name);
            colorObjects[i].index = EditorGUILayout.IntField(colorObjects[i].index);
            colorObjects[i].inColor = EditorGUILayout.ColorField(colorObjects[i].inColor);
            if (GUILayout.Button("X")) colorObjects.RemoveAt(i);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void UpdateOutputTexture()
    {
        outputTexture = new Texture2D(inputTexture.width, inputTexture.height);
        outputTexture.SetPixels(inputTexture.GetPixels());
        outputTexture.filterMode = inputTexture.filterMode;
        outputTexture.alphaIsTransparency = true;
        outputTexture.Apply();

        Dictionary<Color, Color> colorsBeingUsed = new Dictionary<Color, Color>();

        foreach (ColorObject i in colorObjects)
            if (!colorsBeingUsed.ContainsKey(i.inColor))
                colorsBeingUsed.Add(i.inColor, i.inColor);

        var colorsBeingUsedArray = colorsBeingUsed.Keys.ToList();

        for (int x = 0; x < outputTexture.width; x++)
            for (int y = 0; y < outputTexture.height; y++)
            {
                if (colorsBeingUsed.ContainsKey(outputTexture.GetPixel(x,y)))
                {
                    int i = colorsBeingUsedArray.IndexOf(outputTexture.GetPixel(x,y));
                    i = colorObjects[i].index;
                    outputTexture.SetPixel(x,y,new Color32((byte)i,(byte)i,(byte)i,255));
                }
            }

        outputTexture.Apply();
    }

    private void GetColorsFromTextures()
    {
        Dictionary<Color, Color> colorsBeingUsed = new Dictionary<Color, Color>();

        foreach (Color color in inputTexture.GetPixels())
            if (!colorsBeingUsed.ContainsKey(color) && color.a != 0)
                colorsBeingUsed.Add(color, color);

        Color[] colorsBeingUsedArray = colorsBeingUsed.Keys.ToArray();
        colorObjects.Clear();

        for (int i = 0; i < colorsBeingUsedArray.Length; i++)
        {
            ColorObject colorObject = new ColorObject();
            colorObject.inColor = colorsBeingUsedArray[i];
            colorObjects.Add(colorObject);
        }
    }

    private void Save()
    {
        if (outputTexture == null)
            return;

             //Encode the packed texture to PNG
            byte[] bytes = outputTexture.EncodeToPNG();
        
            //Save the packed texture to the datapath of your choice
            File.WriteAllBytes(saveLocation + "/" + fileName + ".png", bytes);
      
    }
}