using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private TileSettings m_Settings;
    private TileRenderer m_Renderer;
    public TileTrigger TileTrigger { get; private set; }

    private BoardManager m_BoardManager;

    private Vector2 m_GridPosition;
    public bool IsMouseOver { get; set; } = false;
    private bool m_IsLock = false;

    public CharacterManager CharacterOnTile { get; set; } = null;

    private UnityEvent m_OnMouseOver, m_OnMouseExit;

    private void Awake()
    {
        TileTrigger = GetComponentInChildren<TileTrigger>();
        m_Renderer = GetComponent<TileRenderer>();
        m_Renderer.Settings = m_Settings;
        m_OnMouseExit = new UnityEvent();
        m_OnMouseOver = new UnityEvent();
    }

    public void Init(Vector2 positionInGrid, BoardManager board)
    {
        m_GridPosition = positionInGrid;
        m_BoardManager = board;
        OnMouseOver.AddListener(() => IsMouseOver = true);
        OnMouseOver.AddListener(() => m_BoardManager.OnTileSelected(this));
        OnMouseExit.AddListener(() => IsMouseOver = false);
        OnMouseExit.AddListener(() => m_BoardManager.OnTileDeselected(this));
    }

    public void AddCharacterOnTile(CharacterDatas characterDatas)
    {
        if (CharacterOnTile != null)
            return;
        GameObject character = Instantiate(Resources.Load("Character_Prefab"), transform) as GameObject;
        CharacterOnTile = character.GetComponent<CharacterManager>();
        CharacterOnTile.Init(characterDatas);
        CharacterOnTile.Tile = this;
        character.transform.DOMoveY(transform.position.y + 0.4f, 0f);
        CharacterOnTile.Renderer.AppearEffect();
    }

    public void ActivateCharacter()
    {
        if (CharacterOnTile == null)
            return;
        CharacterOnTile.ActivateCharacter();
    }

    #region GET/SET

    public bool IsPositionCorresponding(Vector2 toTest)
    {
        return m_GridPosition.x == toTest.x && m_GridPosition.y == toTest.y;
    }

    public Vector2 Position { get { return m_GridPosition; } }

    public TileRenderer Renderer {  get { return m_Renderer; } }

    public TileSettings Settings {  get { return m_Settings; } }

    public UnityEvent OnMouseOver { get { return m_OnMouseOver; } }

    public UnityEvent OnMouseExit { get { return m_OnMouseExit; } }

    public bool IsLock
    {
        set
        {
            if (value)
                m_Renderer.Color = Color.grey;
            else
                m_Renderer.ResetTile();
            m_Renderer.enabled = !value;
            m_IsLock = value;
        }
    }

    #endregion
}
