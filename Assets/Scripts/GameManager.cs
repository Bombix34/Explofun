using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState m_CurrentState = GameState.WAIT;
    private BoardManager m_BoardManager;

    [SerializeField]
    private InGameUI m_InGameUI;

    private int m_CurrentPower = 0;
    private int m_CurrentMoney = 0;

    private void Start()
    {
        m_BoardManager = BoardManager.Instance;
        m_CurrentState = GameState.PLAY;
    }

    private void Update()
    {
        if(m_CurrentState==GameState.PUT_CHARACTER)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (m_BoardManager.AddCharacterToRaw(test))
                {
                    m_CurrentState = GameState.WAIT;
                }
            }
        }
    }

#region EVENT

    public void OnCharacterAdded(bool isFirst)
    {
        m_BoardManager.FusionBoard(isFirst);
    }

    public void OnFusionComplete(bool isFusion)
    {
        if (isFusion)
            m_BoardManager.MoveTileToEmpty();
        else
            PlayBoardSequence();
    }

    public void OnMoveComplete(bool isMove)
    {
        if (isMove)
            m_BoardManager.FusionBoard(false);
        else
            PlayBoardSequence();
    }

    public void OnEventMessageReceive(CharacterManager sender, EventMessage message)
    {
        switch(message.m_MessageType)
        {
            case EventMessage.MessageType.UPDATE_GOLD:
                CurrentMoney += message.m_IntParam;
                break;
            case EventMessage.MessageType.UPDATE_POWER:
                CurrentPower +=message.m_IntParam;
                break;
            case EventMessage.MessageType.ACTIVATE_TILE:
                Vector2 senderPos = sender.Tile.Position;
                switch (message.m_Zone)
                {
                    case EventMessage.TriggerZone.SELF:
                        sender.ActivateCharacter();
                        break;
                    case EventMessage.TriggerZone.LEFT:
                        TileManager concernedTile = m_BoardManager.Board.GetTile((int)senderPos.x-1,(int)senderPos.y);
                        if (concernedTile != null)
                            concernedTile.ActivateCharacter();
                        break;
                }
                break;
        }
    }

    #endregion
    private CharacterType test;

    public void ClickToPutCharacter(int character)
    {
        m_CurrentState = GameState.PUT_CHARACTER;
        test = (CharacterType)character;
    }

    private void PlayBoardSequence()
    {
        StartCoroutine(SequenceCoroutine());
    }

    private IEnumerator SequenceCoroutine()
    {
        m_BoardManager.PrepareSequence();
        List<ActivationSequence> sequence = m_BoardManager.CurrentSequence;
        while(sequence.Count>0)
        {
            ActivationSequence curSequence = sequence[0];
            foreach(TileManager tile in curSequence.Tiles)
            {
                tile.ActivateCharacter();
            }
            yield return new WaitForSeconds(0.3f);
            sequence.RemoveAt(0);
        }
        m_CurrentState = GameState.PLAY;
    }

#region GET/SET

    public GameState CurrentState { get => m_CurrentState; }

    public int CurrentPower
    {
        get => m_CurrentPower;
        set
        {
            m_CurrentPower = value;
            m_InGameUI?.UpdateResourceUI(ResourceType.POWER, m_CurrentPower);
        }
    }
    public int CurrentMoney
    {
        get => m_CurrentMoney;
        set
        {
            m_CurrentMoney = value;
            m_InGameUI?.UpdateResourceUI(ResourceType.GOLD, m_CurrentMoney);
        }
    }

    public InGameUI IngameUI { get => m_InGameUI; }

#endregion

    public enum GameState
    {
        WAIT,
        PLAY,
        PUT_CHARACTER
    }

    public enum ResourceType
    {
        GOLD,
        POWER
    }
}
