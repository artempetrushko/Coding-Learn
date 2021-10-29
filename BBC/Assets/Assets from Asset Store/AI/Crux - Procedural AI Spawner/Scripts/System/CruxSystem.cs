using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Crux.Utility;

#if UNISTORM_PRESENT
using UniStorm;
#endif

namespace Crux
{
    [System.Serializable]
    public class CruxSystem : MonoBehaviour
    {
        public static CruxSystem Instance;

        public TerrainTypeEnum TerrainType = TerrainTypeEnum.UnityTerrain;
        public enum TerrainTypeEnum
        {
            UnityTerrain = 0,
            MeshTerrain
        }

        public UseLayerDetectionEnum UseLayerDetection = UseLayerDetectionEnum.Yes;
        public enum UseLayerDetectionEnum
        {
            Yes = 0,
            No
        }

        public UseSpawnIDsEnum UseSpawnIDs = UseSpawnIDsEnum.No;
        public enum UseSpawnIDsEnum
        {
            Yes = 0,
            No
        }

        public PlayerTransformTypeEnum PlayerTransformType = PlayerTransformTypeEnum.Stardard;
        public enum PlayerTransformTypeEnum
        {
            Stardard = 1,
            Instantiated = 2
        }

        public SpawnTypeEnum SpawnType = SpawnTypeEnum.Stardard;
        public enum SpawnTypeEnum
        {
            Stardard = 0,
            Node = 1
        }

        public KeyCode CruxSpawnIDKey = KeyCode.Escape;
        public int SpawningOptionsTab = 0;
        public float SpawnedHeight;
        public Texture m_Texture;
        public int RespawnIterations = 3;
        public GameObject SpawnNode;
        public int SpawnNodeDeactivateDistance = 300;
        public GameObject ObjectPool;
        public GameObject CurrentNode;
        public GameObject ChildrenHolder;
        public float SpawnNodeUpdateFrequency = 1;
        public LayerMask TerrainInfoLayerMask = 0;
        public LayerMask AILayerMask = 0;
        public int AILayerMaskDectectionDistance = 100;
        public float m_UpdateTickFrequency = 1;

        public Color MinSpawnRadiusColor = new Color32(255, 0, 0, 25);
        public Color MaxSpawnRadiusColor = new Color32(0, 233, 11, 25);
        public Color DespawnRadiusColor = new Color32(0, 76, 0255, 25);

        public Transform m_PlayerObject;
        public int StartingAIAmount = 10;
        public int m_CurrentSpawnedObjects;
        public int m_MaxObjectsToSpawn = 20;
        public int m_MinRadius = 150;
        public int m_MaxRadius = 300;
        public float m_DespawnRadius = 450;
        public TerrainInfo m_TerrainInfo;
        GameObject m_InstancedChecker;
        public Vector3 m_PositionToSpawn;
        Vector3 m_ReceivedHeight;
        public int minSteepness = 0;
        public int maxSteepness = 40;
        public int GroupAmount;

        public int TabNumberTop = 0;
        public int CategoryTabNumber = 0;
        public string PlayerTransformName = "Enter Player Name";

        public List<GameObject> m_SpawnedObjects = new List<GameObject>();
        public List<string> SpawnIDList = new List<string>();

        public string SpawnIDsFilePath = "D:/CruxSpawnIDs.txt";
        public string SpawnIDsFileName = "CruxSpawnIDs";
        public List<string> SpawnIDs = new List<string>();
        public string EnteredSpawnID;
        public int ObjectsPlaceInList;
        public bool writeSpawnIDsToTxtFile = false;

        //UI
        public Text InputText;
        public bool SpawnIDSystem = false;
        public GameObject SpawnIDMenu;
        public bool MenuToggle = false;
        public bool ObjectSkipped = false;

        public GameObject m_InstancedObject;

        [System.Serializable]
        public class CruxClass
        {
            public List<GameObject> WildlifeSpawnNodes = new List<GameObject>();

            public Color BiomeColor = Color.white;

            public string CategoryName = "New Biome";

            public bool collapse;
            public List<Texture> TerrainTextures = new List<Texture>();
            public Texture BiomeIcon;

