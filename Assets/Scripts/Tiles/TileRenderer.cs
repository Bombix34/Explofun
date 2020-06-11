using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileRenderer : EntityRenderer
{
    private TileSettings m_Settings;
    private TileManager m_Manager;

    protected override void Awake()
    {
        base.Awake();
        m_Manager = GetComponent<TileManager>();
    }

    private void Update()
    {
        if(IsBouncing)
        {
            float bounceAmount = Mathf.PingPong(Time.time*m_Settings.BounceSpeed, 0.2f);
            Scale = new Vector2(m_BaseScale.x + bounceAmount, m_BaseScale.x + bounceAmount);
        }
        if(IsScaleUp)
        {
            if (Scale.x < m_BaseScale.x * 1.1f)
            {
                Scale += (Vector2.one * Time.fixedDeltaTime);
                m_Manager.TileTrigger.transform.localScale += (Vector3.one * Time.fixedDeltaTime * 2f);
            }
        }
        else
            m_Manager.TileTrigger.transform.localScale = Vector3.one;
    }

    #region GET/SET

    public TileSettings Settings { set { m_Settings = value; } }

    #endregion
}
