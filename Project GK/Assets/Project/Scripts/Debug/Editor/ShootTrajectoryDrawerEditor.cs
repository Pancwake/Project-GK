using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShootTrajectoryDrawer))]
public class ShootTrajectoryDrawerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ShootTrajectoryDrawer drawer = (ShootTrajectoryDrawer)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Draw Trajectories"))
        {
            drawer.DrawTrajectorySweep();
        }
    }
}