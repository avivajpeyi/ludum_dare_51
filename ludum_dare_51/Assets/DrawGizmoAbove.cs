using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmoAbove : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public float scale = 0.5f;
    private void OnDrawGizmos()
    {
        Gizmos.color = sprite.color;
        Vector3 pos = this.transform.position + Vector3.up*3f;
        Gizmos.DrawSphere(pos, scale);
    }
}
