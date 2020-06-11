using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterManager : MonoBehaviour
{
    private CharacterRenderer m_Renderer;
    public CharacterDatas Datas { get; private set; }
    public CharacterType Type { get; set; } = CharacterType.TYPE_01;
    public TileManager Tile { get; set; } = null;
    private int m_Level = 1;

    private void Awake()
    {
        m_Renderer = GetComponent<CharacterRenderer>();
        m_Renderer.Manager = this;
    }

    public void Init(CharacterDatas datas)
    {
        Datas = datas;
        Type = datas.m_Type;
        m_Renderer.Color = datas.m_Color;
    }

    public void ActivateCharacter()
    {
        m_Renderer.ActivationFeedback();
        for(int i = 0; i < m_Level; ++i)
        {
            Datas.SendEventMessage(this);
        }
    }

    public void DestroyCharacter()
    {
        Tile.CharacterOnTile = null;
        Destroy(this.gameObject);
    }

    public CharacterRenderer Renderer { get => m_Renderer; }

    public int Level
    {
        get => m_Level;
        set
        {
            m_Level = value;
            Renderer.LevelFeedback.UpdateLevel(m_Level);
        }
    }
}

[System.Serializable]
public enum CharacterType
{
    TYPE_01,
    TYPE_02,
    TYPE_03,
    TYPE_04,
    TYPE_05
}
