#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
[InitializeOnLoad]
public class ScriptableObjectIconSetter
{
    private static readonly Dictionary<string, string> TypeToIconMap = new Dictionary<string, string>
    {
        { "LethalMinLibrary.LibPikminType", "LethalMinLibrary.No_Pikmin_P2S_icon.png" },
        { "LethalMinLibrary.LibOnionType", "LethalMinLibrary.OnionTIcon.png" },
        { "LethalMinLibrary.LibOnionFuseRules", "LethalMinLibrary.OnionFLIcon.png" },
        { "LethalMinLibrary.LibPikminSoundPack", "LethalMinLibrary.PikminSPicon.png" },
        { "LethalMinLibrary.LibPikminAnimationPack", "LethalMinLibrary.PikminAPicon.png" },
        { "LethalMinLibrary.LMLmodInfo", "LethalMinLibrary.Infoicon.png" },
        { "LethalMinBundleContainer", "LethalMinLibrary.LethalMinFileIcon.png" }
        // Add more mappings here as needed, e.g.:
        // { "LethalMinLibrary.OnionType", "LethalMinLibrary.Onion_Icon.png" },
    };

    static ScriptableObjectIconSetter()
    {
        EditorApplication.projectWindowItemOnGUI += UpdateProjectWindowItem;
        EditorApplication.delayCall += UpdateAllScriptableObjectIcons;
    }

    static void UpdateProjectWindowItem(string guid, Rect selectionRect)
    {
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

        if (obj != null && TypeToIconMap.ContainsKey(obj.GetType().FullName))
        {
            SetIconForScriptableObject(obj);
        }
    }

    static void UpdateAllScriptableObjectIcons()
    {
        foreach (var typeName in TypeToIconMap.Keys)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeName}");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                if (obj != null && obj.GetType().FullName == typeName)
                {
                    SetIconForScriptableObject(obj);
                }
            }
        }
    }

    static void SetIconForScriptableObject(Object scriptableObject)
    {
        string typeName = scriptableObject.GetType().FullName;
        if (!TypeToIconMap.TryGetValue(typeName, out string iconResourceName))
        {
            return;
        }

        Texture2D icon = LoadEmbeddedIcon(iconResourceName);
        if (icon != null)
        {
            EditorGUIUtility.SetIconForObject(scriptableObject, icon);

            string[] labels = AssetDatabase.GetLabels(scriptableObject);
            string labelName = scriptableObject.GetType().Name;
            if (!labels.Contains(labelName))
            {
                ArrayUtility.Add(ref labels, labelName);
                AssetDatabase.SetLabels(scriptableObject, labels);
            }

            EditorUtility.SetDirty(scriptableObject);
        }
    }

    static Texture2D LoadEmbeddedIcon(string resourceName)
    {
        Assembly assembly = Assembly.Load("NoteBoxz.LethalMinLibrary");
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                Debug.LogError($"Failed to load embedded icon resource: {resourceName}");
                return null;
            }

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);

            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(buffer);

            return texture;
        }
    }
}
#endif