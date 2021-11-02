using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Crux.Utility
{
    [InitializeOnLoad]
    public class CruxDefines
    {
        const string CruxDefinesString = "CRUX_PRESENT";

        static CruxDefines()
        {
            InitializeCruxDefines();
        }

        static void InitializeCruxDefines()
        {
            var BTG = EditorUserBuildSettings.selectedBuildTargetGroup;
            string CruxDef = PlayerSettings.GetScriptingDefineSymbolsForGroup(BTG);

            if (!CruxDef.Contains(CruxDefinesString))
            {
                if (string.IsNullOrEmpty(CruxDef))
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(BTG, CruxDefinesString);
                }
                else
                {
                    if (CruxDef[CruxDef.Length - 1] != ';')
                    {
                        CruxDef += ';';
                    }

                    CruxDef += CruxDefinesString;
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(BTG, CruxDef);
                }
            }
        }
    }
}