            public List<CruxObject> WildlifeToSpawn = new List<CruxObject>();
            public int WildlifeOdds = 75;
            public List<CruxObject> CommonWildlife = new List<CruxObject>();
            public List<CruxObject> UncommonWildlife = new List<CruxObject>();
            public List<CruxObject> RareWildlife = new List<CruxObject>();
            public List<CruxObject> UltraRareWildlife = new List<CruxObject>();
            public List<string> WildlifeSpawnId = new List<string>();
            public int WildlifeGenerationType = 1;
            public int WildlifeCounter = 0;
            public CruxObject WildlifeObjectToSpawn;

            public List<CruxObject> CreaturesToSpawn = new List<CruxObject>();
            public int CreaturesOdds = 20;
            public List<CruxObject> CommonCreatures = new List<CruxObject>();
            public List<CruxObject> UncommonCreatures = new List<CruxObject>();
            public List<CruxObject> RareCreatures = new List<CruxObject>();
            public List<CruxObject> UltraRareCreatures = new List<CruxObject>();
            public List<string> CreaturesSpawnId = new List<string>();
            public int CreaturesGenerationType = 1;
            public int CreaturesCounter = 0;
            public CruxObject CreaturesObjectToSpawn;

            public List<CruxObject> NPCToSpawn = new List<CruxObject>();
            public int NPCOdds = 5;
            public List<CruxObject> CommonNPC = new List<CruxObject>();
            public List<CruxObject> UncommonNPC = new List<CruxObject>();
            public List<CruxObject> RareNPC = new List<CruxObject>();
            public List<CruxObject> UltraRareNPC = new List<CruxObject>();
            public List<string> NPCSpawnId = new List<string>();
            public int NPCGenerationType = 1;
            public int NPCCounter = 0;
            public CruxObject NPCObjectToSpawn;

            public int TabNumber = 0;
            public enum RarityEnum { Common, Uncommon, Rare, UltraRare };
            public enum HeightTypeEnum { Land = 1, Air = 2, Water = 3 };
            public enum GroupEnum { Yes, No };
            public enum UniStormSpawnTimeEnum { Morning = 1, Day, Evening, Night };

            public int RandomObject;
        }

        [System.Serializable]
        public class SpawnableObjects
        {
            [SerializeField]
            public List<CruxObject> WildlifeObjects = new List<CruxObject>();
            [SerializeField]
            public List<CruxObject> CreatureObjects = new List<CruxObject>();
            [SerializeField]
            public List<CruxObject> NPCObjects = new List<CruxObject>();
        }

        [SerializeField]
        public List<SpawnableObjects> BiomeList = new List<SpawnableObjects>(1);

        //This is our list we want to use to represent our class as an array.
        [SerializeField]
        public List<CruxClass> Biome = new List<CruxClass>(1);

        public bool UniStormPresent = false;

