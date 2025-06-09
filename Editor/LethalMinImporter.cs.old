using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.IO;
using System.Collections.Generic;
using LethalMinLibrary;

[ScriptedImporter(1, "LethalMin")]
public class LethalMinBundleImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        // Create a container asset to hold reference to all assets in the bundle
        var container = ScriptableObject.CreateInstance<LethalMinBundleContainer>();
        container.assetPath = ctx.assetPath;
        
        // Load the asset bundle
        AssetBundle bundle = AssetBundle.LoadFromFile(ctx.assetPath);
        if (bundle == null)
        {
            Debug.LogError($"Failed to load {ctx.assetPath} as an asset bundle");
            return;
        }
        
        try
        {
            // Extract bundle info
            container.bundleName = Path.GetFileNameWithoutExtension(ctx.assetPath);
            
            // Get all assets from the bundle
            string[] assetNames = bundle.GetAllAssetNames();
            container.assetNames = assetNames;
            
            // Load LMLmodInfo for main metadata
            LMLmodInfo[] modInfos = bundle.LoadAllAssets<LMLmodInfo>();
            if (modInfos.Length > 0)
            {
                container.modInfo = modInfos[0];
                container.modName = modInfos[0].ModName;
                container.modAuthor = modInfos[0].ModAuthor;
                container.modVersion = modInfos[0].Version;
            }
            
            // Add the container as the main asset
            ctx.AddObjectToAsset("BundleContainer", container);
            ctx.SetMainObject(container);
            
            // Extract and add important assets as sub-assets
            foreach (string assetName in assetNames)
            {
                Object asset = bundle.LoadAsset<Object>(assetName);
                if (asset != null)
                {
                    string objectName = Path.GetFileNameWithoutExtension(assetName);
                    ctx.AddObjectToAsset(objectName, asset);
                    
                    // Add to container's references
                    container.AddAssetReference(objectName, asset);
                }
            }
        }
        finally
        {
            bundle.Unload(false);
        }
    }
}