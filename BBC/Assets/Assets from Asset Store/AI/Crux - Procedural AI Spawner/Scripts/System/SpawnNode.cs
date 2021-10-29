using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Crux.Utility
{
    public class SpawnNode : MonoBehaviour
    {
        [HideInInspector]
        public int CurrentChildren;
        [HideInInspector]
        public GameObject Player;
        [HideInInspector]
        public GameObject SpawnNodeParent;
        [HideInInspector]
        public GameObject ChildrenHolder;
        [HideInInspector]
        public GameObject ObjectPool;
        [HideInInspector]
        public float Distance;
        [HideInInspector]
        public bool UsingObjectPooling = false;
        [HideInInspector]
        public int despawnDistance;
        [HideInInspector]
        public int deactivateDistance;
        [HideInInspector]
        public float UpdateFrequency = 1;

        void Start()
        {
            SpawnNodeParent = gameObject;
            ObjectPool = GameObject.Find("(Crux) Object Pool");
        }

        public void Initialize()
        {
            //StartCoroutine(CheckUpdater());
            InvokeRepeating("CheckNode", UpdateFrequency, UpdateFrequency);
        }

        /*
        IEnumerator CheckUpdater()
        {
            while (true)
            {
                yield return new WaitForSeconds(UpdateFrequency);
                CheckNode();
            }
        }
        */

        void CheckNode()
        {
            CurrentChildren = ChildrenHolder.transform.childCount;
            Distance = Vector3.Distance(transform.position, Player.transform.position);

            if (CurrentChildren <= 0)
            {
                if (!UsingObjectPooling)
                {
                    Destroy(gameObject);
                }
                else if (UsingObjectPooling)
                {
                    CruxPool.Despawn(gameObject);
                }
            }

            if (Distance < deactivateDistance && !ChildrenHolder.activeSelf)
            {
                foreach (Transform T in ChildrenHolder.transform)
                {
                    T.gameObject.SetActive(true);
                }

                ChildrenHolder.SetActive(true);
            }

            if (Distance >= deactivateDistance && Distance < despawnDistance && ChildrenHolder.activeSelf)
            {
                foreach (Transform T in ChildrenHolder.transform)
                {
                    T.gameObject.SetActive(false);
                }

                ChildrenHolder.SetActive(false);
            }
        }
    }
}