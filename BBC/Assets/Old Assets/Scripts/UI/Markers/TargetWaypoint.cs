using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetWaypoint : MonoBehaviour
{
    public Image Waypoint;
    public Transform Target;

    private Transform player;

    private void Update()
    {
        var minX = Waypoint.GetPixelAdjustedRect().width / 2;
        var minY = Waypoint.GetPixelAdjustedRect().height / 2;
        var maxX = Screen.width - minX;
        var maxY = Screen.height - minY;

        var position = Camera.main.WorldToScreenPoint(Target.position);
        if (Vector3.Dot(Target.position - Camera.main.transform.position, Camera.main.transform.forward) < 0)
            position.x = position.x < Screen.width / 2 ? maxX : minX;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        Waypoint.transform.position = position;
        Waypoint.GetComponentInChildren<Text>().text = ((int)Vector3.Distance(Target.position, player.position)).ToString() + "m";
    }

    private void OnEnable()
    {
        if (player == null)
            player = GameManager.Instance.Player.transform;
    }
}
