using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BaseUnit))]
public class UnitRangesEditor : Editor
{

    void OnSceneGUI()
    {
        BaseUnit baseUnit = (BaseUnit)target;
        Handles.color = Color.red;

        Handles.DrawWireArc(baseUnit.transform.position, Vector2.zero, Vector2.right, 360, baseUnit.attackRange);
    }
}
