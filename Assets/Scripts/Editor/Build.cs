using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;

namespace HeavyDev
{
    public class Build
    {
        // Set build path
        static string buildPath = UnityEngine.Application.dataPath + "/../Release";

        [MenuItem("HeavyDev/Build/List Build Paths")]
        public static void ListBuildPaths()
        {
            Dictionary<string, string> paths = new Dictionary<string, string>()
            {
                {"EditorApplication.applicationContentsPath",   EditorApplication.applicationContentsPath},
                {"EditorApplication.applicationPath",           EditorApplication.applicationPath},
                {"UnityEngine.Application.dataPath",            UnityEngine.Application.dataPath},
                {"UnityEngine.Application.absoluteURL",         UnityEngine.Application.absoluteURL},
                {"UnityEngine.Application.persistentDataPath",  UnityEngine.Application.persistentDataPath},
                {"UnityEngine.Application.streamingAssetsPath",  UnityEngine.Application.streamingAssetsPath}
            };

            foreach(KeyValuePair<string, string> path in paths)
            {
                UnityEngine.Debug.Log(string.Format("{0} => {1}", path.Key, path.Value));
            }
        }

        [MenuItem("HeavyDev/Tools/Build to WebGL")]
        public static void BuildToWebGL()
        {
            // Delete directory if it already exists
            if (System.IO.Directory.Exists(buildPath))
            {
                UnityEngine.Debug.Log(string.Format("<color=red>Deleting Directory {0}</color>", buildPath));
                System.IO.Directory.Delete(buildPath, true);
            }

            string[] scenes = new string[] {
                "Assets/Scenes/App.unity",
                "Assets/Scenes/Main.unity",
                "Assets/Scenes/Scene1.unity",
                "Assets/Scenes/Scene2.unity",
                "Assets/Scenes/Scene3.unity",
                "Assets/Scenes/Scene4.unity",
                "Assets/Scenes/Scene5.unity"
            };

            // Build player
            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, BuildOptions.None);

            // Copy a file from the project folder to the build folder, alongside the built game
            if (AssetDatabase.IsValidFolder("Assets/StreamingAssets"))
            {
                FileUtil.CopyFileOrDirectory("Assets/StreamingAssets", buildPath + "/StreamingAssets");
            }

            if (AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                FileUtil.CopyFileOrDirectory("Assets/Resources", buildPath + "/Resources");
            }

            Process proc = new Process();
            proc.Start();            
        }
    }
}