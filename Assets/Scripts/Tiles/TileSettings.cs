using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new tile settings", menuName = "Explofun/Tiles Settings")]
public class TileSettings : ScriptableObject
{
    [Header("BOUNCING")]
    [Range(0f,5f)]
    public float BounceSpeed = 1f;
}
