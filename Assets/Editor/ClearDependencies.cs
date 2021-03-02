using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ClearDependencies : MonoBehaviour
{
    [System.Obsolete]
    public static void CheckParticleSystemRenderer()
    {
        Object[] gos = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var item in gos)
        {
            // Filter non-prefab type,
            if (PrefabUtility.GetPrefabType(item) != PrefabType.Prefab)
            {
                continue;
            }

            GameObject gameObj = item as GameObject;

            ParticleSystemRenderer[] renders = gameObj.GetComponentsInChildren<ParticleSystemRenderer>(true);
            foreach (var renderItem in renders)
            {
                if (renderItem.renderMode != ParticleSystemRenderMode.Mesh)
                {
                    renderItem.mesh = null;
                    EditorUtility.SetDirty(gameObj);
                }
            }
        }

        AssetDatabase.SaveAssets();
    }

    static void ClearMatProperties()
    {
        UnityEngine.Object[] objs = Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets);
        for (int i = 0; i < objs.Length; ++i)
        {
            Material mat = objs[i] as Material;

            if (mat)
            {
                SerializedObject psSource = new SerializedObject(mat);
                SerializedProperty emissionProperty = psSource.FindProperty("m_SavedProperties");
                SerializedProperty texEnvs = emissionProperty.FindPropertyRelative("m_TexEnvs");

                if (CleanMaterialSerializedProperty(texEnvs, mat))
                {
                    Debug.LogError("Find and clean useless texture propreties in " + mat.name);
                }

                psSource.ApplyModifiedProperties();
                EditorUtility.SetDirty(mat);
            }
        }

        AssetDatabase.SaveAssets();
    }

    //true: has useless propeties
    private static bool CleanMaterialSerializedProperty(SerializedProperty property, Material mat)
    {
        bool res = false;

        for (int j = property.arraySize - 1; j >= 0; j--)
        {
            string propertyName = property.GetArrayElementAtIndex(j).FindPropertyRelative("first").FindPropertyRelative("name").stringValue;

            //SerializedProperty arrayElementAtIndex = property.GetArrayElementAtIndex(j);
            //SerializedProperty serializedProperty3 = arrayElementAtIndex.FindPropertyRelative(“first”);
            //string propertyName = serializedProperty3.stringValue;

            if (!mat.HasProperty(propertyName))
            {
                if (propertyName.Equals("_MainTex"))
                {
                    //_MainTex是内建属性，是置空不删除，否则UITexture等控件在获取mat.maintexture的时候会报错
                    if (property.GetArrayElementAtIndex(j).FindPropertyRelative("second").FindPropertyRelative("m_Texture").objectReferenceValue != null)
                    {
                        property.GetArrayElementAtIndex(j).FindPropertyRelative("second").FindPropertyRelative("m_Texture").objectReferenceValue = null;
                        Debug.Log("Set _MainTex is null");
                        res = true;
                    }
                }
                else
                {
                    property.DeleteArrayElementAtIndex(j);
                    Debug.Log("Delete property in serialized object : " + propertyName);
                    res = true;
                }
            }
        }
        return res;
    }
}
