using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileRenderer : MonoBehaviour
{
    private TileSettings m_Settings;
    [SerializeField]
    private SpriteRenderer m_Renderer;
    private Vector3 m_BaseScale;

    public bool IsBouncing { get; set; } = true;
    private float m_BounceAmount;
    private float m_BounceSpeed = 0.8f;

    private void Awake()
    {
        m_BaseScale = m_Renderer.transform.localScale;
    }

    private void Update()
    {
        if(IsBouncing)
        {
            m_BounceAmount = Mathf.PingPong(Time.time*m_BounceSpeed, 0.2f);
            m_Renderer.transform.localScale = new Vector2(m_BaseScale.x + m_BounceAmount, m_BaseScale.x + m_BounceAmount);
        }
    }

    public void InitRenderer(float bounceSpeed)
    {
        m_BounceSpeed = bounceSpeed;
    }

    public void SetColor(Color newColor)
    {
        m_Renderer.color = newColor;
    }

    #region GET/SET

    h

    #endregion
}
