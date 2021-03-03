using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//https://docs.unity3d.com/ScriptReference/SerializedObject.html
//https://answer.uwa4d.com/question/5bade8deafd2174f6fe94675

namespace TenentEditorTool
{
	public class OptimizeAssetsDependency
	{
		//check select CommonPrefab dependency
		[MenuItem("Assets/OptimizeAssetsDependency/CheckSelectionPrefabDependency")]
		public static void CheckSelectionCommonPrefab()
		{
			Object[] gos = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			foreach (var item in gos)
			{
				// Filter non-prefab type,
				if (PrefabUtility.GetPrefabAssetType(item) != PrefabAssetType.NotAPrefab)
				{
					GameObject go = item as GameObject;
					if (go)
					{
						EditorUtility.SetDirty(go);
					}
				}
			}

			Debug.Log("<color=green> CheckSelectionPrefabDependency Success </color>");
			AssetDatabase.SaveAssets();
		}

		//check all CommonPrefab dependency
		[MenuItem("Assets/OptimizeAssetsDependency/CheckAllPrefabDependency")]
		public static void CheckAllCommonPrefab()
		{
			string[] ids = AssetDatabase.FindAssets("t:Prefab");
			for (int i = 0; i < ids.Length; ++i)
			{
				GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(ids[i]));
				if (go != null)
				{
					if (PrefabUtility.GetPrefabAssetType(go) != PrefabAssetType.NotAPrefab)
					{
						EditorUtility.SetDirty(go);
					}
				}
			}

			Debug.Log("<color=green> CheckAllPrefabDependency Success </color>");
			AssetDatabase.SaveAssets();
		}

		[MenuItem("Assets/OptimizeAssetsDependency/CheckParticleDependency")]
		public static void CheckParticleSystemPrefab()
		{
			Object[] gos = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			foreach (var item in gos)
			{
				// Filter non-prefab type,
				if (PrefabUtility.GetPrefabAssetType(item) != PrefabAssetType.NotAPrefab)
				{
					GameObject go = item as GameObject;
					if (go)
					{
						ParticleSystemRenderer[] renders = go.GetComponentsInChildren<ParticleSystemRenderer>(true);
						foreach (var renderItem in renders)
						{
							if (renderItem.renderMode != ParticleSystemRenderMode.Mesh)
							{
								renderItem.mesh = null;
								EditorUtility.SetDirty(go);
							}
						}
					}
				}
			}

			Debug.Log("<color=green> CheckParticleDependency Success </color>");
			AssetDatabase.SaveAssets();
		}

		[MenuItem("Assets/OptimizeAssetsDependency/CheckMaterialDependency")]
		public static void CheckMaterialPropertyDependency()
		{
			int iCounts = 0;
			System.Text.StringBuilder sb = new System.Text.StringBuilder("Find and clean useless texture propreties name: ");
			Material[] mats = Selection.GetFiltered<Material>(SelectionMode.DeepAssets);
			for (int i = 0; i < mats.Length; ++i)
			{
				if (mats[i])
				{
					SerializedObject psSource = new SerializedObject(mats[i]);
					SerializedProperty emissionProperty = psSource.FindProperty("m_SavedProperties");
					SerializedProperty texEnvs = emissionProperty.FindPropertyRelative("m_TexEnvs");
					SerializedProperty floats = emissionProperty.FindPropertyRelative("m_Floats");
					SerializedProperty colos = emissionProperty.FindPropertyRelative("m_Colors");

					bool isCount = false;
					if (CleanMaterialSerializedProperty(texEnvs, mats[i]))
					{
						if (!isCount && iCounts < 1000)
						{
							sb.Append(" /Texture- ");
							sb.Append(mats[i].name);
						}

						isCount = true;
					}
					//if (CleanMaterialSerializedProperty(floats, mats[i]))
					//{
					//	if (!isCount && iCounts < 1000)
					//	{
					//		sb.Append(" /Value- ");
					//		sb.Append(mats[i].name);
					//	}

					//	isCount = true;
					//}
					//if (CleanMaterialSerializedProperty(colos, mats[i]))
					//{
					//	if (!isCount && iCounts < 1000)
					//	{
					//		sb.Append(" /Color- ");
					//		sb.Append(mats[i].name);
					//	}

					//	isCount = true;
					//}

					if (isCount)
					{
						iCounts++;
					}

					psSource.ApplyModifiedProperties();
					EditorUtility.SetDirty(mats[i]);
				}
			}

			Debug.Log($"<color=green>CheckMaterialPropertyDependency success counts: {(iCounts > 1000 ? 999 : iCounts)}</color>");
			Debug.Log($"<color=green>CheckMaterialPropertyDependency success useless propeties names: {sb.ToString()}</color>");

			AssetDatabase.SaveAssets();
		}

		private static bool CleanMaterialSerializedProperty(SerializedProperty property_, Material mat_)
		{
			bool isFind = false;

			for (int i = property_.arraySize - 1; i >= 0; i--)
			{
				string propertyName = property_.GetArrayElementAtIndex(i).FindPropertyRelative("first").stringValue;

				if (!mat_.HasProperty(propertyName))
				{
					if (propertyName.Equals("_MainTex"))
					{
						if (property_.GetArrayElementAtIndex(i).FindPropertyRelative("second").FindPropertyRelative("m_Texture").objectReferenceValue != null)
						{
							property_.GetArrayElementAtIndex(i).FindPropertyRelative("second").FindPropertyRelative("m_Texture").objectReferenceValue = null;
							isFind = true;
						}
					}
					else
					{
						property_.DeleteArrayElementAtIndex(i);
						isFind = true;
					}
				}
			}

			return isFind;
		}
	}

}
