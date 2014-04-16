using UnityEngine;
using System.Collections;
using UnityEditor;

public class PathTool : ScriptableObject {

    static PathNode m_parent = null;

    [MenuItem("PathTool/Set Parent %q")]
    // Use this for initialization
    static void SetParent()
    {
        if (!Selection.activeGameObject || Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1)
            return;

        if (Selection.activeGameObject.tag.CompareTo("pathnode") == 0)
        {
            m_parent = Selection.activeGameObject.GetComponent<PathNode>();
        }
    }

    [MenuItem("PathTool/Set Next %W")]
    static void SetNext()
    {
        if (!Selection.activeGameObject || m_parent == null || Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1)
        {
            return;
        }

        if (Selection.activeGameObject.tag.CompareTo("pathnode") == 0)
        {
            m_parent.SetNext(Selection.activeGameObject.GetComponent<PathNode>());
            m_parent = null;
        }
    }
}
