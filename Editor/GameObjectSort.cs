using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
public class GameObjectSort
{
    [MenuItem("GameObject/Sort Selected")]
    static void SortSelected()
    {
        Sort(Selection.gameObjects);
    }

    private static void Sort(GameObject[] sort, bool asc = true)
    {
        if (sort == null || sort.Length == 0)
        {
            return;
        }

        Transform tempRootParent = new GameObject("[Temp]").transform;
        Dictionary<Transform, List<Transform>> parentList = new Dictionary<Transform, List<Transform>>();
        for (int i = 0; i < sort.Length; i++)
        {
            Transform parent = sort[i].transform.parent == null ? tempRootParent : sort[i].transform.parent;

            if (!parentList.ContainsKey(parent))
            {
                parentList.Add(parent, new List<Transform>());
            }
            parentList[parent].Add(sort[i].transform);
        }

        foreach (KeyValuePair<Transform, List<Transform>> kvp in parentList)
        {
            Transform[] sorted = asc ? kvp.Value.OrderByDescending(o => o.name).ToArray() : kvp.Value.OrderBy(o => o.name).ToArray();
            int topIndex = sorted[0].transform.GetSiblingIndex();

            for (int i = 0; i < sorted.Length; i++)
            {
                Undo.SetTransformParent(sorted[i].transform, sorted[i].transform.parent, "Sorted object in heirarchy");
                sorted[i].transform.SetSiblingIndex(topIndex);
            }
        }
        GameObject.DestroyImmediate(tempRootParent.gameObject);
    }
}