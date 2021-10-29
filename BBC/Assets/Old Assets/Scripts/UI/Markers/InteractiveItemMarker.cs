using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveItemMarker : MonoBehaviour
{
    public Image MarkerPrefab;

    private Image marker;
    private Transform target;

    private void Update()
    {
        var minX = marker.GetPixelAdjustedRect().width / 2;
        var minY = marker.GetPixelAdjustedRect().height / 2;
        var maxX = Screen.width - minX;
        var maxY = Screen.height - minY;

        var position = Camera.main.WorldToScreenPoint(target.position);
        if (Vector3.Dot(target.position - Camera.main.transform.position, Camera.main.transform.forward) < 0)
            position.x = position.x < Screen.width / 2 ? maxX : minX;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        marker.transform.position = position;
    }

    private void OnEnable()
    {
        if (target == null)
            target = gameObject.transform;
        marker = Instantiate(MarkerPrefab, UIManager.Instance.GetComponent<InterfaceMarkerContainers>().InteractiveItemMarkers.transform);
    }

    private void OnDisable()
    {
        Destroy(marker.gameObject);
    }
}
