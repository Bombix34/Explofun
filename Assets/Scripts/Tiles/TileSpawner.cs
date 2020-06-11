using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField]
    private BoardSettings m_Settings;
    private BoardManager m_BoardManager;

    private void Awake()
    {
        m_BoardManager = GetComponent<BoardManager>();
    }

    private void Start()
    {
        InitSpawn();
    }

    public void InitSpawn()
    {
        for(int i = 0; i < m_Settings.COLUMN; ++i)
        {
            for(int j = 0; j < m_Settings.RAW; ++j)
            {
                GameObject tileSpawned = Instantiate(m_Settings.TILE_PREFAB, this.transform) as GameObject;
                tileSpawned.transform.position = new Vector2( i * m_Settings.COLUMN_DISPLACEMENT, j * m_Settings.RAW_DISPLACEMENT);
                TileManager tile = tileSpawned.GetComponent<TileManager>();
                tile.Init(new Vector2(i, j), m_BoardManager);
                m_BoardManager.Board.InitTile(tile, i, j);
            }
        }
    }
}
