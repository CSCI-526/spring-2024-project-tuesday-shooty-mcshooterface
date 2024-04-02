using System;
using Scripts.Player;
using UnityEngine;

public static class GunHelper
{
    public static Vector3 GetDirection(RaycastHit[] hits, Transform modelTransform, float maxRange)
    {
        Span<RaycastHit> hitsSpan = hits;
        for (int i = 0; i < hitsSpan.Length; i++)
        {
            RaycastHit hit = hitsSpan[i];
            if (hit.collider.tag != "IgnoreRaycast")
            {
                return (hit.point - modelTransform.position).normalized;

            }
        }

        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        Transform spawnTransform = PlayerCharacterController.Instance.BulletSpawnTransform;
        return (
            (spawnTransform.position + spawnTransform.forward * maxRange)
            - modelTransform.position
        ).normalized;

    }
}
