using UnityEngine;
using System;
using UnityEditor;
using OMTB.Collections;

namespace OMTB.Editor
{
    public abstract class Builder : MonoBehaviour
    {
        const string basePath = "Assets/Resources/";
        //const string droppablesResourcePath = basePath + "Droppables" + "/";

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


        }

        [MenuItem("Assets/Create/OMTB/Droppable")]
        public static Droppable BuildDroppable()
        {
            try
            {
                string path = basePath + Droppable.ResourceFolder;
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                Droppable asset = ScriptableObject.CreateInstance<Droppable>();

                AssetDatabase.CreateAsset(asset, path + "/droppable.asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;

                return asset;
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("ERROR!", e.Message, "OK");
                Debug.LogError(e);
                return null;
            }
        }

        [MenuItem("Assets/Create/OMTB/Enemy")]
        public static Enemy BuildEnemy()
        {
            try
            {
                string path = basePath + Enemy.ResourceFolder;
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                Enemy asset = ScriptableObject.CreateInstance<Enemy>();

                AssetDatabase.CreateAsset(asset, path + "/enemy.asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;

                return asset;
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("ERROR!", e.Message, "OK");
                Debug.LogError(e);
                return null;
            }
        }
    }

}
