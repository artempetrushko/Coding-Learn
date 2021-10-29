using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNISTORM_PRESENT
using UniStorm;
#endif

namespace Crux.Utility
{
    [CreateAssetMenu(fileName = "New Crux Object", menuName = "Crux/New Crux Object")]
    public class CruxObject : ScriptableObject
    {

        public string ObjectName = "New Object";
        public Texture CruxObjectIcon;
        public GameObject ObjectToSpawn;
        public string SpawnID = "";
        public float SpawnYOffset = 0;
        public int MinSpawnHeight = 0;
        public int MaxSpawnHeight = 1;
        public int GroupSpawnRadius = 50;
        public int MinGroupAmount = 1;
        public int MaxGroupAmount = 3;
        public int CurrentPopulation = 0;
        public int MaxPopulation = 5;
        public int MinSpawnElevation = 0;
        public int MaxSpawnElevation = 1000;

        public YesNoEnum GroupSpawning = YesNoEnum.No;
        public YesNoEnum UseCruxObjectIcon = YesNoEnum.No;
        public RarityEnum Rarity = RarityEnum.Common;
        public HeightTypeEnum HeightType = HeightTypeEnum.Land;
        public enum YesNoEnum { Yes, No };
        public enum RarityEnum { Common, Uncommon, Rare, UltraRare };
        public enum HeightTypeEnum { Land = 1, Air = 2, Water = 3 };

        //UniStorm 
#if UNISTORM_PRESENT
    public YesNoEnum UseUniStormConditions = YesNoEnum.No;
    public YesNoEnum WeatherSpawning = YesNoEnum.No;
    public YesNoEnum TimeSpawning = YesNoEnum.No;
    public YesNoEnum SeasonSpawning = YesNoEnum.No;

    public List<WeatherType> WeatherTypesList = new List<WeatherType>();

    public TimeOfDayEnum SpawningTime;
    public enum TimeOfDayEnum
    {
        Morning = 0, Day, Evening, Night
    }

    public SeasonEnum SpawningSeason = SeasonEnum.Summer;
    public enum SeasonEnum
    {
        Spring = 1, Summer = 2, Fall = 3, Winter = 4
    }
#endif
    }
}