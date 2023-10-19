using HhotateA.AvatarModifyTools.MagicalDresserInventorySystem;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using VRC.SDK3.Avatars.ScriptableObjects;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

[assembly: ExportsPlugin(typeof(HhotateA.AvatarModifyTools.MagicalDresserInventorySystem.ModularAvatarExtension.MagicalDresserInventoryModularAvatarProcessor))]
namespace HhotateA.AvatarModifyTools.MagicalDresserInventorySystem.ModularAvatarExtension
{
    public class MagicalDresserInventoryModularAvatarProcessor : Plugin<MagicalDresserInventoryModularAvatarProcessor>
    {
        protected override void Configure()
        {
            InPhase(BuildPhase.Resolving).BeforePlugin("nadena.dev.modular-avatar").Run("MagicalDresserInventory to MA", ctx =>
            {
                var mdis = ctx.AvatarRootObject.GetComponentsInChildren<MagicalDresserInventoryModularAvatar>();
                foreach (var mdi in mdis)
                {
                    if (mdi.MagicalDresserInventorySaveData == null && File.Exists(mdi.MagicalDresserInventorySaveDataPath))
                    {
                        mdi.MagicalDresserInventorySaveData = AssetDatabase.LoadAssetAtPath<Object>(mdi.MagicalDresserInventorySaveDataPath);
                    }
                    if (mdi.MagicalDresserInventorySaveData.GetType().FullName == "HhotateA.AvatarModifyTools.MagicalDresserInventorySystem.MagicalDresserInventorySaveData")
                    {
                        var assets = MagicalDresserInventorySaveData_assets.GetValue(mdi.MagicalDresserInventorySaveData);
                        var animator = mdi.gameObject.AddComponent<ModularAvatarMergeAnimator>();
                        animator.layerType = VRC.SDK3.Avatars.Components.VRCAvatarDescriptor.AnimLayerType.FX;
                        animator.animator = Assets_fx_controller.GetValue(assets) as UnityEditor.Animations.AnimatorController;
                        animator.matchAvatarWriteDefaults = mdi.matchAvatarWriteDefaults;
                        animator.pathMode = mdi.pathMode;

                        var parameters = mdi.gameObject.AddComponent<ModularAvatarParameters>();
                        parameters.parameters = (Assets_parameter.GetValue(assets) as VRCExpressionParameters).parameters.Select(p => new ParameterConfig
                        {
                            nameOrPrefix = p.name,
                            internalParameter = mdi.internalParameter,
                            syncType = ParameterSyncTypeOf(p.valueType),
                            localOnly = !p.networkSynced,
                            saved = p.saved,
                            defaultValue = p.defaultValue,
                        }).ToList();

                        var menu = mdi.gameObject.AddComponent<ModularAvatarMenuInstaller>();
                        var menuAsset = Assets_menu.GetValue(assets) as VRCExpressionsMenu;
                        menu.menuToAppend = mdi.FromParent ? menuAsset : menuAsset.controls.First().subMenu;
                        menu.installTargetMenu = mdi.TargetMenu;
                    }
                }
            });
        }

        ParameterSyncType ParameterSyncTypeOf(VRCExpressionParameters.ValueType valueType)
        {
            switch (valueType)
            {
                case VRCExpressionParameters.ValueType.Bool:
                    return ParameterSyncType.Bool;
                case VRCExpressionParameters.ValueType.Float:
                    return ParameterSyncType.Float;
                case VRCExpressionParameters.ValueType.Int:
                    return ParameterSyncType.Int;
                default:
                    return ParameterSyncType.NotSynced;
            }
        }

        // asmdef で参照できないので
        Assembly AssemblyCSharpEditor
        {
            get
            {
                if (_AssemblyCSharpEditor == null)
                {
                    _AssemblyCSharpEditor = Assembly.Load("Assembly-CSharp-Editor");
                }
                return _AssemblyCSharpEditor;
            }
        }
        Assembly _AssemblyCSharpEditor;
        System.Type MagicalDresserInventorySaveDataType
        {
            get
            {
                if (_MagicalDresserInventorySaveDataType == null)
                {
                    _MagicalDresserInventorySaveDataType = AssemblyCSharpEditor.GetType("HhotateA.AvatarModifyTools.MagicalDresserInventorySystem.MagicalDresserInventorySaveData");
                }
                return _MagicalDresserInventorySaveDataType;
            }
        }
        System.Type _MagicalDresserInventorySaveDataType;
        System.Type AvatarModifyDataType
        {
            get
            {
                if (_AvatarModifyDataType == null)
                {
                    _AvatarModifyDataType = AssemblyCSharpEditor.GetType("HhotateA.AvatarModifyTools.Core.AvatarModifyData");
                }
                return _AvatarModifyDataType;
            }
        }
        System.Type _AvatarModifyDataType;

        System.Reflection.FieldInfo MagicalDresserInventorySaveData_assets
        {
            get
            {
                if (_MagicalDresserInventorySaveData_assets == null)
                {
                    _MagicalDresserInventorySaveData_assets = MagicalDresserInventorySaveDataType.GetField("assets", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                }
                return _MagicalDresserInventorySaveData_assets;
            }
        }

        System.Reflection.FieldInfo _MagicalDresserInventorySaveData_assets;
        System.Reflection.FieldInfo Assets_fx_controller
        {
            get => AvatarModifyDataType.GetField("fx_controller", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }
        System.Reflection.FieldInfo Assets_parameter
        {
            get => AvatarModifyDataType.GetField("parameter", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }
        System.Reflection.FieldInfo Assets_menu
        {
            get => AvatarModifyDataType.GetField("menu", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }
    }
}
