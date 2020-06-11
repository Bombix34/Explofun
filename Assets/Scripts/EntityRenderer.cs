using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRenderer : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer m_Renderer;
    protected Vector3 m_BaseScale;

    public bool IsBouncing { get; set; } = false;
    public bool IsScaleUp { get; set; } = false;

    protected virtual void Awake()
    {
        m_BaseScale = m_Renderer.transform.localScale;
    }

    public void ResetTile()
    {
        Color = Color.white;
        Scale = m_BaseScale;
    }

    #region GET/SET

    public Color Color { set { m_Renderer.color = value; } }

    public Vector2 Scale
    {
        get { return m_Renderer.transform.localScale; }
        set { m_Renderer.transform.localScale = value; }
    }

    public Vector3 BaseScale { get => m_BaseScale; }


    #endregion
}
