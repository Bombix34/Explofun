using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterLevelFeedback : MonoBehaviour
{
    [SerializeField]
    private List<Image> levelFeedbackContainers;

    private void Start()
    {
        ResetContainers();
    }

    public void UpdateLevel(int level)
    {
        ResetContainers();
        for(int i = 0; i < levelFeedbackContainers.Count; ++i)
        {
            if (i == level - 1)
                break;
            levelFeedbackContainers[i].gameObject.SetActive(true);
            if(i==level-2)
            {
                levelFeedbackContainers[i].transform.DOScale(1.2f, 0.1f)
                    .OnComplete(()=>levelFeedbackContainers[i].transform.DOScale(1f, 0.1f));
            }
        }
    }

    public void ResetContainers()
    {
        for (int i = 0; i < levelFeedbackContainers.Count; ++i)
        {
            levelFeedbackContainers[i].gameObject.SetActive(false);
        }
    }
}
