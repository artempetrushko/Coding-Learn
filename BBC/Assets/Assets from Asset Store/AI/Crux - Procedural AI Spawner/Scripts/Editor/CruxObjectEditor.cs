using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Crux.Utility
{
    [CustomEditor(typeof(CruxObject))]
    [System.Serializable]
    public class CruxObjectEditor : Editor
    {
        public Texture CruxObjectIcon;
        Texture2D BackgroundImage;
        Editor gameObjectEditor;
        GameObject LastObject;
        Texture HelpIcon;
        int instanceID;
        string ModelPreviewText = "Hide Model Preview";
        bool ModelPreviewState = true;

        ReorderableList AllWeatherTypesList;

        void GenerateSpawnID()
        {
            CruxObject self = (CruxObject)target;
            string GeneratedID = "";
            string RandomLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //Generate 3 random letters
            for (int b = 0; b < 3; b++)
            {
                GeneratedID = RandomLetter[UnityEngine.Random.Range(0, RandomLetter.Length)] + GeneratedID;
            }

            //Generate 3 random numbers between 0 and 9
            for (int j = 0; j < 3; j++)
            {
                GeneratedID = GeneratedID + UnityEngine.Random.Range(0, 10).ToString();
            }

            self.SpawnID = GeneratedID;
        }



        void OnEnable()
        {
            CruxObject self = (CruxObject)target;

            if (CruxObjectIcon == null && self.UseCruxObjectIcon == CruxObject.YesNoEnum.No || CruxObjectIcon == null) CruxObjectIcon = Resources.Load("CruxObjectIcon") as Texture;
            if (HelpIcon == null) HelpIcon = Resources.Load("HelpIcon") as Texture;
            if (BackgroundImage == null) BackgroundImage = Resources.Load("PreviewBackground") as Texture2D;

            if (self.SpawnID == "")
            {
                GenerateSpawnID();
            }

            AllWeatherTypesList = new ReorderableList(serializedObject, serializedObject.FindProperty("WeatherTypesList"), true, true, true, true);
            AllWeatherTypesList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Weather Types", EditorStyles.boldLabel);
            };
            AllWeatherTypesList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = AllWeatherTypesList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };
        }

        public override void OnInspectorGUI()
        {
            CruxObject self = (CruxObject)target;
            serializedObject.Update();

            GUIStyle TitleStyle = new GUIStyle(EditorStyles.toolbarButton);
            TitleStyle.fontStyle = FontStyle.Bold;
            TitleStyle.fontSize = 14;
            TitleStyle.alignment = TextAnchor.UpperCenter;
            TitleStyle.normal.textColor = Color.white;

            var HelpStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.UpperRight };

            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginVertical(GUILayout.Width(90 * Screen.width / 100));
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

            if (GUILayout.Button(new GUIContent(HelpIcon), HelpStyle, GUILayout.Height(22.5f)))
            {
                Application.OpenURL("https://docs.google.com/document/d/1Ee0eqh9kzGXK_2IrcXBL3KpujmGctSgPkFkLB4HjR4w/edit#heading=h.oohyzz9cx043");
            }

            GUI.backgroundColor = new Color(0.25f, 0.25f, 0.25f, 0.5f);
            EditorGUILayout.LabelField("Crux Object", TitleStyle);
            GUI.backgroundColor = Color.white;
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("A Crux Object allows you to individually customize the spawnable object’s conditions, rarity tier, information, population amounts, and more.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            GUILayout.Space(6);

            EditorGUILayout.LabelField(self.ObjectName, style, GUILayout.ExpandWidth(true));
            GUILayout.Space(2);

#if UNITY_EDITOR
            if (ModelPreviewState)
            {
                GUIStyle bgColor = new GUIStyle();
                bgColor.normal.background = BackgroundImage;
                if (self.ObjectToSpawn != null || LastObject != self.ObjectToSpawn)
                {
                    if (gameObjectEditor == null)
                    {
                        gameObjectEditor = Editor.CreateEditor(self.ObjectToSpawn);
                    }
                    else if (LastObject != self.ObjectToSpawn)
                    {
                        gameObjectEditor = null;
                        gameObjectEditor = Editor.CreateEditor(self.ObjectToSpawn);
                    }

                    gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
                    LastObject = self.ObjectToSpawn;
                }
            }

            GUILayout.Space(6);
            GUI.backgroundColor = new Color(0.0f, 0.2f, 1f, 0.75f);
            var CreateBiomeStyle = new GUIStyle(GUI.skin.button);
            CreateBiomeStyle.normal.textColor = Color.white;
            CreateBiomeStyle.fontStyle = FontStyle.Bold;
            if (GUILayout.Button(ModelPreviewText, CreateBiomeStyle))
            {
                ModelPreviewState = !ModelPreviewState;

                if (!ModelPreviewState)
                {
                    ModelPreviewText = "Show Model Preview";
                }
                else
                {
                    ModelPreviewText = "Hide Model Preview";
                }
            }
            GUI.backgroundColor = Color.white;
#endif
            if (Application.isPlaying)
            {
                Editor.Destroy(gameObjectEditor);
            }


            GUILayout.Space(4);
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.Space();

            //Info
            GUI.backgroundColor = new Color(0.25f, 0.25f, 0.25f, 0.5f);
            EditorGUILayout.LabelField("Info", TitleStyle);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);

            self.ObjectName = EditorGUILayout.TextField("Object Name", self.ObjectName);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The name of the object that is spawning. This name will be applied when it's created.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.ObjectToSpawn = (GameObject)EditorGUILayout.ObjectField("Object to Spawn", self.ObjectToSpawn, typeof(GameObject), false);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The Game Object that will be spawning.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.UseCruxObjectIcon = (CruxObject.YesNoEnum)EditorGUILayout.EnumPopup("Use Crux Object Icon", self.UseCruxObjectIcon);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls whether or not this Crux Object will use a custom icon. If no is selected, the default icon will be used instead.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            if (self.UseCruxObjectIcon == CruxObject.YesNoEnum.Yes)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginVertical();

                self.CruxObjectIcon = (Texture)EditorGUILayout.ObjectField("Crux Object Icon", self.CruxObjectIcon, typeof(Texture), false);
                CruxObjectIcon = self.CruxObjectIcon;

                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("Spawn ID: " + self.SpawnID, style, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("Regenerate Spawn ID"))
            {
                GenerateSpawnID();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(15);

            //Settings
            GUI.backgroundColor = new Color(0.25f, 0.25f, 0.25f, 0.5f);
            EditorGUILayout.LabelField("Settings", TitleStyle);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);

            self.Rarity = (CruxObject.RarityEnum)EditorGUILayout.EnumPopup("Spawn Rarity", self.Rarity);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The chance to spawn, if the necessary conditions are met for a successful spawn.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.MaxPopulation = EditorGUILayout.IntSlider("Population Cap", self.MaxPopulation, 1, 20);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the total spawning population cap for this object. Once the Population Cap has been reached, " +
                "no more of this object will spawn until it's been despawned or destroyed.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.SpawnYOffset = EditorGUILayout.Slider("Spawn Height Offset", self.SpawnYOffset, -10, 10);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The Y position offset for spawning this object. This is useful if your AI's transform is off and is spawning too high or low.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.MinSpawnElevation = EditorGUILayout.IntSlider("Min Spawning Elevation", self.MinSpawnElevation, -500, 2000);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The minimum elevation this AI can spawn at. This can be useful for keeping water objects in the water and mountain objects at higher elevations.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.MaxSpawnElevation = EditorGUILayout.IntSlider("Max Spawning Elevation", self.MaxSpawnElevation, -500, 2000);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The maximum elevation this AI can spawn at. This can be useful for keeping water objects in the water and mountain objects at higher elevations.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            self.HeightType = (CruxObject.HeightTypeEnum)EditorGUILayout.EnumPopup("AI Type", self.HeightType);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the type of AI which is used for spawning. Air and Water let you adjust the spawning height.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(self.HeightType == CruxObject.HeightTypeEnum.Land);
            self.MinSpawnHeight = EditorGUILayout.IntSlider("Min Spawn Height", self.MinSpawnHeight, 0, 30);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The minimum spawning height on the Y axis allowed. This is useful for water and flying AI that need to have randomized spawning heights.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.MaxSpawnHeight = EditorGUILayout.IntSlider("Max Spawn Height", self.MaxSpawnHeight, 0, 30);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The maximum spawning height on the Y axis allowed. This is useful for water and flying AI that need to have randomized spawning heights.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            self.GroupSpawning = (CruxObject.YesNoEnum)EditorGUILayout.EnumPopup("Use Group Spawning", self.GroupSpawning);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls whether or not this object can be spawned in groups.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(self.GroupSpawning == CruxObject.YesNoEnum.No);
            self.GroupSpawnRadius = EditorGUILayout.IntSlider("Group Radius", self.GroupSpawnRadius, 5, 100);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The maximum group spawning radius. This helps keep AI spread out when spawning as groups.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.MinGroupAmount = EditorGUILayout.IntSlider("Min Group Amount", self.MinGroupAmount, 1, 8);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The minimum group spawning allowed.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.MaxGroupAmount = EditorGUILayout.IntSlider("Max Group Amount", self.MaxGroupAmount, 1, 8);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The maximum group spawning allowed.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(15);

            //UniStorm Conditions
