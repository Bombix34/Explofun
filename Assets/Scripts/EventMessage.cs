using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventMessage 
{
    public MessageType m_MessageType;
    public TriggerZone m_Zone;
    public bool m_BoolParam;
    public int m_IntParam;
    public CharacterType m_CharacterTypeParam;

    public enum MessageType
    {
        UPDATE_GOLD,
        UPDATE_POWER,
        ACTIVATE_TILE
    }

    public enum TriggerZone
    {
        NONE,
        SELF,
        ZONE,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        RANDOM
    }
}
