using nadena.dev.modular_avatar.core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace HhotateA.AvatarModifyTools.MagicalDresserInventorySystem.ModularAvatarExtension
{
    [AddComponentMenu("Modular Avatar/MA MagicalDresserInventory")]
    public class MagicalDresserInventoryModularAvatar : AvatarTagComponent
    {
        public Object MagicalDresserInventorySaveData;
        public string MagicalDresserInventorySaveDataPath;
        public VRCExpressionsMenu TargetMenu;
        public bool FromParent = true;
        public MergeAnimatorPathMode pathMode = MergeAnimatorPathMode.Relative;
        public bool matchAvatarWriteDefaults = true;
        public bool internalParameter;
    }
}
