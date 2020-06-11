using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new board settings", menuName = "Explofun/Board Settings")]
public class BoardSettings : ScriptableObject
{
    [Header("SPAWN")]
    public GameObject TILE_PREFAB;

    public int COLUMN = 8;
    public int RAW = 4;


    [Header("POSITIONS")]
    [Range(1f,2f)]
    public float COLUMN_DISPLACEMENT;
    [Range(1f, 2f)]
    public float RAW_DISPLACEMENT;
}
