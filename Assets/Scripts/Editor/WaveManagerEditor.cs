using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// #if UNITY_EDITOR
[CustomEditor(typeof(WaveManager)), CanEditMultipleObjects]
public class WaveManagerEditor : Editor
{
        WaveManager waveManager;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        void OnEnable()
        {
            Tools.hidden = true;
        }

        void OnDisable()
        {
            Tools.hidden = false;
        }
    
        private void OnSceneGUI() {
            waveManager = (WaveManager)target;
            
            for(int i = 0; i < waveManager.spawnPoints.Count; i++){
                EditorGUI.BeginChangeCheck();

                Vector3 pos = Handles.PositionHandle(waveManager.spawnPoints[i], Quaternion.identity);
            
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(waveManager, "Change Spawn Position");
                    waveManager.spawnPoints[i] = pos;
                }
            }
        }
}
// #endif