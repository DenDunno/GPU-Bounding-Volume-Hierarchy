using Code.Utils.Factory;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public static class EditorSaveUtilities
    {
        public static bool TryGetFilePathFromSavePanel(string defaultName, out string filePath)
        {
            filePath = EditorUtility.SaveFilePanel("Saving", "Assets", defaultName, "asset");

            return filePath.Length != 0;
        }

        public static void Save<T>(string filePath, IFactory<T> objectFactory) where T : Object
        {
            Save(filePath, objectFactory.Create());
        }

        public static void Save<T>(string filePath, T asset) where T : Object
        {
            filePath = "Assets" + filePath.Substring(Application.dataPath.Length);
            AssetDatabase.CreateAsset(asset, filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }
}