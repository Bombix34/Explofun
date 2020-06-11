using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardManager : Singleton<BoardManager>
{
    private GameManager m_GameManager;
    [SerializeField]
    private BoardSettings m_Settings;
    [SerializeField]
    private List<CharacterDatas> m_CharacterSettings;
    private Board m_Board;

    private List<TileManager> m_CurrentRawSelected;
    private TileManager m_CurrentTileSelected = null;
    private TileManager m_LastTileSpawn = null;
    public float m_ChronoCurrentSelection = -1f;

    private List<ActivationSequence> m_CurrentSequence = null;

    private void Awake()
    {
        m_Board = new Board(m_Settings.COLUMN, m_Settings.RAW);
        m_CurrentRawSelected = new List<TileManager>();
    }

    private void Start()
    {
        m_GameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (m_ChronoCurrentSelection == 0f)
            m_ChronoCurrentSelection=0.1f;
        else if (m_ChronoCurrentSelection > 0f)
        {
            m_ChronoCurrentSelection += Time.deltaTime;
            if (m_ChronoCurrentSelection >= 0.16f)
            {
                ResetSelection();
            }
        }
    }

    public void OnTileSelected(TileManager tileSelected)
    {
        m_CurrentTileSelected = tileSelected;
        m_ChronoCurrentSelection = -1f;
        if (m_GameManager.CurrentState == GameManager.GameState.PLAY)
        {
            //afficher les informations de la tile actuelle
            tileSelected.Renderer.IsBouncing = true;
        }
        else if (m_GameManager.CurrentState == GameManager.GameState.PUT_CHARACTER)
        {
            if (m_CurrentRawSelected.Count != 0 && m_CurrentRawSelected[0].Position.y != tileSelected.Position.y)
                ResetSelection();
            m_CurrentRawSelected = Board.GetRaw((int)tileSelected.Position.y);
            foreach(TileManager tile in m_CurrentRawSelected)
            {
                tile.Renderer.IsScaleUp = true;
            }
            TileManager freeTile = Board.GetLeftFreeTileInRaw(m_CurrentRawSelected);
            if (freeTile != null)
            {
                freeTile.Renderer.IsBouncing = true;
                freeTile.Renderer.Color = Color.green;
                freeTile.Renderer.IsScaleUp = false;
            }
        }
    }

    public void OnTileDeselected(TileManager tileDeselected)
    {
        if (m_GameManager.CurrentState == GameManager.GameState.PLAY)
        {
            //afficher les informations de la tile actuelle
            tileDeselected.Renderer.IsBouncing = false;
            tileDeselected.Renderer.ResetTile();
        }
        else if (m_GameManager.CurrentState == GameManager.GameState.PUT_CHARACTER)
        {
            m_ChronoCurrentSelection = 0f;
        }
    }

    private void ResetSelection()
    {
        foreach (TileManager tile in m_CurrentRawSelected)
        {
            tile.Renderer.IsScaleUp = false;
            tile.Renderer.IsBouncing = false;
            tile.Renderer.ResetTile();
        }
        m_CurrentRawSelected.Clear();
    }

    public bool AddCharacterToRaw(CharacterType characterType)
    {
        if (m_CurrentRawSelected.Count == 0)
            return false;
        StartCoroutine(AddCharacterCoroutine(characterType));
        return true;
    }

    private IEnumerator AddCharacterCoroutine(CharacterType characterType)
    {
        TileManager freeTile = Board.GetLeftFreeTileInRaw(m_CurrentRawSelected);
        freeTile.AddCharacterOnTile(DatasOfType(characterType));
        m_LastTileSpawn = freeTile;
        ResetSelection();
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.OnCharacterAdded(true);
    }

    public void FusionBoard(bool isFirstFusion)
    {
        StartCoroutine(FusionCoroutine(isFirstFusion));
    }

    private IEnumerator FusionCoroutine(bool isFirstFusion)
    {
        bool isFusion = false;
        TileManager[] fusionedTiles;
        if (isFirstFusion)
        {
            fusionedTiles = Board.GetFusionedTiles(m_LastTileSpawn);
            if(fusionedTiles[0]!=null)
            {
                foreach(TileManager tile in fusionedTiles)
                {
                    tile.CharacterOnTile.Renderer.FusionFeedback(m_LastTileSpawn);
                    tile.CharacterOnTile = null;
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(0.2f);
                m_LastTileSpawn.CharacterOnTile.Level++;
                isFusion = true;
            }
        }
        foreach(TileManager tileBoard in Board.Tiles)
        {
            if(tileBoard != m_LastTileSpawn && tileBoard.CharacterOnTile!=null)
            {
                fusionedTiles = Board.GetFusionedTiles(tileBoard);
                if (fusionedTiles[0] != null)
                {
                    foreach (TileManager tileFusion in fusionedTiles)
                    {
                        tileFusion.CharacterOnTile.Renderer.FusionFeedback(tileBoard);
                        tileFusion.CharacterOnTile = null;
                        if (tileFusion == m_LastTileSpawn)
                            m_LastTileSpawn = tileBoard;
                        yield return new WaitForSeconds(0.1f);
                    }
                    yield return new WaitForSeconds(0.1f);
                    tileBoard.CharacterOnTile.Level++;
                    isFusion = true;
                }
            }
        }
        GameManager.Instance.OnFusionComplete(isFusion);
    }

    public void MoveTileToEmpty()
    {
        StartCoroutine(MoveTiles());
    }

    private IEnumerator MoveTiles()
    {
        bool isMove = false;
        foreach(TileManager tile in Board.Tiles)
        {
            if(tile.Position.x >0 && tile.CharacterOnTile!=null && Board.GetTile((int)tile.Position.x-1,(int)tile.Position.y).CharacterOnTile==null)
            {
                TileManager prevTile = Board.GetTile((int)tile.Position.x - 1, (int)tile.Position.y);
                tile.CharacterOnTile.transform.DOMoveX(prevTile.transform.position.x, 0.2f);
                prevTile.CharacterOnTile = tile.CharacterOnTile;
                prevTile.CharacterOnTile.Tile = prevTile;
                tile.CharacterOnTile = null;
                isMove = true;
                yield return new WaitForSeconds(0.1f);
            }
        }
        GameManager.Instance.OnMoveComplete(isMove);
    }

    public void PrepareSequence()
    {
        m_CurrentSequence = new List<ActivationSequence>();
        int posY = (int)m_LastTileSpawn.Position.y;
        int initPosX = (int)m_LastTileSpawn.Position.x;
        for(int i = 0; i <= initPosX; ++i)
        {
            m_CurrentSequence.Add(new ActivationSequence(Board.GetTile(initPosX - i, posY)));
        }
    }

    #region GET/SET

    public Board Board { get => m_Board; }

    public List<ActivationSequence> CurrentSequence { get => m_CurrentSequence; }

    private CharacterDatas DatasOfType(CharacterType type)
    {
        foreach(CharacterDatas data in m_CharacterSettings)
        {
            if (data.m_Type == type)
                return data;
        }
        return null;
    }

    #endregion
}

public class Board
{
    private TileManager[,] m_Tiles;
    private int m_Column, m_Raw;

    public Board(int column, int raw)
    {
        m_Tiles = new TileManager[column, raw];
        m_Column = column;
        m_Raw = raw;
    }

    public void InitTile(TileManager newTile, int column, int raw)
    {
        m_Tiles[column, raw] = newTile;
    }

    public TileManager GetTile(int column, int raw)
    {
        if (column < 0 || column >= m_Column || raw < 0 || raw >= m_Raw)
            return null;
        return m_Tiles[column, raw];
    }

    public List<TileManager> GetRaw(int rawNb)
    {
        List<TileManager> toReturn = new List<TileManager>();
        foreach (TileManager tile in m_Tiles)
        {
            if (tile.Position.y == rawNb)
                toReturn.Add(tile);
        }
        return toReturn;
    }

    public TileManager GetLeftFreeTileInRaw(List<TileManager> raw)
    {
        TileManager toReturn = null;
        for (int i = 0; i < raw.Count; ++i)
        {
            if (raw[i].CharacterOnTile == null)
            {
                toReturn = raw[i];
                break;
            }
        }
        return toReturn;
    }

    public TileManager[] GetFusionedTiles(TileManager tileToCheck)
    {
        if (tileToCheck.CharacterOnTile == null)
            return null;
        TileManager[] foundTiles = new TileManager[2];
        Vector2 tileIndex = tileToCheck.Position;
        //si pas collé a droite ou a gauche
        if(tileToCheck.Position.x>0 && tileToCheck.Position.x<m_Column-1)
        {
            //si pas collé en haut ou en bas et pas collé a droite ou a gauche
            if (tileToCheck.Position.y > 0 && tileToCheck.Position.y < m_Raw - 1)
            {
                //tout vérifier
                if(IsFusionWorking(tileToCheck,-1,0))
                {
                    foundTiles[0] = GetTile((int)tileIndex.x - 1, (int)tileIndex.y);
                    foundTiles[1] = GetTile((int)tileIndex.x + 1, (int)tileIndex.y);
                }
                else if (IsFusionWorking(tileToCheck, -1, 1))
                {
                    foundTiles[0] = GetTile((int)tileIndex.x - 1, (int)tileIndex.y+1);
                    foundTiles[1] = GetTile((int)tileIndex.x + 1, (int)tileIndex.y-1);
                }
                else if (IsFusionWorking(tileToCheck, 0, 1))
                {
                    foundTiles[0] = GetTile((int)tileIndex.x, (int)tileIndex.y + 1);
                    foundTiles[1] = GetTile((int)tileIndex.x, (int)tileIndex.y - 1);
                }
                else if (IsFusionWorking(tileToCheck, 1, 1))
                {
                    foundTiles[0] = GetTile((int)tileIndex.x + 1, (int)tileIndex.y + 1);
                    foundTiles[1] = GetTile((int)tileIndex.x - 1, (int)tileIndex.y - 1);
                }
            }
            //si collé en haut ou en bas mais pas collé a droite ou a gauche
            else
            {
                if (IsFusionWorking(tileToCheck, -1, 0))
                {
                    foundTiles[0] = GetTile((int)tileIndex.x - 1, (int)tileIndex.y);
                    foundTiles[1] = GetTile((int)tileIndex.x + 1, (int)tileIndex.y);
                }
            }
        }
        //si pas collé en haut ou en bas mais collé a droite ou a gauche
        else if (tileToCheck.Position.y > 0 && tileToCheck.Position.y < m_Raw - 1)
        {
            //vérifier haut et bas
            if (IsFusionWorking(tileToCheck, 0, 1))
            {
                foundTiles[0] = GetTile((int)tileIndex.x, (int)tileIndex.y + 1);
                foundTiles[1] = GetTile((int)tileIndex.x, (int)tileIndex.y - 1);
            }
        }
        //collé partout
        else
        {
            return foundTiles;
        }
        return foundTiles;
    }

    private bool IsFusionWorking(TileManager tile, int testX, int testY)
    {
        if (tile.CharacterOnTile == null)
            return false;
        CharacterManager tileCharacter = tile.CharacterOnTile;
        CharacterManager tileOneCharacter = GetTile((int)tile.Position.x + testX, (int)tile.Position.y + testY).CharacterOnTile;
        CharacterManager tileTwoCharacter = GetTile((int)tile.Position.x + (testX*-1), (int)tile.Position.y + (testY * -1)).CharacterOnTile;
        if (tileOneCharacter == null || tileTwoCharacter==null || tileCharacter.Level != tileOneCharacter.Level || tileCharacter.Level != tileTwoCharacter.Level
            || tileCharacter.Type != tileOneCharacter.Type || tileCharacter.Type != tileTwoCharacter.Type )
            return false;
        return true;
    }

    public TileManager[,] Tiles { get => m_Tiles; }
}

public class ActivationSequence
{
    public List<TileManager> m_Tiles;

    public ActivationSequence(List<TileManager> tilesToActivate)
    {
        m_Tiles = tilesToActivate;
    }

    public ActivationSequence(TileManager tileToActivate)
    {
        m_Tiles = new List<TileManager>();
        m_Tiles.Add(tileToActivate);
    }

    public List<TileManager> Tiles { get => m_Tiles; }
}
