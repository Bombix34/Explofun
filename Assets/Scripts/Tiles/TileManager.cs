using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private TileSettings m_Settings;
    private TileRenderer m_Renderer;

    private void Awake()
    {
        m_Renderer = GetComponent<TileRenderer>();
    }

    #region GET/SET

    public TileRenderer Renderer
    {
        get
        {
            return m_Renderer;
        }
    }

    public TileSettings Settings
    {
        get
        {
            return m_Settings;
        }
    }

    #endregion
}
