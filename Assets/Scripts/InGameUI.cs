using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_PowerUI, m_MoneyUI;

    public void UpdateResourceUI(GameManager.ResourceType resource, int val)
    {
        if (resource == GameManager.ResourceType.GOLD)
        {
            BounceEffect(m_MoneyUI.rectTransform,0.2f);
            m_MoneyUI.text = val.ToString("### ##0");
        }
        else if (resource == GameManager.ResourceType.POWER)
        {
            BounceEffect(m_PowerUI.rectTransform, 0.2f);
            m_PowerUI.text = val.ToString("### ###0");
        }
    }

    private void BounceEffect(RectTransform rect, float duration)
    {
        rect.DOScale(1.2f, duration/2).OnComplete(() => rect.DOScale(1f, duration/2));
    }
}
