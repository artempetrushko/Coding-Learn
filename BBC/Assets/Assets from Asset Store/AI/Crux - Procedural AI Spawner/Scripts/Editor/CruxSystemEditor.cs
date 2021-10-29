using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace Crux
{
    [CustomEditor(typeof(CruxSystem))]
    public class CruxSystemEditor : Editor
    {
        enum GenerationType
        {
            Random = 0,
            Odds = 1
        }

        Editor gameObjectEditor;
        CruxSystem t;
        SerializedObject GetTarget;
        SerializedProperty Biome;
        int ListSize;

        SerializedProperty TabNumberTop_SP;
        SerializedProperty CategoryTabNumber_SP;
        SerializedProperty BiomeList;

        //Variables
        SerializedProperty AILayerMaskProp;
        SerializedProperty TerrainInfoLayerMaskProp;
        SerializedProperty TerrainTypeProp;
        SerializedProperty SpawningOptionsTabProp;
        SerializedProperty UseLayerDetectionProp;
        SerializedProperty UseSpawnIDsProp;
        SerializedProperty PlayerTransformTypeProp;
        SerializedProperty PlayerTransformProp;
        SerializedProperty SpawnTypeProp;
        SerializedProperty UpdateFrequencyProp;
        SerializedProperty MinRadiusProp;
        SerializedProperty MaxRadiusProp;
        SerializedProperty DespawnRadiusProp;
        SerializedProperty MaxObjectsProp;
        SerializedProperty StartingAmountProp;
        SerializedProperty MinSpawnSteepnessProp;
        SerializedProperty MaxSpawnSteepnessProp;
        SerializedProperty SpawnNodeDeactivateDistanceProp;
        SerializedProperty SpawnNodeUpdateFrequencyProp;
        SerializedProperty CruxSpawnIDKeyProp;

        public string[] TabName = new string[] { "Wildlife", "Creatures", "NPC/Other" };
        public string[] TabNameTop = new string[] { "Biome Management", "Spawning Options" };
        public string[] CategoryNames = new string[] { };
        public string[] ObjectNames = new string[] { };
        public string[] SpawnIds = new string[] { };
        public string NewSpawnID;
        public bool FoldOuts = true;

        GenerationType GenerationTypeEnum = GenerationType.Odds;

        public List<ReorderableList> WildlifeList = new List<ReorderableList>();
        public List<ReorderableList> CreatureList = new List<ReorderableList>();
        public List<ReorderableList> NPCList = new List<ReorderableList>();

        public Texture WildlifeIcon;
        public Texture CreatureIcon;
        public Texture NPCIcon;
        public Texture PlantIcon;
        public Texture BiomeIcon;
        public Texture SettingsIcon;
        public Texture CruxIcon;
        public Texture HeightIcon;
        public Texture AngleIcon;
        public Texture TerrainInfoIcon;
        public Texture SpawningOptionsIcon;

        public float CurrentTerrainHeight = 0;
        public float CurrentTerrainAngle = 0;
        public Texture CurrentTerrainTexture;
        public bool RepaintedSceneView = false;

        public void UpdateReorderableLists()
        {
            //All Wildlife objects
            for (int i = 0; i < t.Biome.Count; i++)
            {
                string m_PropertyPath = "BiomeList.Array.data[" + i + "]";
                WildlifeList.Add(new ReorderableList(GetTarget, GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("WildlifeObjects"), true, true, true, true));
                WildlifeList[i].drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Wildlife Objects", EditorStyles.boldLabel);
                };
                WildlifeList[i].drawElementCallback =
                    (Rect rect, int index, bool isActive, bool isFocused) =>
                    {
                        var element = GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("WildlifeObjects").GetArrayElementAtIndex(index);
                        EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                    };
            }

            //All Creature objects
            for (int i = 0; i < t.Biome.Count; i++)
            {
                string m_PropertyPath = "BiomeList.Array.data[" + i + "]";
                CreatureList.Add(new ReorderableList(GetTarget, GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("CreatureObjects"), true, true, true, true));
                CreatureList[i].drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Creature Objects", EditorStyles.boldLabel);
                };
                CreatureList[i].drawElementCallback =
                    (Rect rect, int index, bool isActive, bool isFocused) =>
                    {
                        var element = GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("CreatureObjects").GetArrayElementAtIndex(index);
                        EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                    };
            }

            //All NPC objects
            for (int i = 0; i < t.Biome.Count; i++)
            {
                string m_PropertyPath = "BiomeList.Array.data[" + i + "]";
                NPCList.Add(new ReorderableList(GetTarget, GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("NPCObjects"), true, true, true, true));
                NPCList[i].drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "NPC Objects", EditorStyles.boldLabel);
                };
                NPCList[i].drawElementCallback =
                    (Rect rect, int index, bool isActive, bool isFocused) =>
                    {
                        var element = GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("NPCObjects").GetArrayElementAtIndex(index);
                        EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                    };
            }
        }

        void UpdateReorderableListsAddButton()
        {
            //All Wildlife objects
            for (int i = 0; i < t.Biome.Count; i++)
            {
                if (i < t.Biome.Count)
                {
                    string m_PropertyPath = "BiomeList.Array.data[" + i + "]";
                    WildlifeList.Add(new ReorderableList(GetTarget, GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("WildlifeObjects"), true, true, true, true));
                    WildlifeList[i].drawHeaderCallback = rect =>
                    {
                        EditorGUI.LabelField(rect, "Wildlife Objects", EditorStyles.boldLabel);
                    };
                    WildlifeList[i].drawElementCallback =
                        (Rect rect, int index, bool isActive, bool isFocused) =>
                        {
                            var element = GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("WildlifeObjects").GetArrayElementAtIndex(index);
                            EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                        };
                }
            }

            //All Creature objects
            for (int i = 0; i < t.Biome.Count; i++)
            {
                if (i < t.Biome.Count)
                {
                    string m_PropertyPath = "BiomeList.Array.data[" + i + "]";
                    CreatureList.Add(new ReorderableList(GetTarget, GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("CreatureObjects"), true, true, true, true));
                    CreatureList[i].drawHeaderCallback = rect =>
                    {
                        EditorGUI.LabelField(rect, "Creature Objects", EditorStyles.boldLabel);
                    };
                    CreatureList[i].drawElementCallback =
                        (Rect rect, int index, bool isActive, bool isFocused) =>
                        {
                            var element = GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("CreatureObjects").GetArrayElementAtIndex(index);
                            EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                        };
                }
            }

            //All NPC objects
            for (int i = 0; i < t.Biome.Count; i++)
            {
                if (i < t.Biome.Count)
                {
                    string m_PropertyPath = "BiomeList.Array.data[" + i + "]";
                    NPCList.Add(new ReorderableList(GetTarget, GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("NPCObjects"), true, true, true, true));
                    NPCList[i].drawHeaderCallback = rect =>
                    {
                        EditorGUI.LabelField(rect, "NPC Objects", EditorStyles.boldLabel);
                    };
                    NPCList[i].drawElementCallback =
                        (Rect rect, int index, bool isActive, bool isFocused) =>
                        {
                            var element = GetTarget.FindProperty(m_PropertyPath).FindPropertyRelative("NPCObjects").GetArrayElementAtIndex(index);
                            EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                        };
                }
            }
        }

        void OnEnable()
        {
            t = (CruxSystem)target;
            GetTarget = new SerializedObject(t);
            Biome = GetTarget.FindProperty("Biome");
            TabNumberTop_SP = GetTarget.FindProperty("TabNumberTop");
            CategoryTabNumber_SP = GetTarget.FindProperty("CategoryTabNumber");
            BiomeList = GetTarget.FindProperty("BiomeList");
            SpawningOptionsTabProp = GetTarget.FindProperty("SpawningOptionsTab");

            AILayerMaskProp = GetTarget.FindProperty("AILayerMask");
            TerrainInfoLayerMaskProp = GetTarget.FindProperty("TerrainInfoLayerMask");
            TerrainTypeProp = GetTarget.FindProperty("TerrainType");
            UseLayerDetectionProp = GetTarget.FindProperty("UseLayerDetection");
            UseSpawnIDsProp = GetTarget.FindProperty("UseSpawnIDs");
            PlayerTransformTypeProp = GetTarget.FindProperty("PlayerTransformType");
            PlayerTransformProp = GetTarget.FindProperty("m_PlayerObject");
            SpawnTypeProp = GetTarget.FindProperty("SpawnType");

            UpdateFrequencyProp = GetTarget.FindProperty("m_UpdateTickFrequency");
            MinRadiusProp = GetTarget.FindProperty("m_MinRadius");
            MaxRadiusProp = GetTarget.FindProperty("m_MaxRadius");
            DespawnRadiusProp = GetTarget.FindProperty("m_DespawnRadius");
            MaxObjectsProp = GetTarget.FindProperty("m_MaxObjectsToSpawn");
            StartingAmountProp = GetTarget.FindProperty("StartingAIAmount");
            MinSpawnSteepnessProp = GetTarget.FindProperty("minSteepness");
            MaxSpawnSteepnessProp = GetTarget.FindProperty("maxSteepness");
            SpawnNodeDeactivateDistanceProp = GetTarget.FindProperty("SpawnNodeDeactivateDistance");
            SpawnNodeUpdateFrequencyProp = GetTarget.FindProperty("SpawnNodeUpdateFrequency");
            CruxSpawnIDKeyProp = GetTarget.FindProperty("CruxSpawnIDKey");

            if (WildlifeIcon == null) WildlifeIcon = Resources.Load("WildlifeIcon") as Texture;
            if (CreatureIcon == null) CreatureIcon = Resources.Load("CreatureIcon") as Texture;
            if (NPCIcon == null) NPCIcon = Resources.Load("NPCIcon") as Texture;
            if (PlantIcon == null) PlantIcon = Resources.Load("PlantIcon") as Texture;
            if (BiomeIcon == null) BiomeIcon = Resources.Load("BiomeIcon") as Texture;
            if (SettingsIcon == null) SettingsIcon = Resources.Load("SettingsIcon") as Texture;
            if (CruxIcon == null) CruxIcon = Resources.Load("CruxIcon") as Texture;
            if (HeightIcon == null) HeightIcon = Resources.Load("HeightIcon") as Texture;
            if (AngleIcon == null) AngleIcon = Resources.Load("AngleIcon") as Texture;
            if (TerrainInfoIcon == null) TerrainInfoIcon = Resources.Load("TerrainInfoIcon") as Texture;
            if (SpawningOptionsIcon == null) SpawningOptionsIcon = Resources.Load("SpawningOptionsIcon") as Texture;

            for (int i = 0; i < Biome.arraySize; i++)
            {
                if (t.Biome[i].BiomeIcon == null) t.Biome[i].BiomeIcon = Resources.Load("DefaultBiomeIcon") as Texture;
            }

            UpdateReorderableLists();
        }

        void OnDisable()
        {
            RepaintedSceneView = false;
        }

        //Generates our Spawn IDs by picking three random characters and 3 random numbers
        //void GenerateAllSpawnIDs ()
        void SaveSpawnIDsToTxt()
        {
            for (int l = 0; l < t.Biome.Count; l++)
            {
                for (int i = 0; i < t.BiomeList[l].WildlifeObjects.Count; i++)
                {
                    t.SpawnIDs.Add("Name: " + t.BiomeList[l].WildlifeObjects[i].ObjectName + "  -  Category: " + "Wildlife" + "  -  Biome: " + t.Biome[l].CategoryName + "  -  Spawn ID: " + t.BiomeList[l].WildlifeObjects[i].SpawnID);
                }

                for (int i = 0; i < t.BiomeList[l].CreatureObjects.Count; i++)
                {
                    t.SpawnIDs.Add("Name: " + t.BiomeList[l].CreatureObjects[i].ObjectName + "  -  Category: " + "Creatures" + "  -  Biome: " + t.Biome[l].CategoryName + "  -  Spawn ID: " + t.BiomeList[l].CreatureObjects[i].SpawnID);
                }

                for (int i = 0; i < t.BiomeList[l].NPCObjects.Count; i++)
                {
                    t.SpawnIDs.Add("Name: " + t.BiomeList[l].NPCObjects[i].ObjectName + "  -  Category: " + "NPC" + "  -  Biome: " + t.Biome[l].CategoryName + "  -  Spawn ID: " + t.BiomeList[l].NPCObjects[i].SpawnID);
                }

                if (l == t.Biome.Count - 1)
                {
                    //If enabled, write all of our generated Spawn IDs to a txt file. This is convenient and helpful for testing.
                    //This allows developers, or players, to spawn AI at their position according to the spawn ID.
                    System.IO.File.WriteAllLines(t.SpawnIDsFilePath, t.SpawnIDs.ToArray());

                    //Clear the SpawnID list
                    t.SpawnIDs.Clear();

                    Repaint();
                }
            }
        }

        public override void OnInspectorGUI()
        {
            GetTarget.Update();

            ListSize = Biome.arraySize;

            GUIStyle FoldoutStyle = new GUIStyle(EditorStyles.foldout);
            FoldoutStyle.fontStyle = FontStyle.Bold;
            FoldoutStyle.fontSize = 12;
            FoldoutStyle.active.textColor = Color.black;
            FoldoutStyle.focused.textColor = Color.white;
            FoldoutStyle.onHover.textColor = Color.black;
            FoldoutStyle.normal.textColor = Color.white;
            FoldoutStyle.onNormal.textColor = Color.black;
            FoldoutStyle.onActive.textColor = Color.black;
            FoldoutStyle.onFocused.textColor = Color.black;
            //Color myStyleColor = Color.black;

            if (ListSize != Biome.arraySize)
            {
                while (ListSize > Biome.arraySize)
                {
                    Biome.InsertArrayElementAtIndex(Biome.arraySize);
                }
                while (ListSize < Biome.arraySize)
                {
                    Biome.DeleteArrayElementAtIndex(Biome.arraySize - 1);
                }
            }

            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / Screen.dpi));
            EditorGUILayout.Space();

            var CruxTopTabsStlye = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            CruxTopTabsStlye.fontSize = 15;
            EditorGUILayout.LabelField(new GUIContent(CruxIcon), CruxTopTabsStlye, GUILayout.ExpandWidth(true), GUILayout.Height(50));
            EditorGUILayout.LabelField("Crux Spawner", CruxTopTabsStlye, GUILayout.ExpandWidth(true), GUILayout.Height(22));

            EditorGUILayout.HelpBox("Crux - Procedural AI spawner allows you to spawn AI and objects procedurally across terrains. The Biome Management tab allows you to create various biomes " +
                "and assign spawnable objects to each category list. The Settings tab allows you to adjust Crux's system and global settings.", MessageType.None, true);
            GUIContent[] TabButtonsTop = new GUIContent[3] { new GUIContent(" Biome Management", BiomeIcon), new GUIContent(" Global Settings", SettingsIcon), new GUIContent(" Terrain Info", TerrainInfoIcon) };
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            TabNumberTop_SP.intValue = GUILayout.SelectionGrid(TabNumberTop_SP.intValue, TabButtonsTop, 3, GUILayout.Height(28), GUILayout.Width(85 * Screen.width / Screen.dpi));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            if (TabNumberTop_SP.intValue == 1)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                var style_Settings = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(SettingsIcon), style_Settings, GUILayout.ExpandWidth(true), GUILayout.Height(40));
                EditorGUILayout.LabelField("Global Settings", style_Settings, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space();
                GUIContent[] DetectionTagsButtons = new GUIContent[3] { new GUIContent("Spawning Options"), new GUIContent("Spawn ID Options"), new GUIContent("Documentation")};
                SpawningOptionsTabProp.intValue = GUILayout.Toolbar(SpawningOptionsTabProp.intValue, DetectionTagsButtons, EditorStyles.miniButton, GUILayout.Height(25), GUILayout.Width(87.5f * Screen.width / Screen.dpi));
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                if (SpawningOptionsTabProp.intValue == 0)
                {
                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                    EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);
                    GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                    EditorGUILayout.HelpBox("Controls the type of method that Curx uses to get your player.", MessageType.None);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(PlayerTransformTypeProp, new GUIContent("Player Transform Type"));
                    CustomHelpLabelField("The Player Transform Type is the transform that Crux will use as a reference point for spawning objects.", true);

                    if (t.PlayerTransformType == CruxSystem.PlayerTransformTypeEnum.Stardard)
                    {
                        EditorGUILayout.PropertyField(PlayerTransformProp, new GUIContent("Player Transform"));
                        CustomHelpLabelField("The Standard Player Transform Type will allow you to manually set the player transform.", false);

                        if (t.m_PlayerObject == null)
                        {
                            EditorGUILayout.HelpBox("You need to assign your Player's tranform to the Player Transform slot. Crux will be disabled during runtime if no transform is assigned.", MessageType.Error, true);
                        }
                    }

                    if (t.PlayerTransformType == CruxSystem.PlayerTransformTypeEnum.Instantiated)
                    {
                        t.PlayerTransformName = EditorGUILayout.TextField("Player Name", t.PlayerTransformName);
                        CustomHelpLabelField("The Instantiated Player Transform Type will have Crux automatically assign the player transform using your player's game object name. This option is intended for players that are created on Start. You can assign the player's name in the text box below.", false);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                    EditorGUILayout.LabelField("Spawning Settings", EditorStyles.boldLabel);
                    GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                    EditorGUILayout.HelpBox("Controls the type of spawning techniques Crux will use.", MessageType.None);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    EditorGUI.BeginChangeCheck();
                    var layersSelection_TerrainInfo = EditorGUILayout.MaskField("Terrain Spawning Layer", LayerMaskToField(TerrainInfoLayerMaskProp.intValue), InternalEditorUtility.layers);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(t, "Layers changed");
                        TerrainInfoLayerMaskProp.intValue = FieldToLayerMask(layersSelection_TerrainInfo);
                    }
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("The layer mask used for spawning on the terrain. This should be the layer that your terrain is using. This feature is also useful to " +
                        "exclude layers that you do not want to be spawned on such as in water or on rocks. Note: The Terrain Info also uses this layer.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(SpawnTypeProp, new GUIContent("Spawn Type"));

                    if (t.SpawnType == CruxSystem.SpawnTypeEnum.Node)
                    {
                        CustomHelpLabelField("The Nodes Spawn Type option will spawn all object groups to nodes. If the player is out of the customizable range, the objects within the said node will be deactivated until the player is within range. This allows objects to stay inactive when not close enough to help increase performance.", false);
                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();

                        CustomIntSlider(new Rect(), new GUIContent(), SpawnNodeDeactivateDistanceProp, "Deactivate Distance", 10, (int)t.m_DespawnRadius - 25);
                        CustomHelpLabelField("The distance that the objects will deactivate when using spawning nodes. Anything less than the Deactivate Distance will set the objects as inactive. When the objects have reached Crux's Despawn Radius, they will be despawned.", false);

                        EditorGUILayout.Space();
                        CustomFloatSlider(new Rect(), new GUIContent(), SpawnNodeUpdateFrequencyProp, "Node Update Frequency", 0.1f, 2.0f);
                        CustomHelpLabelField("The Node Update Frequency controls how often the Spawn Nodes are updated to check for the player's distance to be activated.", false);

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (t.SpawnType == CruxSystem.SpawnTypeEnum.Stardard)
                    {
                        CustomHelpLabelField("The Standard Spawn Type option will simply spawn objects around the player. They will remain active as long as they don't leave Crux's despawn radius. This option is helpful for those who want objects to continue to be active even though they might not be visible or near the player.", false);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(UseLayerDetectionProp, new GUIContent("Use Object Detection"));
                    CustomHelpLabelField("Controls spawning objects by checking the area for previously spawned objects of the appropriate layer. This is to avoiding spawning new objects " +
                        "too close to previously spawn objects and keeps objects evenly distributed. This layer can be any layer that is desired other than the Default layer. " +
                        "If the Default layer is used, this feature will be disabled.", true);

                    if (t.UseLayerDetection == CruxSystem.UseLayerDetectionEnum.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();

                        EditorGUI.BeginChangeCheck();
                        var layersSelection = EditorGUILayout.MaskField("Layer Detection Layers", LayerMaskToField(AILayerMaskProp.intValue), InternalEditorUtility.layers);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(t, "Layers changed");
                            AILayerMaskProp.intValue = FieldToLayerMask(layersSelection);
                        }
                        CustomHelpLabelField("The layers that Crux will use when using Layer Detection Spawning.", true);

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();


                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                    EditorGUILayout.LabelField("Spawning Parameters", EditorStyles.boldLabel);
                    GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                    EditorGUILayout.HelpBox("Controls various settings related to spawning sizes and total allowed objects.", MessageType.None);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    CustomFloatSlider(new Rect(), new GUIContent(), UpdateFrequencyProp, "Update Frequency", 0.1f, 30.0f);
                    CustomHelpLabelField("The Update Frequency is how often the system will attempt to spawn an object.", false);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomIntSlider(new Rect(), new GUIContent(), MinRadiusProp, "Min Radius", 10, 600);
                    CustomHelpLabelField("The Min Radius is the closest allowed spawning position with the player's radius. Objects can be spawned " +
                        "anywhere outside of this radius until the Max Radius is met. This is used from keeping objects spawning too close to your player.", false);

                    CustomIntSlider(new Rect(), new GUIContent(), MaxRadiusProp, "Max Radius", 100, 1200);
                    CustomHelpLabelField("The Max Radius is the max allowed spawning position with the player's radius. Note: This distance must be kept smaller than the Despawning Radius.", false);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomFloatSlider(new Rect(), new GUIContent(), DespawnRadiusProp, "Despawn Radius", 150.0f, 2500.0f);
                    CustomHelpLabelField("The Despawn Radius determines when the system will despawn an objects.", false);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomIntSlider(new Rect(), new GUIContent(), MaxObjectsProp, "Max Object", 0, 50);
                    CustomHelpLabelField("The Max Objects determines the max amount of objects the system can spawn at one time. If the max value is reached, the system will not spawn another object until an object has been despanwed dropping the current amount of objects spawned.", false);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomIntSlider(new Rect(), new GUIContent(), StartingAmountProp, "Starting Amount", 0, 50);
                    CustomHelpLabelField("The Starting Amount determines how many object will be spawned on start around your player.", false);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomIntSlider(new Rect(), new GUIContent(), MinSpawnSteepnessProp, "Min Spawning Steepness", 0, 90);
                    CustomHelpLabelField("Controls the minimum terrain steepness your objects can spawn at.", false);

                    CustomIntSlider(new Rect(), new GUIContent(), MaxSpawnSteepnessProp, "Max Spawning Steepness", 0, 90);
                    CustomHelpLabelField("Controls the maximum terrain steepness your objects can spawn at.", false);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.EndVertical();

                }

                if (SpawningOptionsTabProp.intValue == 1)
                {
                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                    EditorGUILayout.LabelField("Spawn ID Options", EditorStyles.boldLabel);
                    GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                    EditorGUILayout.HelpBox("The Spawn ID system generates Spawn IDs for each object created with Crux. These Spawn IDs can then be used for spawning objects at the player's location. This is useful for testing and development. These spawning IDs can also be written to a txt file or regenerated using the buttons below.", MessageType.None, true);
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(UseSpawnIDsProp, new GUIContent("Use Spawn IDs"));
                    CustomHelpLabelField("Controls whether or not users will have the ability to spawn objects around the player using the object's spawn IDs.", true);

                    if (t.UseSpawnIDs == CruxSystem.UseSpawnIDsEnum.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.PropertyField(CruxSpawnIDKeyProp, new GUIContent("Spawn ID Menu Key"));
                        CustomHelpLabelField("Controls what key is needed to toggle the Spawn by ID Menu.", false);

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomHelpLabelField("Save all IDs to txt will save all objects' IDs to a txt file along with their Name, Category, and Biome.", false);

                    if (GUILayout.Button("Save all IDs to txt"))
                    {
                        if (t.SpawnIDsFileName.Contains(".txt"))
                        {
                            t.SpawnIDsFileName = t.SpawnIDsFileName.Replace(".txt", "");
                        }

                        t.SpawnIDsFilePath = EditorUtility.SaveFilePanelInProject("Save as txt", "Crux Spawn IDs", "txt", "Please enter a name to save the file to");

                        if (t.SpawnIDsFilePath != string.Empty)
                        {
                            SaveSpawnIDsToTxt();
                            AssetDatabase.Refresh();
                        }
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                }

                if (SpawningOptionsTabProp.intValue == 2)
                {
                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                    EditorGUILayout.LabelField("Documentation", EditorStyles.boldLabel);
                    GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                    EditorGUILayout.HelpBox("Crux's documentation can be found using the Documentation button below. The documentation is done online so it is always " +
                        "update to date. The tutorial videos are also a great way to help you get the most out of Crux with useful guides on how to use the system.", MessageType.None, true);
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomHelpLabelField("Online Documentation", false);
                    if (GUILayout.Button("Documentation"))
                    {
                        Application.OpenURL("https://docs.google.com/document/d/1Ee0eqh9kzGXK_2IrcXBL3KpujmGctSgPkFkLB4HjR4w/edit?usp=sharing");
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomHelpLabelField("Online Tutorial Videos", false);
                    if (GUILayout.Button("Tutorial Videos"))
                    {
                        Application.OpenURL("https://www.youtube.com/playlist?list=PLlyiPBj7FznasueQUe7_51nNvGkUuu7B1");
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            if (TabNumberTop_SP.intValue == 2)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                var style_Settings = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(TerrainInfoIcon), style_Settings, GUILayout.ExpandWidth(true), GUILayout.Height(40));
                EditorGUILayout.LabelField("Terrain Info", style_Settings, GUILayout.ExpandWidth(true));
                GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                EditorGUILayout.HelpBox("Press and hold the Left Control key while using your mouse to determine the point you'd like detected. " +
                    "The terrain information will be updated below. If it's not updating, you may need to right click to have the scene view become active.", MessageType.None);
                EditorGUILayout.HelpBox("The Terrain Info allows you to easily detect the slope and height of your terrain.", MessageType.None);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(TerrainTypeProp, new GUIContent("Terrain Type", "Controls what type of terrain Crux will use to spawn objects."));
                if (t.TerrainType == CruxSystem.TerrainTypeEnum.MeshTerrain)
                {
                    GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                    EditorGUILayout.HelpBox("Note: The Mesh Terrain Type does not support biome spawning as it is not currently possible to lookup the texture on a mesh terrain's surface." +
                        " When using this option, only the first biome will be used for spawning.", MessageType.None);
                    GUI.backgroundColor = Color.white;
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUI.BeginChangeCheck();
                var layersSelection_TerrainInfo = EditorGUILayout.MaskField("Terrain Info Mask", LayerMaskToField(TerrainInfoLayerMaskProp.intValue), InternalEditorUtility.layers);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(t, "Layers changed");
                    TerrainInfoLayerMaskProp.intValue = FieldToLayerMask(layersSelection_TerrainInfo);
                }
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("The layer mask for detecting the terrain's height and angle. It is recommended that the terrain has its own layer so other objects don't interfere" +
                    " with the detection process.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                var Style_CruxTools = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                CruxTopTabsStlye.fontSize = 14;
                string TerrainHeight = System.String.Format("{0:F1}", CurrentTerrainHeight);
                EditorGUILayout.LabelField(new GUIContent("Terrain Elevation: " + TerrainHeight + "m", HeightIcon), Style_CruxTools, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                string TerrainAngle = System.String.Format("{0:F1}", CurrentTerrainAngle);
                EditorGUILayout.LabelField(new GUIContent("Terrain Angle: " + TerrainAngle + "°", AngleIcon), Style_CruxTools, GUILayout.ExpandWidth(true), GUILayout.Height(32));

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            if (TabNumberTop_SP.intValue == 0)
            {
                EditorGUILayout.Space();
                RepaintedSceneView = false;

                EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));

                var style_Biome = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(BiomeIcon), style_Biome, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                EditorGUILayout.LabelField("Biome Management", style_Biome, GUILayout.ExpandWidth(true));
                GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                EditorGUILayout.HelpBox("The Biome Management section allows you to manage all of your Biomes, create new Biomes, and assign objects to each spawning category. " +
                    "Below you can create a Biome for each environment type. These Biomes allow you to choose exactly what " +
                    "Wildlife, Creatures, and NPCs will spawn according to the textures for the environment.", MessageType.None);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Create Biome", EditorStyles.boldLabel);
                GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                EditorGUILayout.HelpBox("Create New Biome will create a new Biome. You will need to assign the textures from your terrain that defines the newly created Biome.", MessageType.None);
                GUI.backgroundColor = Color.white;

                GUI.backgroundColor = new Color(0f, 0.6f, 0f, 0.8f);
                var CreateBiomeStyle = new GUIStyle(GUI.skin.button);
                CreateBiomeStyle.normal.textColor = Color.white;
                CreateBiomeStyle.fontStyle = FontStyle.Bold;
                if (GUILayout.Button("Create New Biome", CreateBiomeStyle, GUILayout.Height(25)))
                {
                    Biome.arraySize++;
                    BiomeList.arraySize++;

                    Biome = GetTarget.FindProperty("Biome");
                    BiomeList = GetTarget.FindProperty("BiomeList");
                    GetTarget.ApplyModifiedProperties();

                    t.BiomeList[t.BiomeList.Count - 1].WildlifeObjects.Clear();
                    t.BiomeList[t.BiomeList.Count - 1].CreatureObjects.Clear();
                    t.BiomeList[t.BiomeList.Count - 1].NPCObjects.Clear();

                    //Reset our values
                    t.Biome[t.Biome.Count - 1].CategoryName = "New Biome";
                    t.Biome[t.Biome.Count - 1].TerrainTextures.Clear();
                    t.Biome[t.Biome.Count - 1].BiomeColor = Color.white;
                    t.Biome[t.Biome.Count - 1].BiomeIcon = Resources.Load("DefaultBiomeIcon") as Texture;
                    t.Biome[t.Biome.Count - 1].WildlifeOdds = 75;
                    t.Biome[t.Biome.Count - 1].WildlifeGenerationType = 1;
                    t.Biome[t.Biome.Count - 1].CreaturesOdds = 20;
                    t.Biome[t.Biome.Count - 1].CreaturesGenerationType = 1;
                    t.Biome[t.Biome.Count - 1].NPCOdds = 5;
                    t.Biome[t.Biome.Count - 1].NPCGenerationType = 1;

                    WildlifeList.Clear();
                    CreatureList.Clear();
                    NPCList.Clear();

                    UpdateReorderableListsAddButton();
                }
                GUI.backgroundColor = Color.white;

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                if (t.Biome.Count > 0)
                {
                    EditorGUILayout.LabelField("Biome List", EditorStyles.boldLabel);
                    GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                    EditorGUILayout.HelpBox("A list of all created biomes. You can select each biome to adjust its settings and define its textures.", MessageType.None);
                    GUI.backgroundColor = Color.white;
                }
                else
                {
                    EditorGUILayout.LabelField("Biome List", EditorStyles.boldLabel);
                    GUI.backgroundColor = new Color(1f, 0, 0, 0.7f);
                    EditorGUILayout.HelpBox("No Biomes have been created. To creae one, press the 'Create New Biome' button.", MessageType.None);
                    GUI.backgroundColor = Color.white;
                }
                CategoryNames = new string[t.Biome.Count];

                for (int l = 0; l < t.Biome.Count; l++)
                {
                    CategoryNames[l] = t.Biome[l].CategoryName;
                }

                CategoryTabNumber_SP.intValue = GUILayout.SelectionGrid(CategoryTabNumber_SP.intValue, CategoryNames, 2);

                EditorGUILayout.Space();


                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();

                for (int i = 0; i < Biome.arraySize; i++)
                {
                    SerializedProperty BiomeRef = Biome.GetArrayElementAtIndex(i);
                    CruxSystem.CruxClass m_BiomeList = t.Biome[i];
                    SerializedProperty TabNumber_SP = BiomeRef.FindPropertyRelative("TabNumber");

                    if (CategoryTabNumber_SP.intValue == i)
                    {
                        if (t.TerrainType == CruxSystem.TerrainTypeEnum.MeshTerrain && i != 0 && i > 0)
                        {
                            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.35f);
                            EditorGUILayout.HelpBox("The Mesh Terrain Type only supports the first biome and does not support texture spawning. " +
                                " This is due to Unity's current inability to get the texture of Mesh Terrains. To use biomes and texture spawning, " +
                                "you must use Unity Terrains. This can be adjusted under the Settings tab in the Terrain Info section.", MessageType.Warning);
                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.EndVertical();
                        }

                        GUI.backgroundColor = m_BiomeList.BiomeColor;

                        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));

                        EditorGUI.BeginDisabledGroup(t.TerrainType == CruxSystem.TerrainTypeEnum.MeshTerrain && i != 0 && i > 0);

                        GUI.backgroundColor = Color.white;
                        var BiomeStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                        BiomeStyle.fontSize = 15;
                        EditorGUILayout.LabelField(new GUIContent(m_BiomeList.BiomeIcon), BiomeStyle, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                        EditorGUILayout.LabelField(m_BiomeList.CategoryName, BiomeStyle, GUILayout.ExpandWidth(true));
                        GUI.backgroundColor = m_BiomeList.BiomeColor;
                        EditorGUILayout.Space();

                        EditorGUILayout.Space();
                        GUI.backgroundColor = Color.white;
                        m_BiomeList.CategoryName = EditorGUILayout.TextField("Biome Name ", m_BiomeList.CategoryName);
                        EditorGUILayout.HelpBox("The Biome Name defines the name of this Biome.", MessageType.None);
                        GUI.backgroundColor = m_BiomeList.BiomeColor;

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();
                        //GUILayout.FlexibleSpace();
                        m_BiomeList.BiomeIcon = (Texture)EditorGUILayout.ObjectField(m_BiomeList.CategoryName + " Biome Icon", m_BiomeList.BiomeIcon, typeof(Texture), false);
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();


                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        GUI.backgroundColor = Color.white;
                        m_BiomeList.BiomeColor = EditorGUILayout.ColorField(m_BiomeList.CategoryName + " Biome Editor Color", m_BiomeList.BiomeColor);
                        GUI.backgroundColor = m_BiomeList.BiomeColor;

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();

                        //Terrain Textures
                        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));

                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.LabelField(m_BiomeList.CategoryName + " Biome" + " Texture Options", EditorStyles.boldLabel);
                        EditorGUILayout.HelpBox("The Texture Options allow you to pick which textures define a Biome. When the system " +
                            "detects these textures, it will spawn an object accordingly.", MessageType.None, true);
                        GUI.backgroundColor = m_BiomeList.BiomeColor;

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        if (m_BiomeList.TerrainTextures != null)
                        {
                            if (m_BiomeList.TerrainTextures.Count > 0)
                            {
                                int totalTextures = 0;

                                GUI.backgroundColor = Color.white;
                                for (int j = 0; j < m_BiomeList.TerrainTextures.Count; ++j)
                                {
                                    if (totalTextures == 0)
                                    {
                                        GUILayout.BeginHorizontal(GUILayout.MinHeight(100));
                                        GUILayout.Space(50);
                                    }

                                    GUILayout.BeginVertical(GUILayout.MinHeight(100));
                                    m_BiomeList.TerrainTextures[j] = (Texture)EditorGUILayout.ObjectField(m_BiomeList.TerrainTextures[j], typeof(Texture), false,
                                        GUILayout.MinWidth(50), GUILayout.MaxWidth(75), GUILayout.MinHeight(50), GUILayout.MaxHeight(75));

                                    if (GUILayout.Button("Remove", GUILayout.MinWidth(50), GUILayout.MaxWidth(75)))
                                    {
                                        m_BiomeList.TerrainTextures.RemoveAt(j);
                                        --j;
                                    }

                                    GUILayout.EndVertical();
                                    totalTextures++;

                                    if (totalTextures == 4 || j == m_BiomeList.TerrainTextures.Count - 1)
                                    {
                                        GUILayout.Space(30);

                                        GUILayout.EndHorizontal();
                                        totalTextures = 0;
                                    }
                                }
                            }
                            GUI.backgroundColor = m_BiomeList.BiomeColor;


                            if (m_BiomeList.TerrainTextures.Count == 0)
                            {
                                GUI.backgroundColor = Color.white;
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                EditorGUILayout.HelpBox("There are currently no textures for this Biome.", MessageType.Info);
                                GUILayout.Space(20);
                                GUILayout.EndHorizontal();
                                GUI.backgroundColor = m_BiomeList.BiomeColor;
                            }

                            GUI.backgroundColor = Color.white;
                            GUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Add Texture", GUILayout.MinWidth(90), GUILayout.MaxWidth(90)))
                            {
                                Texture newTexture = null;
                                m_BiomeList.TerrainTextures.Add(newTexture);
                            }
                            GUILayout.EndHorizontal();
                            GUI.backgroundColor = m_BiomeList.BiomeColor;
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.LabelField(m_BiomeList.CategoryName + " Biome" + " Object Options", EditorStyles.boldLabel);
                        EditorGUILayout.HelpBox("The Object Options allow you to pick which objects spawn for this Biome. If the system detects any of the above textures, it will spawn an object according to the conditions below.", MessageType.None, true);
                        GUI.backgroundColor = m_BiomeList.BiomeColor;
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        GUI.backgroundColor = Color.white;
                        GUIContent[] TabButtons = new GUIContent[3] { new GUIContent(" Wildlife", WildlifeIcon), new GUIContent(" Creatures", CreatureIcon), new GUIContent(" NPC/Other", NPCIcon) };
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        TabNumber_SP.intValue = GUILayout.SelectionGrid(TabNumber_SP.intValue, TabButtons, 3, GUILayout.Height(28), GUILayout.Width(88f * Screen.width / Screen.dpi));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                        GUI.backgroundColor = m_BiomeList.BiomeColor;

                        EditorGUILayout.Space();

                        //Wildlife Objects
                        if (TabNumber_SP.intValue == 0)
                        {
                            GUI.backgroundColor = Color.white;
                            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                            EditorGUILayout.LabelField(new GUIContent(WildlifeIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                            EditorGUILayout.LabelField("Wildlife Options", style, GUILayout.ExpandWidth(true));
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
                            EditorGUILayout.HelpBox("Wildlife objects are typically animals that can be passive or aggressive. Below you can adjust the settings " +
                                "for any object within this category as well as adjusting settings for each individual object.", MessageType.None, true);
                            GUI.backgroundColor = m_BiomeList.BiomeColor;
                            EditorGUILayout.Space();

                            GUI.backgroundColor = Color.white;
                            m_BiomeList.WildlifeOdds = EditorGUILayout.IntSlider("Wildlife Odds", m_BiomeList.WildlifeOdds, 0, 100);
                            EditorGUILayout.HelpBox("Creature Odds controls the odds of the Wildlife Category objects spawning in the " + m_BiomeList.CategoryName + " Biome.", MessageType.None, true);
                            GUI.backgroundColor = m_BiomeList.BiomeColor;
                            GUILayout.Space(10);

                            GUI.backgroundColor = Color.white;
                            GenerationTypeEnum = (GenerationType)m_BiomeList.WildlifeGenerationType;
                            GenerationTypeEnum = (GenerationType)EditorGUILayout.EnumPopup("Generation Type", GenerationTypeEnum);
                            m_BiomeList.WildlifeGenerationType = (int)GenerationTypeEnum;
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.HelpBox("The Generation Type determines if your object is generated by odds or by random. The Random setting will generate any object within the list below by random. The Odds setting will allow users to choose the odds of each object spawning.", MessageType.None);
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            GUILayout.Space(15);

                            GUI.backgroundColor = Color.white;
                            WildlifeList[i].DoLayoutList();
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            EditorGUILayout.Space();
                        }


                        if (TabNumber_SP.intValue == 1)
                        {
                            GUI.backgroundColor = Color.white;
                            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                            EditorGUILayout.LabelField(new GUIContent(CreatureIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                            EditorGUILayout.LabelField("Creature Options", style, GUILayout.ExpandWidth(true));
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
                            EditorGUILayout.HelpBox("Creature objects are typically non-animal and are usually aggressive. Below you can adjust the settings for " +
                                "any object within this category as well as adjusting settings for each individual object.", MessageType.None, true);
                            GUI.backgroundColor = m_BiomeList.BiomeColor;
                            EditorGUILayout.Space();

                            GUI.backgroundColor = Color.white;
                            m_BiomeList.CreaturesOdds = EditorGUILayout.IntSlider("Creature Odds", m_BiomeList.CreaturesOdds, 0, 100);
                            EditorGUILayout.HelpBox("Creature Odds controls the odds of the Creature Category objects spawning in the " + m_BiomeList.CategoryName + " Biome.", MessageType.None, true);
                            GUI.backgroundColor = m_BiomeList.BiomeColor;
                            GUILayout.Space(10);

                            GUI.backgroundColor = Color.white;
                            GenerationTypeEnum = (GenerationType)m_BiomeList.CreaturesGenerationType;
                            GenerationTypeEnum = (GenerationType)EditorGUILayout.EnumPopup("Generation Type", GenerationTypeEnum);
                            m_BiomeList.CreaturesGenerationType = (int)GenerationTypeEnum;
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.HelpBox("The Generation Type determines if your object is generated by odds or by random. The Random setting will generate any object within the list below by random. The Odds setting will allow users to choose the odds of each object spawning.", MessageType.None);
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            GUILayout.Space(15);

                            GUI.backgroundColor = Color.white;
                            CreatureList[i].DoLayoutList();
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            EditorGUILayout.Space();
                        }

                        //NPC
                        if (TabNumber_SP.intValue == 2)
                        {
                            GUI.backgroundColor = Color.white;
                            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                            EditorGUILayout.LabelField(new GUIContent(NPCIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                            EditorGUILayout.LabelField("NPC/Other Options", style, GUILayout.ExpandWidth(true));
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
                            EditorGUILayout.HelpBox("NPC objects are typically hunmaniod and can be passive or aggressive. However, any object can be used, even non-AI object such as collectable " +
                                "plants or materials. Below you can adjust the settings for any object within this category as well as adjusting settings for each individual object.", MessageType.None, true);
                            GUI.backgroundColor = m_BiomeList.BiomeColor;
                            EditorGUILayout.Space();

                            GUI.backgroundColor = Color.white;
                            m_BiomeList.NPCOdds = EditorGUILayout.IntSlider("NPC/Other Odds", m_BiomeList.NPCOdds, 0, 100);
                            EditorGUILayout.HelpBox("NPC/Other Odds controls the odds of the NPC Category objects spawning in the " + m_BiomeList.CategoryName + " Biome.", MessageType.None, true);
                            GUI.backgroundColor = m_BiomeList.BiomeColor;
                            GUILayout.Space(10);

                            GUI.backgroundColor = Color.white;
                            GenerationTypeEnum = (GenerationType)m_BiomeList.NPCGenerationType;
                            GenerationTypeEnum = (GenerationType)EditorGUILayout.EnumPopup("Generation Type", GenerationTypeEnum);
                            m_BiomeList.NPCGenerationType = (int)GenerationTypeEnum;
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.HelpBox("The Generation Type determines if your object is generated by odds or by random. The Random setting will generate any object within the list below by random. The Odds setting will allow users to choose the odds of each object spawning.", MessageType.None);
                            GUI.backgroundColor = m_BiomeList.BiomeColor;
                            GUILayout.Space(15);

                            GUI.backgroundColor = Color.white;
                            NPCList[i].DoLayoutList();
                            GUI.backgroundColor = m_BiomeList.BiomeColor;

                            EditorGUILayout.Space();
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();
                        EditorGUI.EndDisabledGroup();

                        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90f * Screen.width / Screen.dpi));
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.LabelField("Remove " + m_BiomeList.CategoryName + " Biome", EditorStyles.boldLabel);
                        EditorGUILayout.HelpBox("The button below will delete the " + m_BiomeList.CategoryName + " Biome.", MessageType.None, true);
                        GUI.backgroundColor = new Color(1, 0, 0, 0.8f);

                        var RemoveBiomeStyle = new GUIStyle(GUI.skin.button);
                        RemoveBiomeStyle.normal.textColor = Color.white;
                        RemoveBiomeStyle.fontStyle = FontStyle.Bold;

                        if (GUILayout.Button("Remove " + m_BiomeList.CategoryName + " Biome", RemoveBiomeStyle, GUILayout.Height(25)))
                        {
                            if (EditorUtility.DisplayDialog("Remove Biome", "Are you sure you remove this biome? This process cannot be undone.", "Yes", "Cancel"))
                            {
                                Biome.DeleteArrayElementAtIndex(i);
                                BiomeList.DeleteArrayElementAtIndex(i);
                                t.Biome.RemoveAt(i);
                                t.BiomeList.RemoveAt(i);
                                CategoryTabNumber_SP.intValue = i - 1;
                                UpdateReorderableLists();
                            }
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();

                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            if (GUI.changed && !EditorApplication.isPlaying)
            {
                EditorUtility.SetDirty(t);
            }

            GetTarget.ApplyModifiedProperties();

        }

        public static bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style)
        {
            Rect position = GUILayoutUtility.GetRect(40f, 40f, 16f, 16f, style);
            return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
        }

        public static bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style)
        {
            return Foldout(foldout, new GUIContent(content), toggleOnLabelClick, style);
        }

        // Converts the field value to a LayerMask
        private LayerMask FieldToLayerMask(int field)
        {
            LayerMask mask = 0;
            var layers = InternalEditorUtility.layers;
            for (int c = 0; c < layers.Length; c++)
            {
                if ((field & (1 << c)) != 0)
                {
                    mask |= 1 << LayerMask.NameToLayer(layers[c]);
                }
            }
            return mask;
        }
        // Converts a LayerMask to a field value
        private int LayerMaskToField(LayerMask mask)
        {
            int field = 0;
            var layers = InternalEditorUtility.layers;
            for (int c = 0; c < layers.Length; c++)
            {
                if ((mask & (1 << LayerMask.NameToLayer(layers[c]))) != 0)
                {
                    field |= 1 << c;
                }
            }
            return field;
        }

        void CustomHelpLabelField(string TextInfo, bool UseSpace)
        {
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField(TextInfo, EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            if (UseSpace)
            {
                EditorGUILayout.Space();
            }
        }

        public void CustomIntSlider(Rect position, GUIContent label, SerializedProperty property, string Name, int MinValue, int MaxValue)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.IntSlider(Name, property.intValue, MinValue, MaxValue);
            if (EditorGUI.EndChangeCheck())
                property.intValue = newValue;

            EditorGUI.EndProperty();
        }

        public void CustomFloatSlider(Rect position, GUIContent label, SerializedProperty property, string Name, float MinValue, float MaxValue)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.Slider(Name, property.floatValue, MinValue, MaxValue);
            if (EditorGUI.EndChangeCheck())
                property.floatValue = newValue;

            EditorGUI.EndProperty();
        }

        void OnSceneGUI()
        {
            //Repaint the scene view because the Event won't trigger unless the scene view is moved (Unity bug work around)
            if (!RepaintedSceneView && TabNumberTop_SP.intValue == 1)
            {
                EditorWindow view = EditorWindow.GetWindow<SceneView>();
                view.Repaint();
                RepaintedSceneView = true;
            }

            CruxSystem self = (CruxSystem)target;
            Event e = Event.current;

            if (e.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == (KeyCode.LeftControl))
                {
                    RaycastHit hit;
                    Vector3 mousePosition = Event.current.mousePosition;
                    Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
                    mousePosition = ray.origin;

                    if (Physics.Raycast(ray, out hit, 10000, self.TerrainInfoLayerMask))
                    {
                        if (hit.collider != null)
                        {
                            CurrentTerrainHeight = Mathf.Round(hit.point.y * 10f) / 10f;
                            float ReceivedTerrainAngle = Vector3.Angle(Vector3.up, hit.normal);
                            CurrentTerrainAngle = Mathf.Round(ReceivedTerrainAngle * 10f) / 10f;
                            Repaint();
                        }
                    }
                }
            }

            if (self.m_PlayerObject != null)
            {
                Handles.color = self.MinSpawnRadiusColor;
                Handles.DrawWireDisc(self.m_PlayerObject.position, Vector3.up, self.m_MinRadius);

                Handles.color = self.MaxSpawnRadiusColor;
                Handles.DrawWireDisc(self.m_PlayerObject.position, Vector3.up, self.m_MaxRadius);

                Handles.color = self.DespawnRadiusColor;
                Handles.DrawWireDisc(self.m_PlayerObject.position, Vector3.up, self.m_DespawnRadius);
            }
        }
    }
}