        //Create our Terrain Checker component. This object is what gets and sends all the spawning information.
        void Awake()
        {
            Instance = this;
            m_InstancedChecker = (GameObject)Instantiate(Resources.Load("Terrain Info"), new Vector3(0, 0, 0), Quaternion.identity);
            SpawnNode = Resources.Load("Crux Spawn Node") as GameObject;

            m_TerrainInfo = m_InstancedChecker.GetComponent<TerrainInfo>();

            if (Terrain.activeTerrain == null && TerrainType != TerrainTypeEnum.MeshTerrain)
            {
                Debug.LogError("No terrain could be found, Crux has been disabled. Please ensure you have an active terrain in your scene. " +
                    "If your terrain is being procedurally built, keep Crux inactive until it has finished building. If you are using a Mesh Terrain, " +
                    "ensure you are using the Mesh Terrain Setting located under the Terrain Info tab.");
                m_TerrainInfo = null;
            }

            if (Biome.Count == 0)
            {
                Debug.LogError("You currently have 0 biomes. Crux needs at least 1 biome in order to work. To creat one, go to the Biome Options, " +
                    "press the 'Add Biome' button, and create an object for it. Ensure that you have also assigned textures to define your biome.");
            }

            for (int i = 0; i < Biome.Count; i++)
            {
                if (Biome[i].TerrainTextures.Count == 0 && TerrainType != TerrainTypeEnum.MeshTerrain)
                {
                    Debug.LogWarning("Your " + Biome[i].CategoryName + " biome has 0 textures. Please assign at least 1 texture to this biome " +
                        "by pressing the 'Add Texture' button and assigning a texture from your terrain to it. This will be used to spawn " +
                        "objects to the portions of your terrain with this texture and any others you apply.");
                }
            }

            //Create Crux's Object Pool
            ObjectPool = new GameObject();
            ObjectPool.transform.SetParent(transform);
            ObjectPool.name = "(Crux) Object Pool";

            if (TerrainType == TerrainTypeEnum.MeshTerrain)
            {
                m_TerrainInfo.TerrainType = TerrainInfo.TerrainTypeEnum.MeshTerrain;
            }
            else if (TerrainType == TerrainTypeEnum.UnityTerrain && m_TerrainInfo != null)
            {
                m_TerrainInfo.TerrainType = TerrainInfo.TerrainTypeEnum.UnityTerrain;
            }

            if (m_TerrainInfo != null && TerrainType != TerrainTypeEnum.MeshTerrain)
            {
                m_TerrainInfo.terrain = Terrain.activeTerrain;
                m_TerrainInfo.terrainData = m_TerrainInfo.terrain.terrainData;
                m_TerrainInfo.terrainPos = m_TerrainInfo.terrain.transform.position;
                m_InstancedChecker.transform.parent = transform;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(CruxSpawnIDKey) && UseSpawnIDs == UseSpawnIDsEnum.Yes)
            {
                MenuToggle = !MenuToggle;
                SpawnIDMenu.SetActive(MenuToggle);
            }
        }

