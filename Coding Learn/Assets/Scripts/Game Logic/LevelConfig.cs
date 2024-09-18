using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Level Config", menuName = "Game Configs/Level Configs/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private LocalizedString _title;
        [SerializeField] private LocalizedString _description;
        [SerializeField] private AssetReference _thumbnailReference;
        [Space]
        [SerializeField] private AssetReference _sceneReference;
        [Space]
        [SerializeField] private LevelContent _content;

        public string Id => _id;
        public AssetReference SceneReference => _sceneReference;
        public LocalizedString Title => _title;
        public LocalizedString Description => _description;
        public AssetReference ThumbnailReference => _thumbnailReference;
        public LevelContent Content => _content;        
    }
}
