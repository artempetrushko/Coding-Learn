using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraBehaviour : MonoBehaviour
{
    public Transform Player;
    public float Smooth = 5.0f;
    public Vector3 Offset = new Vector3(0, 34, 0);

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Player.position + Offset, Time.deltaTime * Smooth);
    }
}
