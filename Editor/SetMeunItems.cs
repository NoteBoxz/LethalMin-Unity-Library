using UnityEngine;
using UnityEditor;

public class SetMenuItems
{
    [MenuItem("Assets/Create/LethalMin Library/Pikmin Type Example")]
    static void ImportExamplePackage()
    {
        string packagePath = "Packages/LethalMin-Unity-Library/Example.unitypackage";
        
        if (System.IO.File.Exists(packagePath))
        {
            AssetDatabase.ImportPackage(packagePath, true);
        }
        else
        {
            Debug.LogError("Package not found at path: " + packagePath);
        }
    }
}