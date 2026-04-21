using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Drag Player aici in Inspector

    [Header("Pozitie camera")]
    public float height = 5f;       // Cat de sus e camera
    public float distance = 6f;     // Cat de departe e camera in spate
    public float smoothSpeed = 8f;  // Cat de lin urmareste

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Pozitia dorita: in spatele si deasupra playerului
        Vector3 desiredPosition = target.position
            - Vector3.forward * distance
            + Vector3.up * height;

        // Smooth movement
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            1f / smoothSpeed
        );

        // Camera se uita mereu la player
        transform.LookAt(target.position + Vector3.up * 0.5f);
    }
}