using UnityEngine;
using System.Collections.Generic;
using LethalMinLibrary;

// Container to hold references to bundle assets
public class LethalMinBundleContainer : ScriptableObject
{
    public string assetPath;
    public string bundleName;
    public string[] assetNames;
    public string modName;
    public string modAuthor;
    public string modVersion;
    public LMLmodInfo modInfo;
    
    [System.Serializable]
    public class AssetReference
    {
        public string name;
        public Object asset;
    }
    
    public List<AssetReference> assetReferences = new List<AssetReference>();
    
    public void AddAssetReference(string name, Object asset)
    {
        assetReferences.Add(new AssetReference { name = name, asset = asset });
    }
}