using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterRenderer : EntityRenderer
{
    public CharacterManager Manager { get; set; }
    public CharacterLevelFeedback LevelFeedback { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        LevelFeedback = GetComponent<CharacterLevelFeedback>();
    }

    public void ActivationFeedback()
    {
        Transform renderTransform = m_Renderer.gameObject.transform;
        renderTransform.DOScale(m_BaseScale.x * 1.2f, 0.1f)
            .OnComplete(()=> renderTransform.DOScale(m_BaseScale.x,0.1f));
    }

    public void AppearEffect()
    {
        m_Renderer.transform.DOScale(0f, 0f);
        m_Renderer.transform.DOScale(1f, 0.1f);
    }

    public void FusionFeedback(TileManager centertile)
    {
        LevelFeedback.ResetContainers();
        Transform renderTransform = m_Renderer.gameObject.transform;
        renderTransform.DOScale(0f, 0.3f);
        renderTransform.DOMove(centertile.transform.position,0.3f)
            .OnComplete(() => Manager.DestroyCharacter()); 
    }
}
