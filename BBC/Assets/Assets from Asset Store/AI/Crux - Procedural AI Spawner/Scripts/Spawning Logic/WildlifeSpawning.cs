using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNISTORM_PRESENT
using UniStorm;
#endif

namespace Crux.Utility
{
    public class WildlifeSpawning
    {
        public static void AttemptWildlifeSpawn(int i)
        {
            float GeneratedOdds = UnityEngine.Random.Range(1, 101);

            //If using the random wildlife setting, randomly pick an object from the whole list of wildlife objects.
            if (CruxSystem.Instance.Biome[i].WildlifeGenerationType == 0)
            {
                if (CruxSystem.Instance.BiomeList[i].WildlifeObjects.Count > 0)
                {
                    CruxSystem.Instance.Biome[i].RandomObject = UnityEngine.Random.Range(0, CruxSystem.Instance.BiomeList[i].WildlifeObjects.Count);
                    CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn = CruxSystem.Instance.BiomeList[i].WildlifeObjects[CruxSystem.Instance.Biome[i].RandomObject];

#if UNISTORM_PRESENT
                if (CruxSystem.Instance.UniStormPresent && CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.UseUniStormConditions == CruxObject.YesNoEnum.Yes)
                {
                    WildlifeUniStormConditions(CruxSystem.Instance.BiomeList[i].WildlifeObjects, i);
                }
#endif
                }
            }

            //If using the odds based generation type, pick the spawn category according to rarity of the generated odds.
            //When the rarity has beed reached, randomly pick an object within that rarity category
            if (CruxSystem.Instance.Biome[i].WildlifeGenerationType == 1)
            {
                //Common - 55%
                if (GeneratedOdds >= 45)
                {
                    if (CruxSystem.Instance.Biome[i].CommonWildlife.Count > 0)
                    {
                        CruxSystem.Instance.Biome[i].RandomObject = UnityEngine.Random.Range(0, CruxSystem.Instance.Biome[i].CommonWildlife.Count);
                        CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn = CruxSystem.Instance.Biome[i].CommonWildlife[CruxSystem.Instance.Biome[i].RandomObject];

#if UNISTORM_PRESENT
                    if (CruxSystem.Instance.UniStormPresent && CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.UseUniStormConditions == CruxObject.YesNoEnum.Yes)
                    {
                        WildlifeUniStormConditions(CruxSystem.Instance.Biome[i].CommonWildlife, i);
                    }
#endif
                    }
                }
                //Uncommon - 30%
                else if (GeneratedOdds < 45 && GeneratedOdds >= 15)
                {
                    if (CruxSystem.Instance.Biome[i].UncommonWildlife.Count > 0)
                    {
                        CruxSystem.Instance.Biome[i].RandomObject = UnityEngine.Random.Range(0, CruxSystem.Instance.Biome[i].UncommonWildlife.Count);
                        CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn = CruxSystem.Instance.Biome[i].UncommonWildlife[CruxSystem.Instance.Biome[i].RandomObject];

#if UNISTORM_PRESENT
                    if (CruxSystem.Instance.UniStormPresent && CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.UseUniStormConditions == CruxObject.YesNoEnum.Yes)
                    {
                        WildlifeUniStormConditions(CruxSystem.Instance.Biome[i].UncommonWildlife, i);
                    }
#endif
                    }
                }
                //Rare - 10%
                else if (GeneratedOdds < 15 && GeneratedOdds >= 5)
                {
                    if (CruxSystem.Instance.Biome[i].RareWildlife.Count > 0)
                    {
                        CruxSystem.Instance.Biome[i].RandomObject = UnityEngine.Random.Range(0, CruxSystem.Instance.Biome[i].RareWildlife.Count);
                        CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn = CruxSystem.Instance.Biome[i].RareWildlife[CruxSystem.Instance.Biome[i].RandomObject];

#if UNISTORM_PRESENT
                    if (CruxSystem.Instance.UniStormPresent && CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.UseUniStormConditions == CruxObject.YesNoEnum.Yes)
                    {
                        WildlifeUniStormConditions(CruxSystem.Instance.Biome[i].RareWildlife, i);
                    }
#endif
                    }
                }
                //Ultra Rare - 5%
                else if (GeneratedOdds < 5)
                {
                    if (CruxSystem.Instance.Biome[i].UltraRareWildlife.Count > 0)
                    {
                        CruxSystem.Instance.Biome[i].RandomObject = UnityEngine.Random.Range(0, CruxSystem.Instance.Biome[i].UltraRareWildlife.Count);
                        CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn = CruxSystem.Instance.Biome[i].UltraRareWildlife[CruxSystem.Instance.Biome[i].RandomObject];

#if UNISTORM_PRESENT
                    if (CruxSystem.Instance.UniStormPresent && CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.UseUniStormConditions == CruxObject.YesNoEnum.Yes)
                    {
                        WildlifeUniStormConditions(CruxSystem.Instance.Biome[i].UltraRareWildlife, i);
                    }
#endif
                    }
                }
            }

            if (CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn != null)
            {
                if (CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.GroupSpawning == CruxObject.YesNoEnum.Yes)
                {
                    CruxSystem.Instance.GroupAmount = UnityEngine.Random.Range(CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.MinGroupAmount, CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.MaxGroupAmount + 1);
                }

                if (CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn != null)
                {
                    if (CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.GroupSpawning == CruxObject.YesNoEnum.No)
                    {
                        CruxSystem.Instance.GroupAmount = 1;
                    }
                }

                //Spawn the generated object, but only if the population cap for that specific object hasn't been reached
                if (CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn != null && CruxSystem.Instance.m_TerrainInfo.height >= CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.MinSpawnElevation && CruxSystem.Instance.m_TerrainInfo.height <= CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.MaxSpawnElevation)
                {
                    //Spawn Nodes
                    //When using the Spawning Node setting, set all spawned AI as children to a node. This node will be set as inactive until a player is within range.
                    if (CruxSystem.Instance.SpawnType == CruxSystem.SpawnTypeEnum.Node && CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.CurrentPopulation < CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.MaxPopulation)
                    {
                        CruxSystem.Instance.CurrentNode = CruxPool.Spawn(CruxSystem.Instance.SpawnNode, CruxSystem.Instance.transform.position, Quaternion.identity); //Create spawn node using Crux's Object Pool
                        CruxSystem.Instance.CurrentNode.name = "Crux Spawn Node";
                        Vector3 NodeSpawn = CruxSystem.Instance.m_PositionToSpawn;

                        float height = 0;

                        if (CruxSystem.Instance.TerrainType == CruxSystem.TerrainTypeEnum.UnityTerrain)
                        {
                            height = CruxSystem.Instance.m_TerrainInfo.terrain.SampleHeight(NodeSpawn);
                            NodeSpawn = new Vector3(NodeSpawn.x, height + CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.SpawnYOffset + CruxSystem.Instance.m_TerrainInfo.terrain.transform.position.y, NodeSpawn.z);
                        }
                        else if (CruxSystem.Instance.TerrainType == CruxSystem.TerrainTypeEnum.MeshTerrain)
                        {
                            height = CruxSystem.Instance.m_TerrainInfo.height;
                            NodeSpawn = new Vector3(NodeSpawn.x, height + CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.SpawnYOffset, NodeSpawn.z);
                        }

                        CruxSystem.Instance.CurrentNode.transform.parent = CruxSystem.Instance.transform;
                        CruxSystem.Instance.CurrentNode.transform.position = NodeSpawn;
                        CruxSystem.Instance.ChildrenHolder = CruxSystem.Instance.CurrentNode.transform.GetChild(0).gameObject;
                        CruxSystem.Instance.ChildrenHolder.SetActive(false);

                        //Gets the spawn point system and applies the needed settings
                        SpawnNode SpawnNodeSystem = CruxSystem.Instance.CurrentNode.GetComponent<SpawnNode>();
                        SpawnNodeSystem.UpdateFrequency = CruxSystem.Instance.SpawnNodeUpdateFrequency;
                        SpawnNodeSystem.Player = CruxSystem.Instance.m_PlayerObject.gameObject;
                        SpawnNodeSystem.ChildrenHolder = CruxSystem.Instance.ChildrenHolder;
                        SpawnNodeSystem.despawnDistance = (int)CruxSystem.Instance.m_DespawnRadius;
                        SpawnNodeSystem.deactivateDistance = CruxSystem.Instance.SpawnNodeDeactivateDistance;
                        SpawnNodeSystem.UsingObjectPooling = true;
                        SpawnNodeSystem.Initialize();
                    }

                    for (int o = 0; o < CruxSystem.Instance.GroupAmount; o++)
                    {
                        if (CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.CurrentPopulation < CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.MaxPopulation)
                        {
                            if (CruxSystem.Instance.TerrainType == CruxSystem.TerrainTypeEnum.UnityTerrain)
                            {
                                CruxSystem.Instance.m_InstancedObject = CruxPool.Spawn(CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.ObjectToSpawn,
                                    new Vector3(CruxSystem.Instance.m_PositionToSpawn.x, CruxSystem.Instance.m_PositionToSpawn.y + CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.SpawnYOffset, CruxSystem.Instance.m_PositionToSpawn.z),
                                    Quaternion.Euler(CruxSystem.Instance.m_TerrainInfo.terrainData.GetInterpolatedNormal(CruxSystem.Instance.m_TerrainInfo.normalizedPos.x, CruxSystem.Instance.m_TerrainInfo.normalizedPos.y)));
                                CruxSystem.Instance.m_InstancedObject.SetActive(false);
                            }
                            else if (CruxSystem.Instance.TerrainType == CruxSystem.TerrainTypeEnum.MeshTerrain)
                            {
                                CruxSystem.Instance.m_InstancedObject = CruxPool.Spawn(CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.ObjectToSpawn,
                                    new Vector3(CruxSystem.Instance.m_PositionToSpawn.x, CruxSystem.Instance.m_PositionToSpawn.y + CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.SpawnYOffset, CruxSystem.Instance.m_PositionToSpawn.z),
                                    Quaternion.identity);
                            }

                            CruxSystem.Instance.m_InstancedObject.transform.parent = CruxSystem.Instance.transform;
                            CruxSystem.Instance.m_InstancedObject.transform.position = new Vector3(UnityEngine.Random.insideUnitSphere.x, 0, UnityEngine.Random.insideUnitSphere.z) *
                                UnityEngine.Random.Range(CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.GroupSpawnRadius / 2, CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.GroupSpawnRadius + 1) +
                                new Vector3(CruxSystem.Instance.m_PositionToSpawn.x, CruxSystem.Instance.m_PositionToSpawn.y + CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.SpawnYOffset, CruxSystem.Instance.m_PositionToSpawn.z);
                            CruxSystem.Instance.m_InstancedObject.transform.position = new Vector3(CruxSystem.Instance.m_InstancedObject.transform.position.x, 0, CruxSystem.Instance.m_InstancedObject.transform.position.z);
                            CruxSystem.Instance.m_InstancedObject.gameObject.name = CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.ObjectName;

                            //Recalculate our AI's Y position after the group spawning position has been created
                            //If using the Air or Water AI Type, randomize the height.
                            if (CruxSystem.Instance.TerrainType == CruxSystem.TerrainTypeEnum.UnityTerrain)
                            {
                                CruxSystem.Instance.SpawnedHeight = CruxSystem.Instance.m_TerrainInfo.terrain.SampleHeight(CruxSystem.Instance.m_InstancedObject.transform.position) + CruxSystem.Instance.m_TerrainInfo.terrain.transform.position.y;
                            }
                            else if (CruxSystem.Instance.TerrainType == CruxSystem.TerrainTypeEnum.MeshTerrain)
                            {
                                RaycastHit hit;
                                if (Physics.Raycast(new Vector3(CruxSystem.Instance.m_InstancedObject.transform.position.x, CruxSystem.Instance.m_InstancedObject.transform.position.y + 1000, CruxSystem.Instance.m_InstancedObject.transform.position.z), -Vector3.up, out hit))
                                {
                                    CruxSystem.Instance.SpawnedHeight = hit.point.y;
                                }
                            }

                            //Spawn height
                            if (CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.HeightType == CruxObject.HeightTypeEnum.Land)
                            {
                                CruxSystem.Instance.m_InstancedObject.transform.position = new Vector3(CruxSystem.Instance.m_InstancedObject.transform.position.x, CruxSystem.Instance.SpawnedHeight + CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.SpawnYOffset, CruxSystem.Instance.m_InstancedObject.transform.position.z);
                            }
                            else
                            {
                                CruxSystem.Instance.m_InstancedObject.transform.position = new Vector3(CruxSystem.Instance.m_InstancedObject.transform.position.x, CruxSystem.Instance.SpawnedHeight + CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.SpawnYOffset + UnityEngine.Random.Range(CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.MinSpawnHeight, CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.MaxSpawnHeight + 1), CruxSystem.Instance.m_InstancedObject.transform.position.z);
                            }

                            //Assign the spawned AI to the current spawn node
                            if (CruxSystem.Instance.SpawnType == CruxSystem.SpawnTypeEnum.Node)
                            {
                                CruxSystem.Instance.m_InstancedObject.transform.parent = CruxSystem.Instance.ChildrenHolder.transform;
                                CruxSystem.Instance.m_InstancedObject.SetActive(false);
                            }
                            else
                            {
                                CruxSystem.Instance.m_InstancedObject.SetActive(true);
                            }

                            CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.CurrentPopulation++;
                            CruxSystem.Instance.m_CurrentSpawnedObjects++;
                            CruxSystem.Instance.m_SpawnedObjects.Add(CruxSystem.Instance.m_InstancedObject);
                            CruxSystem.Instance.SpawnIDList.Add(CruxSystem.Instance.Biome[i].WildlifeObjectToSpawn.SpawnID);
                        }
                    }
                }
            }
        }

#if UNISTORM_PRESENT
    public static bool WildlifeUniStormConditions(List<CruxObject> RarityCategory, int BiomeIndex)
    {
        //Handles our UniStorm spawning conditions
        //If the UniStorm conditions are not met for the initial object picked, try again for each RespawnIteration.
        //If no object is found after this, the retry was unsuccessful and no object will be spawned.
        if (CruxSystem.Instance.UniStormPresent && CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn != null && CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.UseUniStormConditions == CruxObject.YesNoEnum.Yes)
        {
            //Weather Spawning
            if (CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.WeatherSpawning == CruxObject.YesNoEnum.Yes)
            {
                if (!CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.WeatherTypesList.Contains(UniStormSystem.Instance.CurrentWeatherType))
                {
                    for (int r = 0; r < CruxSystem.Instance.RespawnIterations; r++)
                    {
                        if (CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.UseUniStormConditions == CruxObject.YesNoEnum.Yes)
                        {
                            CruxSystem.Instance.Biome[BiomeIndex].RandomObject = UnityEngine.Random.Range(0, RarityCategory.Count);
                            CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn = RarityCategory[CruxSystem.Instance.Biome[BiomeIndex].RandomObject];

                            if (r == CruxSystem.Instance.RespawnIterations - 1)
                            {
                                CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn = null;
                            }
                        }
                    }
                }
            }
            //Time Spawning
            if (CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn != null && CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.TimeSpawning == CruxObject.YesNoEnum.Yes)
            {
                if ((int)CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.SpawningTime != (int)UniStormSystem.Instance.CurrentTimeOfDay)
                {
                    for (int r = 0; r < CruxSystem.Instance.RespawnIterations; r++)
                    {
                        if (CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.UseUniStormConditions == CruxObject.YesNoEnum.Yes && CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn != null)
                        {
                            CruxSystem.Instance.Biome[BiomeIndex].RandomObject = UnityEngine.Random.Range(0, RarityCategory.Count);
                            CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn = RarityCategory[CruxSystem.Instance.Biome[BiomeIndex].RandomObject];

                            if (r == CruxSystem.Instance.RespawnIterations - 1)
                            {
                                CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn = null;
                            }
                        }
                    }
                }
            }
            //Seasonal Spawning
            if (CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn != null && CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.SeasonSpawning == CruxObject.YesNoEnum.Yes)
            {
                if ((int)CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.SpawningSeason != (int)UniStormSystem.Instance.CurrentSeason)
                {
                    for (int r = 0; r < CruxSystem.Instance.RespawnIterations; r++)
                    {
                        if (CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn.UseUniStormConditions == CruxObject.YesNoEnum.Yes && CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn != null)
                        {
                            CruxSystem.Instance.Biome[BiomeIndex].RandomObject = UnityEngine.Random.Range(0, RarityCategory.Count);
                            CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn = RarityCategory[CruxSystem.Instance.Biome[BiomeIndex].RandomObject];

                            if (r == CruxSystem.Instance.RespawnIterations - 1)
                            {
                                CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn = null;
                            }
                        }
                    }
                }
            }
        }

        if (CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn != null)
        {
            //Respawn Successful
            return true;
        }
        else if (CruxSystem.Instance.Biome[BiomeIndex].WildlifeObjectToSpawn == null)
        {
            //Respawn Unsuccessful
            return false;
        }

        return false;
    }
#endif
    }
}
