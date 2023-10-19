using nadena.dev.modular_avatar.core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using UnityEditor;
using System.IO;
using HhotateA.AvatarModifyTools.MagicalDresserInventorySystem;

namespace HhotateA.AvatarModifyTools.MagicalDresserInventorySystem.ModularAvatarExtension
{
    [CustomEditor(typeof(MagicalDresserInventoryModularAvatar))]
    class MagicalDresserInventoryModularAvatarEditor : Editor
    {
        void OnEnable()
        {
            serializedObject.Update();
            var saveData = serializedObject.FindProperty(nameof(MagicalDresserInventoryModularAvatar.MagicalDresserInventorySaveData));
            var saveDataPath = serializedObject.FindProperty(nameof(MagicalDresserInventoryModularAvatar.MagicalDresserInventorySaveDataPath));
            if (saveData.objectReferenceValue == null && File.Exists(saveDataPath.stringValue))
            {
                Debug.Log("Load SaveData");
                saveData.objectReferenceValue = AssetDatabase.LoadAssetAtPath<Object>(saveDataPath.stringValue);
            }
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var saveData = serializedObject.FindProperty(nameof(MagicalDresserInventoryModularAvatar.MagicalDresserInventorySaveData));
            var saveDataPath = serializedObject.FindProperty(nameof(MagicalDresserInventoryModularAvatar.MagicalDresserInventorySaveDataPath));
            EditorGUILayout.PropertyField(saveData);
            if (saveData.objectReferenceValue == null || (saveData.objectReferenceValue.GetType().FullName != "HhotateA.AvatarModifyTools.MagicalDresserInventorySystem.MagicalDresserInventorySaveData"))
            {
                EditorGUILayout.HelpBox("Please select a MagicalDresserInventorySaveData.", MessageType.Error);
            }
            if (saveData.objectReferenceValue == null)
            {
                EditorGUILayout.PropertyField(saveDataPath);
            }
            else
            {
                var path = AssetDatabase.GetAssetPath(saveData.objectReferenceValue);
                if (saveDataPath.stringValue != path)
                {
                    saveDataPath.stringValue = path;
                }
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(MagicalDresserInventoryModularAvatar.TargetMenu)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(MagicalDresserInventoryModularAvatar.FromParent)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(MagicalDresserInventoryModularAvatar.pathMode)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(MagicalDresserInventoryModularAvatar.matchAvatarWriteDefaults)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(MagicalDresserInventoryModularAvatar.internalParameter)));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
