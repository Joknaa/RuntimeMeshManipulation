using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using Battlehub.Integration;


namespace Battlehub.MeshTools
{
    public partial class MeshToolsMenu
    {
        [MenuItem("Tools/Mesh/Combine", validate = true)]
        public static bool CanCombine()
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length == 0)
            {
                return false;
            }

            return selectedObjects.Any(so => so.GetComponent<MeshFilter>());
        }

        [MenuItem("Tools/Mesh/Combine")]
        public static void Combine()
        {
            GameObject[] selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.Unfiltered | SelectionMode.ExcludePrefab).OfType<GameObject>().ToArray();
            GameObject activeSelected = Selection.activeTransform == null ? selection.FirstOrDefault() : Selection.activeTransform.gameObject;
            CombineResult result = MeshUtils.Combine(selection, activeSelected);
            if(result != null)
            {
                MeshCombinerIntegration.RaiseCombined(result.GameObject, result.Mesh);
            }
        }


    

    }
}
