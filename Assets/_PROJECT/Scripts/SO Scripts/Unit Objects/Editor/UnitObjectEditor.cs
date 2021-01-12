using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitObject))]
public class UnitObjectEditor : Editor
{
    private UnitObject _unit;
    private void OnEnable() => _unit = (UnitObject)target;
    private void OnDisable() => _unit = null;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Randomise Stats")) _unit.CalculateRandomisedStats();
    }
}
