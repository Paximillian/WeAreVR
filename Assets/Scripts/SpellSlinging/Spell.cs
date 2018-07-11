using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

public abstract class Spell : ScriptableObject
{
    public abstract void Cast(SpellWeaver i_Caster);

#if UNITY_EDITOR
    [DidReloadScripts]
    private static void onScriptReload()
    {
        string baseFolder = $"{Application.dataPath}/Data/Spells";
        if (!Directory.Exists(baseFolder)) { Directory.CreateDirectory(baseFolder); }
        string formFolder = $"{Application.dataPath}/Data/Spells/Forms";
        if (!Directory.Exists(formFolder)) { Directory.CreateDirectory(formFolder); }
        string elementFolder = $"{Application.dataPath}/Data/Spells/Elements";
        if (!Directory.Exists(elementFolder)) { Directory.CreateDirectory(elementFolder); }
        string spellFolder = $"{Application.dataPath}/Data/Spells/Spells";
        if (!Directory.Exists(spellFolder)) { Directory.CreateDirectory(spellFolder); }

        foreach (Type spellType in AppDomain.CurrentDomain.GetAssemblies()
                                                          .SelectMany(assembly => assembly.GetTypes()
                                                                                          .Where(type => MethodBase.GetCurrentMethod().DeclaringType.IsAssignableFrom(type)
                                                                                                         && !type.IsAbstract)))
        {
            string spellTypeFolder = "Spells";
            if (typeof(ElementSelectionSpell).IsAssignableFrom(spellType)) { spellTypeFolder = "Elements"; }
            else if (typeof(FormSelectionSpell).IsAssignableFrom(spellType)) { spellTypeFolder = "Forms"; }

            string assetPath = $"Assets/Data/Spells/{spellTypeFolder}/{spellType.Name}.asset";
            if (AssetDatabase.GetMainAssetTypeAtPath(assetPath) == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(spellType), assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
#endif
}
