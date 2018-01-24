using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class EditorWindowTextureEditor : EditorWindow
{
    Rect editAreaRect, toolPanelRect;
    Texture2D sourceTexture;
    private bool isSourceAreaEnabled, isExportAreaEnabled;

    private TextureEditorImage textureEditorImage;
    


    [MenuItem("Window/Color Palette/Texture Editor")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(EditorWindowTextureEditor));
        window.titleContent = new GUIContent("Texture Editor");
    }

    private void OnGUI()
    {
        if (textureEditorImage == null)
        {
		   textureEditorImage = new TextureEditorImage();	
		}

        toolPanelRect = new Rect(position.width - 300, 0, 300, position.height);
        editAreaRect = new Rect(40, 0, position.width - toolPanelRect.width - 40, position.height);

        ToolBar();
        EditWindow();
        ToolPanel();
    }

    private void ToolBar()
    {
        GUILayout.BeginArea(new Rect(0, 0, 40, position.height));
        GUILayout.BeginVertical();
        GUILayout.Button("S");
        GUILayout.Button("M");
        GUILayout.Button("S");
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void EditWindow()
    {
        EditorGUI.DrawRect(editAreaRect, new Color32(25, 25, 25, 255));

        //GUI.DrawTextureWithTexCoords(editAreaRect,textureEditorImage.sourceTexture,)
    }

    private void ToolPanel()
    {
        GUILayout.BeginArea(toolPanelRect);
        SourceArea();
        ExportArea();
        GUILayout.EndArea();
    }

    private void SourceArea()
    {
        if (!TitleBar(ref isSourceAreaEnabled, "Source"))
            return;

        sourceTexture = (Texture2D)EditorGUILayout.ObjectField("Texture: ", sourceTexture, typeof(Texture2D), false);

        if (textureEditorImage.colorPallet != null && textureEditorImage.colorPallet.Count > 0)
        {
            GUILayout.Label("Pallet:");
            GUILayout.BeginHorizontal();
            for (int i = 0; i < textureEditorImage.colorPallet.Count; i++)
                GUILayout.Box(textureEditorImage.colorPalletTextures[i]);
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Load")) textureEditorImage.LoadSourceTexture(sourceTexture);
    }

    private void ExportArea()
    {
        if (!TitleBar(ref isExportAreaEnabled, "Export"))
            return;

        EditorGUILayout.TextField("Save Location: ", "");
        EditorGUILayout.TextField("File Name", "");
        GUILayout.Button("Export");
    }

    private bool TitleBar(ref bool isEnabled, string title)
    {
        EditorGUILayout.BeginVertical("Box");
        isEnabled = EditorGUILayout.Foldout(isEnabled, title);
        EditorGUILayout.EndVertical();
        return isEnabled;
    }
}