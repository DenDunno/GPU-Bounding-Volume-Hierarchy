#if UNITY_EDITOR
using System;
using UnityEditor;

public static class InspectorExtensions
{
    private static Object[] selectedObjects = Array.Empty<Object>();
        
    [MenuItem("Edit/Toggle Inspector Lock _`")]
    private static void ToggleInspectorLock()
    {
        if (!ActiveEditorTracker.sharedTracker.isLocked)
        {
            selectedObjects = Selection.objects;
        }
        else
        {
            Selection.objects = (UnityEngine.Object[])selectedObjects;
        }
            
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }
}
#endif