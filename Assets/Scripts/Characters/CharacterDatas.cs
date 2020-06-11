using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TYPE_00_DATAS", menuName = "Explofun/new character datas")]
public class CharacterDatas : ScriptableObject
{
    public CharacterType m_Type;
    public Color m_Color;
    [SerializeField]
    private List<EventMessage> m_EventMessages;

    public void SendEventMessage(CharacterManager characterSender)
    {
        foreach (EventMessage message in m_EventMessages)
        {
            GameManager.Instance.OnEventMessageReceive(characterSender, message);
        }
    }
}