        IEnumerator SpawnCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_UpdateTickFrequency);
                Create();
            }
        }

        void Start()
        {
            //Look for UniStorm. If it's detected, set UniStormPresent to true.
#if UNISTORM_PRESENT
        if (FindObjectOfType<UniStormSystem>())
        {
            UniStormPresent = true;
            //Posibly add an option within the crux system to use UniStorm spawning?
        }
#endif

            StartCoroutine(InitializeCrux());
        }

        IEnumerator InitializeCrux()
        {
            //If using UniStorm
#if UNISTORM_PRESENT
        if (UniStormPresent)
        {
            yield return new WaitWhile(() => !UniStormSystem.Instance.UniStormInitialized);
        }
#endif
            //If using the Mesh Terrain Option, remove our biomes so only the first is remaining.
            if (TerrainType == TerrainTypeEnum.MeshTerrain)
            {
                for (int a = Biome.Count - 1; a > 0; a--)
                {
                    Biome.RemoveAt(a);
                    BiomeList.RemoveAt(a);
                }
            }

            AssignRarityCategories();

            //If using spawn IDs, create our UI components.
            //If you'd like to have your own custom UI for spawn IDs, this portion can be removed.
            if (UseSpawnIDs == UseSpawnIDsEnum.Yes)
            {
                GameObject CruxUI = Instantiate(Resources.Load("Crux UI")) as GameObject;

                foreach (Text T in CruxUI.GetComponentsInChildren<Text>())
                {
                    if (T.gameObject.name == "CruxInputText")
                    {
                        InputText = T;
                    }
                }

                Button SpawnButton = CruxUI.GetComponentInChildren<Button>();
                SpawnButton.onClick.AddListener(delegate { GetSpawnIDUI(); });

                if (GameObject.Find("EventSystem") == null)
                {
                    Instantiate(Resources.Load("CruxUIEventSystem"));
                }

                CruxUI.SetActive(false);
                SpawnIDMenu = CruxUI;
            }

            if (PlayerTransformType == PlayerTransformTypeEnum.Stardard)
            {
                if (m_PlayerObject == null)
                {
                    m_TerrainInfo = null;
                    Debug.LogError("Your Player Transform object has not been assigned, Crux has been disabled. You need to assign your player object to the Player Transform slot of the Crux Editor under Spawning Options. If your player is instantiated, use the Instantiated Player Transform Type.");
                }
            }
            else if (PlayerTransformType == PlayerTransformTypeEnum.Instantiated)
            {
                //If the instantiated player option is enabled, add a slight delay when looking for our instantiated player to ensure it has been spawned.
                yield return new WaitForSeconds(0.1f);
                m_PlayerObject = GameObject.Find(PlayerTransformName).transform;
            }

            if (m_TerrainInfo != null)
            {
                for (int i = 0; i < StartingAIAmount; i++)
                {
                    Create();
                }
            }

            StartCoroutine(SpawnCheck());
        }

        //Assigns all of our spawnanle objects to rarity categories using lists.
        //These lists are then used to spawn AI according to the generated odds and biomes.
        void AssignRarityCategories()
        {
            for (int i = 0; i < BiomeList.Count; i++)
            {
                //Wildlife
                for (int k = 0; k < BiomeList[i].WildlifeObjects.Count; k++)
                {
                    if (BiomeList[i].WildlifeObjects[k] != null)
                    {
                        if (BiomeList[i].WildlifeObjects[k].Rarity == CruxObject.RarityEnum.Common)
                        {
                            if (!Biome[i].CommonWildlife.Contains(BiomeList[i].WildlifeObjects[k]))
                            {
                                Biome[i].CommonWildlife.Add(BiomeList[i].WildlifeObjects[k]);
                                BiomeList[i].WildlifeObjects[k].CurrentPopulation = 0;
                            }
                        }
                        else if (BiomeList[i].WildlifeObjects[k].Rarity == CruxObject.RarityEnum.Uncommon)
                        {
                            if (!Biome[i].UncommonWildlife.Contains(BiomeList[i].WildlifeObjects[k]))
                            {
                                Biome[i].UncommonWildlife.Add(BiomeList[i].WildlifeObjects[k]);
                                BiomeList[i].WildlifeObjects[k].CurrentPopulation = 0;
                            }
                        }
                        else if (BiomeList[i].WildlifeObjects[k].Rarity == CruxObject.RarityEnum.Rare)
                        {
                            if (!Biome[i].RareWildlife.Contains(BiomeList[i].WildlifeObjects[k]))
                            {
                                Biome[i].RareWildlife.Add(BiomeList[i].WildlifeObjects[k]);
                                BiomeList[i].WildlifeObjects[k].CurrentPopulation = 0;
                            }
                        }
                        else if (BiomeList[i].WildlifeObjects[k].Rarity == CruxObject.RarityEnum.UltraRare)
                        {
                            if (!Biome[i].UltraRareWildlife.Contains(BiomeList[i].WildlifeObjects[k]))
                            {
                                Biome[i].UltraRareWildlife.Add(BiomeList[i].WildlifeObjects[k]);
                                BiomeList[i].WildlifeObjects[k].CurrentPopulation = 0;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("The Biome '" + Biome[i].CategoryName + "' is missing a Crux Object in the Wildlife Objects list. Please check where you " +
                            "are missing an object and remove it or assign a Crux Object.");
                    }
                }

                //Creatures
                for (int k = 0; k < BiomeList[i].CreatureObjects.Count; k++)
                {
                    if (BiomeList[i].CreatureObjects[k] != null)
                    {
                        if (BiomeList[i].CreatureObjects[k].Rarity == CruxObject.RarityEnum.Common)
                        {
                            if (!Biome[i].CommonCreatures.Contains(BiomeList[i].CreatureObjects[k]))
                            {
                                Biome[i].CommonCreatures.Add(BiomeList[i].CreatureObjects[k]);
                                BiomeList[i].CreatureObjects[k].CurrentPopulation = 0;
                            }
                        }
                        else if (BiomeList[i].CreatureObjects[k].Rarity == CruxObject.RarityEnum.Uncommon)
                        {
                            if (!Biome[i].UncommonCreatures.Contains(BiomeList[i].CreatureObjects[k]))
                            {
                                Biome[i].UncommonCreatures.Add(BiomeList[i].CreatureObjects[k]);
                                BiomeList[i].CreatureObjects[k].CurrentPopulation = 0;
                            }
                        }
                        else if (BiomeList[i].CreatureObjects[k].Rarity == CruxObject.RarityEnum.Rare)
                        {
                            if (!Biome[i].RareCreatures.Contains(BiomeList[i].CreatureObjects[k]))
                            {
                                Biome[i].RareCreatures.Add(BiomeList[i].CreatureObjects[k]);
                                BiomeList[i].CreatureObjects[k].CurrentPopulation = 0;
                            }
                        }
                        else if (BiomeList[i].CreatureObjects[k].Rarity == CruxObject.RarityEnum.UltraRare)
                        {
                            if (!Biome[i].UltraRareCreatures.Contains(BiomeList[i].CreatureObjects[k]))
                            {
                                Biome[i].UltraRareCreatures.Add(BiomeList[i].CreatureObjects[k]);
                                BiomeList[i].CreatureObjects[k].CurrentPopulation = 0;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("The Biome '" + Biome[i].CategoryName + "' is missing a Crux Object in the Creature Objects list. Please check where you " +
                            "are missing an object and remove it or assign a Crux Object.");
                    }
                }

                //NPC
                for (int k = 0; k < BiomeList[i].NPCObjects.Count; k++)
                {
                    if (BiomeList[i].NPCObjects[k] != null)
                    {
                        if (BiomeList[i].NPCObjects[k].Rarity == CruxObject.RarityEnum.Common)
                        {
                            if (!Biome[i].CommonCreatures.Contains(BiomeList[i].NPCObjects[k]))
                            {
                                Biome[i].CommonCreatures.Add(BiomeList[i].NPCObjects[k]);
                                BiomeList[i].NPCObjects[k].CurrentPopulation = 0;
                            }
                        }
                        else if (BiomeList[i].NPCObjects[k].Rarity == CruxObject.RarityEnum.Uncommon)
                        {
                            if (!Biome[i].UncommonCreatures.Contains(BiomeList[i].NPCObjects[k]))
                            {
                                Biome[i].UncommonCreatures.Add(BiomeList[i].NPCObjects[k]);
                                BiomeList[i].NPCObjects[k].CurrentPopulation = 0;
                            }
                        }
                        else if (BiomeList[i].NPCObjects[k].Rarity == CruxObject.RarityEnum.Rare)
                        {
                            if (!Biome[i].RareCreatures.Contains(BiomeList[i].NPCObjects[k]))
                            {
                                Biome[i].RareCreatures.Add(BiomeList[i].NPCObjects[k]);
                                BiomeList[i].NPCObjects[k].CurrentPopulation = 0;
                            }
                        }
                        else if (BiomeList[i].NPCObjects[k].Rarity == CruxObject.RarityEnum.UltraRare)
                        {
                            if (!Biome[i].UltraRareCreatures.Contains(BiomeList[i].NPCObjects[k]))
                            {
                                Biome[i].UltraRareCreatures.Add(BiomeList[i].NPCObjects[k]);
                                BiomeList[i].NPCObjects[k].CurrentPopulation = 0;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("The Biome '" + Biome[i].CategoryName + "' is missing a Crux Object in the NPC Objects list. Please check where you " +
                            "are missing an object and remove it or assign a Crux Object.");
                    }
                }
            }
        }

        //This function handles all of our spawning logic. 
        //It gets all biomes' info, spawning info, and spawns AI accordingly
        void Create()
        {
            if (m_TerrainInfo != null)
            {
                if (m_CurrentSpawnedObjects < m_MaxObjectsToSpawn)
                {
                    m_PositionToSpawn = (new Vector3(m_PlayerObject.position.x, m_PlayerObject.position.y, m_PlayerObject.position.z)) + UnityEngine.Random.insideUnitSphere * m_MaxRadius;
                    m_InstancedChecker.transform.position = new Vector3(m_PositionToSpawn.x, m_PlayerObject.position.y + 50, m_PositionToSpawn.z);
                    m_TerrainInfo.UpdateTerrainInfo();

                    if (TerrainType == TerrainTypeEnum.UnityTerrain)
                    {
                        m_ReceivedHeight.y = m_TerrainInfo.terrain.SampleHeight(m_PositionToSpawn);
                        m_PositionToSpawn = new Vector3(m_InstancedChecker.transform.position.x, m_ReceivedHeight.y, m_InstancedChecker.transform.position.z);
                    }
                    else if (TerrainType == TerrainTypeEnum.MeshTerrain)
                    {
                        m_ReceivedHeight.y = m_TerrainInfo.height;
                        m_PositionToSpawn = new Vector3(m_InstancedChecker.transform.position.x, m_ReceivedHeight.y, m_InstancedChecker.transform.position.z);
                    }

                    if (UseLayerDetection == UseLayerDetectionEnum.Yes && Vector3.Distance(m_PlayerObject.position, m_PositionToSpawn) >= m_MinRadius && !Physics.CheckSphere(m_PositionToSpawn, AILayerMaskDectectionDistance, AILayerMask) ||
                        Vector3.Distance(m_PlayerObject.position, m_PositionToSpawn) >= m_MinRadius && UseLayerDetection == UseLayerDetectionEnum.No)
                    {
                        if (!m_TerrainInfo.positionInvalid && m_TerrainInfo.terrainAngle >= minSteepness && m_TerrainInfo.terrainAngle <= maxSteepness)
                        {
                            if (TerrainType == TerrainTypeEnum.UnityTerrain)
                            {
                                #if UNITY_2018_3_OR_NEWER
                                m_Texture = m_TerrainInfo.terrainData.terrainLayers[m_TerrainInfo.surfaceIndex].diffuseTexture;
                                #else
                                m_Texture = m_TerrainInfo.terrainData.splatPrototypes[m_TerrainInfo.surfaceIndex].texture;
                                #endif
                            }

                            for (int i = 0; i < Biome.Count; i++)
                            {
                                //Check the texture and find the matching Biome. Spawn AI according to the Biome
                                if (Biome[i].TerrainTextures.Contains(m_Texture) || TerrainType == TerrainTypeEnum.MeshTerrain)
                                {
                                    float random = UnityEngine.Random.value;
                                    float Sum = Biome[i].WildlifeOdds + Biome[i].CreaturesOdds + Biome[i].NPCOdds;
                                    float generatedWildlifeOdds = Biome[i].WildlifeOdds / Sum;
                                    float generatedCreaturesOdds = (Biome[i].CreaturesOdds / Sum) + generatedWildlifeOdds;
                                    float generatedNPCOdds = (Biome[i].NPCOdds / Sum) + generatedWildlifeOdds + generatedCreaturesOdds;

                                    if (random < generatedWildlifeOdds)
                                    {
                                        WildlifeSpawning.AttemptWildlifeSpawn(i);
                                    }
                                    else if (random < generatedCreaturesOdds)
                                    {
                                        CreatureSpawning.AttemptCreatureSpawn(i);
                                    }
                                    else if (random < generatedNPCOdds)
                                    {
                                        NPCSpawning.AttemptNPCSpawn(i);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //This portion handles all of our despawning and population tracking.
            //This is accomplished by checking the distance every spawn interval.
            //If an object is within the despawn radius, or if it has been destroyed, 
            //remove it from the lists and reduce the population for that specific object.
            if (m_SpawnedObjects.Count > 0)
            {
                foreach (GameObject G in m_SpawnedObjects.ToArray())
                {
                    if (G != null)
                    {
                        if (Vector3.Distance(G.transform.position, m_PlayerObject.position) > m_DespawnRadius)
                        {
                            //Emerald AI support
                            #if EMERALD_AI_PRESENT
                            if (G.GetComponent<EmeraldAI.EmeraldAISystem>() != null)
                            {
                                if (G.GetComponent<EmeraldAI.EmeraldAISystem>().IsDead && G.GetComponent<EmeraldAI.EmeraldAISystem>().UseMagicEffectsPackRef == EmeraldAI.EmeraldAISystem.UseMagicEffectsPack.Yes)
                                {
                                    RemoveObjectNoPop(G);                                  
                                }
                                if (!G.GetComponent<EmeraldAI.EmeraldAISystem>().IsDead || G.GetComponent<EmeraldAI.EmeraldAISystem>().UseMagicEffectsPackRef == EmeraldAI.EmeraldAISystem.UseMagicEffectsPack.No)
                                {
                                    DespawnObject(G);
                                }
                            }
                            else if (G.GetComponent<EmeraldAI.EmeraldAISystem>() == null)
                            {
                                DespawnObject(G);
                            }
                            #else
                            DespawnObject(G);
                            #endif
                        }
                    }
                    else if (G == null)
                    {
                        DespawnObject(G);
                    }
                }
            }
        }

        void RemoveObjectNoPop(GameObject G)
        {
            int GetIndex = m_SpawnedObjects.IndexOf(G);
            string GetSpawnID = SpawnIDList[GetIndex];
            SpawnIDList.RemoveAt(GetIndex);
            m_SpawnedObjects.RemoveAt(GetIndex);

            if (SpawnType == SpawnTypeEnum.Stardard)
            {
                CruxPool.Despawn(G);
            }
            else if (SpawnType == SpawnTypeEnum.Node)
            {
                foreach (Transform T in G.transform.parent.transform)
                {
                    if (G == T.gameObject)
                    {
                        CruxPool.Despawn(T.gameObject);
                        T.parent = ObjectPool.transform;
                    }
                }
            }
        }

        public void RemoveObjectFromPopulation(GameObject G)
        {
            int GetIndex = m_SpawnedObjects.IndexOf(G);
            string GetSpawnID = SpawnIDList[GetIndex];
            m_CurrentSpawnedObjects--;

            for (int i = 0; i < Biome.Count; i++)
            {
                //Wildlife
                for (int l = 0; l < BiomeList[i].WildlifeObjects.Count; l++)
                {
                    if (BiomeList[i].WildlifeObjects[l].SpawnID == GetSpawnID)
                    {                      
                        BiomeList[i].WildlifeObjects[l].CurrentPopulation--;
                        return;
                    }
                }

                //Creature
                for (int l = 0; l < BiomeList[i].CreatureObjects.Count; l++)
                {
                    if (BiomeList[i].CreatureObjects[l].SpawnID == GetSpawnID)
                    {
                        BiomeList[i].CreatureObjects[l].CurrentPopulation--;
                        return;
                    }
                }

                //NPC
                for (int l = 0; l < BiomeList[i].NPCObjects.Count; l++)
                {
                    if (BiomeList[i].NPCObjects[l].SpawnID == GetSpawnID)
                    {
                        BiomeList[i].NPCObjects[l].CurrentPopulation--;
                        return;
                    }
                }
            }
        }

        void DespawnObject(GameObject G)
        {
            int GetIndex = m_SpawnedObjects.IndexOf(G);
            string GetSpawnID = SpawnIDList[GetIndex];
            SpawnIDList.RemoveAt(GetIndex);
            m_SpawnedObjects.RemoveAt(GetIndex);
            m_CurrentSpawnedObjects--;

            for (int i = 0; i < Biome.Count; i++)
            {
                //Wildlife
                for (int l = 0; l < BiomeList[i].WildlifeObjects.Count; l++)
                {
                    if (BiomeList[i].WildlifeObjects[l].SpawnID == GetSpawnID)
                    {
                        BiomeList[i].WildlifeObjects[l].CurrentPopulation--;
                    }
                }

                //Creature
                for (int l = 0; l < BiomeList[i].CreatureObjects.Count; l++)
                {
                    if (BiomeList[i].CreatureObjects[l].SpawnID == GetSpawnID)
                    {
                        BiomeList[i].CreatureObjects[l].CurrentPopulation--;
                    }
                }

                //NPC
                for (int l = 0; l < BiomeList[i].NPCObjects.Count; l++)
                {
                    if (BiomeList[i].NPCObjects[l].SpawnID == GetSpawnID)
                    {
                        BiomeList[i].NPCObjects[l].CurrentPopulation--;
                    }
                }
            }

            if (G != null)
            {
                if (SpawnType == SpawnTypeEnum.Stardard)
                {
                    CruxPool.Despawn(G);
                }
                else if (SpawnType == SpawnTypeEnum.Node)
                {
                    foreach (Transform T in G.transform.parent.transform)
                    {
                        if (G == T.gameObject)
                        {
                            CruxPool.Despawn(T.gameObject);
                            T.parent = ObjectPool.transform;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function handles the Spawn by ID feature for Crux.
        /// All objects that have been created with Crux receive a Spawn ID.
        /// These Spawn IDs can be used to spawn AI to the player's location for easy testing and development.
        /// The Spawn ID can be found on the Crux Object of the object you would like to spawn.
        /// </summary>
        /// <param name="SpawnID">The Crux Object's Spawn ID.</param>
        public void SpawnByID (string SpawnID)
        {
            EnteredSpawnID = SpawnID;
            CreateObjectBySpawnID();
        }

        void GetSpawnIDUI ()
        {
            EnteredSpawnID = InputText.text;
            CreateObjectBySpawnID();
        }
        
        void CreateObjectBySpawnID()
        {
            for (int i = 0; i < Biome.Count; i++)
            {
                for (int l = 0; l < BiomeList[i].WildlifeObjects.Count; l++)
                {
                    if (BiomeList[i].WildlifeObjects[l].SpawnID == EnteredSpawnID)
                    {
                        GameObject SpawnObject = Instantiate(BiomeList[i].WildlifeObjects[l].ObjectToSpawn, m_PlayerObject.transform.position + UnityEngine.Random.onUnitSphere * 10, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
                        RaycastHit hit;
                        if (Physics.Raycast(new Vector3(SpawnObject.transform.position.x, SpawnObject.transform.position.y + 50, SpawnObject.transform.position.z), -Vector3.up, out hit, 100, TerrainInfoLayerMask))
                        {
                            SpawnObject.transform.position = hit.point;
                        }
                    }
                }
            }

            for (int i = 0; i < Biome.Count; i++)
            {
                if (Biome[i].CreaturesSpawnId.Contains(EnteredSpawnID))
                {
                    int IDIndex = Biome[i].CreaturesSpawnId.IndexOf(EnteredSpawnID);
                    Instantiate(Biome[i].CreaturesToSpawn[IDIndex], m_PlayerObject.transform.position + UnityEngine.Random.onUnitSphere * 10, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
                }
            }

            for (int i = 0; i < Biome.Count; i++)
            {
                if (Biome[i].NPCSpawnId.Contains(EnteredSpawnID))
                {
                    int IDIndex = Biome[i].NPCSpawnId.IndexOf(EnteredSpawnID);
                    Instantiate(Biome[i].NPCToSpawn[IDIndex], m_PlayerObject.transform.position + UnityEngine.Random.insideUnitSphere * 10, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
                }
            }
        }

        /// <summary>
        /// Removes and despawns the gameobject from Crux.
        /// </summary>
        /// <param name="ObjectToRemove">The object that will be removed and despawned from Crux.</param>
        public void RemoveObject(GameObject ObjectToRemove)
        {
            if (m_SpawnedObjects.Contains(ObjectToRemove))
            {
                DespawnObject(ObjectToRemove);
            }
            else
            {
                Debug.LogWarning("The object " + ObjectToRemove.name + " wasn't created with Crux - Ignoring Object");
            }
        }

        void OnApplicationQuit()
        {
            for (int i = 0; i < BiomeList.Count; i++)
            {
                //Wildlife
                for (int k = 0; k < BiomeList[i].WildlifeObjects.Count; k++)
                {
                    if (BiomeList[i].WildlifeObjects[k] != null)
                    {
                        BiomeList[i].WildlifeObjects[k].CurrentPopulation = 0;
                    }
                }

                //Creature
                for (int k = 0; k < BiomeList[i].CreatureObjects.Count; k++)
                {
                    if (BiomeList[i].CreatureObjects[k] != null)
                    {
                        BiomeList[i].CreatureObjects[k].CurrentPopulation = 0;
                    }
                }

                //NPC
                for (int k = 0; k < BiomeList[i].NPCObjects.Count; k++)
                {
                    if (BiomeList[i].NPCObjects[k] != null)
                    {
                        BiomeList[i].NPCObjects[k].CurrentPopulation = 0;
                    }
                }
            }
        }
    }
}