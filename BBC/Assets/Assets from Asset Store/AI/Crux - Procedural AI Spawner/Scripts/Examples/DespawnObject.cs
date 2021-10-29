using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crux.Example
{
    public class DespawnObject : MonoBehaviour
    {
        Camera cam;

        void Start()
        {
            cam = GetComponent<Camera>();
        }

        public void RemoveObject(GameObject G)
        {
            CruxSystem.Instance.RemoveObject(G);
        }

        void Update()
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 15))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.tag == "AI")
                    {
                        RemoveObject(hit.collider.gameObject);
                    }
                }
            }
        }
    }
}