using Systems;
using UI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GenerateLevelEditor : EditorWindow
    {
        private EGameTypes _gameType = EGameTypes.Football;
        private int _level = 0;
        private int _length = 1;
        
        [MenuItem("Tools/GenerateLevelEditor")]
        public static void ShowWindow()
        {
            GenerateLevelEditor window = GetWindow<GenerateLevelEditor>("Generate Level Window");
            window.Show();
        }

        private void OnGUI()
        {
            _gameType = (EGameTypes)EditorGUILayout.EnumPopup("Type", _gameType);
            _level = EditorGUILayout.IntField("Level", _level);
            _length = EditorGUILayout.IntField("Length", _length);
            
            if (GUILayout.Button("Generate"))
            {
                var selected = Selection.activeGameObject; 
                if (selected.TryGetComponent<LevelsPanel>(out var levelsPanel))
                {
                    var data = LevelGenerator.GenerateLevelData(_length);
                    levelsPanel.SetData(_gameType, _level, data);
                }
            }
        }
    }
}