using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Unity
using UnityEngine;
using UnityEditor;

namespace OPS.Obfuscator.Editor.Gui
{
    public class InfoWindow : EditorWindow
    {
        [MenuItem("OPS/AntiCheat/Info")]

        public static void ShowWindow()
        {
            var var_Window = EditorWindow.GetWindow(typeof(InfoWindow));
            var_Window.titleContent = new GUIContent("AntiCheat");
        }

        /// <summary>
        /// Scrollview position 2d.
        /// </summary>
        private Vector2 scrollPosition;

        private void OnGUI()
        {
            try
            {
                //Start Scroll
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                GUIStyle var_TopBarButtonStyle = new GUIStyle("button");
                var_TopBarButtonStyle.normal.background = null;
                var_TopBarButtonStyle.active.background = null;

                GUILayout.BeginVertical();

                GUILayout.Space(20);

                GUILayout.BeginHorizontal();

                GUILayout.Space(20);

                GUILayout.Label((Texture)EditorGUIUtility.Load("Assets/OPS/AntiCheat/Editor/Gui/Resources/Icon_32x32.png"), GUILayout.MaxWidth(24), GUILayout.MaxHeight(24), GUILayout.MinWidth(24), GUILayout.MinHeight(24));

                GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
                boldStyle.alignment = TextAnchor.MiddleLeft;
                boldStyle.fontSize = 15;
                boldStyle.fontStyle = FontStyle.Bold;

                GUILayout.Label("AntiCheat Free v3.1.3", boldStyle);

                GUILayout.FlexibleSpace();
                if (GUILayout.Button((Texture)EditorGUIUtility.Load("Assets/OPS/AntiCheat/Editor/Gui/Resources/Rate.png"), var_TopBarButtonStyle, GUILayout.MaxWidth(100), GUILayout.MaxHeight(26)))
                {
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/anti-cheat-free-140341");
                }
                if (GUILayout.Button((Texture)EditorGUIUtility.Load("Assets/OPS/AntiCheat/Editor/Gui/Resources/BugQuestion.png"), var_TopBarButtonStyle, GUILayout.MaxWidth(200), GUILayout.MaxHeight(26)))
                {
                    Application.OpenURL("mailto:guardingpearsoftware@gmail.com?subject=AntiCheat_Free");
                }

                GUILayout.Space(20);

                GUILayout.EndHorizontal();

                GUILayout.Space(5);

                GUILayout.BeginHorizontal();

                GUILayout.Space(20);

                EditorGUILayout.HelpBox("Special offer: Write a review for AntiCheat Free and write a mail to guardingpearsoftware@gmail.com and win AntiCheat Pro.", MessageType.Warning);

                GUILayout.Space(20);

                GUILayout.EndHorizontal();

                GUILayout.EndVertical();

                //End Scroll
                GUILayout.EndScrollView();
            }
            catch (Exception e)
            {
                Debug.LogError("[OPS.OBF] " + e.ToString());
                this.Close();
            }
        }
    }
}