#if UNISTORM_PRESENT
        GUI.backgroundColor = new Color(0.25f, 0.25f, 0.25f, 0.5f);
        EditorGUILayout.LabelField("UniStorm Conditions", TitleStyle);
        GUI.backgroundColor = Color.white;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(15);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(5);

        EditorGUILayout.Space();
        self.UseUniStormConditions = (CruxObject.YesNoEnum)EditorGUILayout.EnumPopup("Use UniStorm Conditions", self.UseUniStormConditions);
        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
        EditorGUILayout.LabelField("Controls whether or not this object will use UniStorm Conditions such as only spawning during certain weather types and times.", EditorStyles.helpBox);
        GUI.backgroundColor = Color.white;
        EditorGUILayout.Space();

        if (self.UseUniStormConditions == CruxObject.YesNoEnum.Yes)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);

            self.WeatherSpawning = (CruxObject.YesNoEnum)EditorGUILayout.EnumPopup("Use Weather Spawning", self.WeatherSpawning);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls whether or not this object will spawn based off of UniStorm's weather.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);
            EditorGUI.BeginDisabledGroup(self.WeatherSpawning == CruxObject.YesNoEnum.No);
            EditorGUILayout.Space();
            GUILayout.Box(new GUIContent("What's This?", "A list of Weather Types this AI will only spawn during. Note: Your UniStorm system must contain the applied weather types."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
            AllWeatherTypesList.DoLayoutList();
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            self.TimeSpawning = (CruxObject.YesNoEnum)EditorGUILayout.EnumPopup("Use Time Spawning", self.TimeSpawning);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls whether or not this object will spawn based off of UniStorm's time.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);
            EditorGUI.BeginDisabledGroup(self.TimeSpawning == CruxObject.YesNoEnum.No);
            self.SpawningTime = (CruxObject.TimeOfDayEnum)EditorGUILayout.EnumPopup("Spawning Time", self.SpawningTime);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls what time of day this object will spawn.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            self.SeasonSpawning = (CruxObject.YesNoEnum)EditorGUILayout.EnumPopup("Use Season Spawning", self.SeasonSpawning);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls whether or not this object will spawn based off of UniStorm's season.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);
            EditorGUI.BeginDisabledGroup(self.SeasonSpawning == CruxObject.YesNoEnum.No);
            self.SpawningSeason = (CruxObject.SeasonEnum)EditorGUILayout.EnumPopup("Spawning Season", self.SpawningSeason);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls what season this object will spawn in.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(15);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
#endif

            EditorGUILayout.EndVertical();

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Undo.RecordObject(self, "Undo");

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                }
            }
#endif

            serializedObject.ApplyModifiedProperties();
        }
    }
}