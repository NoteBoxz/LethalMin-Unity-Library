using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetScriptableObjectIconEditor : Editor
{
    // Dictionary mapping ScriptableObject types to their icon filenames
    private static readonly Dictionary<string, string> TypeToIconMap = new Dictionary<string, string>
    {
        { "LethalMinLibrary.LibPikminType", "No_Pikmin_P2S_icon.png" },
        { "LethalMinLibrary.LibOnionType", "OnionTIcon.png" },
        { "LethalMinLibrary.LibOnionFuseRules", "OnionFLIcon.png" },
        { "LethalMinLibrary.LibPikminSoundPack", "PikminSPicon.png" },
        { "LethalMinLibrary.LibPikminAnimationPack", "PikminAPicon.png" },
        { "LethalMinLibrary.LMLmodInfo", "Infoicon.png" },
        { "LethalMinLibrary.LethalMinBundleContainer", "LethalMinFileIcon.png" }
    };

    // This will run automatically when Unity loads
    [InitializeOnLoadMethod]
    private static void AutoSetIconsOnLoad()
    {
        // Delay execution slightly to ensure assets are fully loaded
        EditorApplication.delayCall += () =>
        {
            //Debug.Log("Checking ScriptableObject icons...");
            SetIconsIfNeeded();
        };
    }

    // Keep the manual menu item for convenience
    // [MenuItem("Tools/Set ScriptableObject Icon")]
    // private static void SetIconForScriptableObject()
    // {
    //     SetIconsIfNeeded(true);
    // }

    private static void SetIconsIfNeeded(bool forceUpdate = false)
    {
        // Find the plugin importer
        PluginImporter pluginImporter = AssetImporter.GetAtPath("Packages/com.noteboxz.lethalmin-unity-library/Plugins/NoteBoxz.LethalMinLibrary.dll") as PluginImporter;
        if (pluginImporter == null)
            pluginImporter = AssetImporter.GetAtPath("Assets/LethalMin-Unity-Library/Plugins/NoteBoxz.LethalMinLibrary.dll") as PluginImporter;

        if (pluginImporter == null)
        {
            //Debug.LogError("LethalMinLibrary plugin not found.");
            return;
        }

        bool changesNeeded = false;

        foreach (var kvp in TypeToIconMap)
        {
            // Load the icon texture
            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>($"Packages/com.noteboxz.lethalmin-unity-library/Editor/Icons/{kvp.Value}");
            if (icon == null)
                icon = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/LethalMin-Unity-Library/Editor/Icons/{kvp.Value}");

            if (icon == null)
            {
                //Debug.LogWarning($"Icon texture not found: {kvp.Value}");
                continue;
            }

            // Check if the icon is already set correctly (only if not forcing update)
            if (!forceUpdate)
            {
                changesNeeded = pluginImporter.GetIcon(kvp.Key) == null;
            }
            else
            {
                changesNeeded = true;
            }

            if (changesNeeded)
            {
                // Set the icon for the ScriptableObject type
                pluginImporter.SetIcon(kvp.Key, icon);
                //Debug.Log($"Set icon for {kvp.Key} to {kvp.Value}");
            }
        }

        // Only save and reimport if changes were made
        if (changesNeeded)
        {
            //Debug.Log("Saving and reimporting plugin with updated icons");
            pluginImporter.SaveAndReimport();
        }
        else
        {
            //Debug.Log("No icon changes needed");
        }
    }
}