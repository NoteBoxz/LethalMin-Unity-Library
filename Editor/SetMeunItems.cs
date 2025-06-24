using UnityEngine;
using UnityEditor;

public class SetMenuItems
{
    [MenuItem("Assets/Create/LethalMin Library/Pikmin Type Example")]
    static void ImportExamplePackage()
    {
        string packagePath = "Packages/com.noteboxz.lethalmin-unity-library/example.unitypackage";

        if (System.IO.File.Exists(packagePath))
        {
            AssetDatabase.ImportPackage(packagePath, true);
        }
        else
        {
            Debug.LogError("Package not found at path: " + packagePath);
        }
    }

    [MenuItem("Assets/Create/LethalMin Library/Base Game Refs")]
    static void ImportBGRPackage()
    {
        string sourcePackagePath = "Packages/com.noteboxz.lethalmin-unity-library/basegame.lethalmin.disab";
        string destFolder = "Assets/LethalMinBaseGameRefs";
        string destFileName = "basegame.lethalmin";
        string destFilePath = System.IO.Path.Combine(destFolder, destFileName);

        if (System.IO.File.Exists(sourcePackagePath))
        {
            try
            {
                // Create the destination directory if it doesn't exist
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }

                // Copy the file from package to Assets folder and remove .disab extension
                System.IO.File.Copy(sourcePackagePath, destFilePath, true);

                // Refresh the AssetDatabase to see the new file
                AssetDatabase.Refresh();

                Debug.Log($"Successfully copied and renamed base game refs to {destFilePath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error copying file: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("Package not found at path: " + sourcePackagePath);
        }
    }
}