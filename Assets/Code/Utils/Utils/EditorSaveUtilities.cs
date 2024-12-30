using Code.Utils.Factory;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public static class EditorSaveUtilities
    {
        public static void Save<T>(string defaultName, IFactory<T> objectFactory) where T : Object
        {
            string filePath = EditorUtility.SaveFilePanel("Saving", "Assets", defaultName, "asset");
            
            if (filePath.Length != 0)
            {
                CreateAsset(filePath, objectFactory);
            }
        }

        private static void CreateAsset<T>(string filePath, IFactory<T> objectFactory) where T : Object
        {
            T asset = objectFactory.Create();
            string pathName = AssetDatabase.GenerateUniqueAssetPath(filePath);
            AssetDatabase.CreateAsset(asset, pathName);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}