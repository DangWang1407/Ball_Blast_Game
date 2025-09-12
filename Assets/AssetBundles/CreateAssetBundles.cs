using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string platform = GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget);
        // Put bundles outside Assets to avoid re-import churn
        string root = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "AssetBundles"));
        string output = Path.Combine(root, platform);

        try
        {
            Directory.CreateDirectory(output);
            var opts = BuildAssetBundleOptions.None;
            var target = EditorUserBuildSettings.activeBuildTarget;
            var manifest = BuildPipeline.BuildAssetBundles(output, opts, target);
            if (manifest == null)
            {
                Debug.LogError($"AssetBundle build returned null manifest. Path: {output}");
            }
            else
            {
                Debug.Log($"AssetBundles built to: {output}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error during AssetBundle build: " + ex.Message);
        }
    }

    private static string GetPlatformFolder(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows64: return "StandaloneWindows64";
            case BuildTarget.StandaloneWindows: return "StandaloneWindows";
            case BuildTarget.Android: return "Android";
            case BuildTarget.iOS: return "iOS";
            case BuildTarget.StandaloneOSX: return "StandaloneOSX";
            default: return target.ToString();
        }
    }
}
