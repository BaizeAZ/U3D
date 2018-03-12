using UnityEditor;
using System.IO;

public class CreateAssetBundles  {
    
	[MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string dir = "AssetBundles";
        if (Directory.Exists("AssetBundles") == false)
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
