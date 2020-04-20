using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class CharacterLinesController : MonoBehaviour
{
    public CharacterLines enemyRelatedLines;

    [SerializeField] private TextMeshProUGUI characterLine = null;

    private Queue<string> linesWaiting = new Queue<string>();

    private bool showingLine;

    private const float SHOWING_TIME = 2f;

    public void AddNewLine(string line)
    {
        linesWaiting.Enqueue(line);
        if(!showingLine)
        {
            ShowNextLine();
        }
    }

    private void ShowNextLine()
    {
        if (linesWaiting.Count > 0)
        {
            characterLine.text = linesWaiting.Dequeue();
            PlayAppearAnimation();
        }
    }

    public void ShowRandomEnemyLine()
    {
        AddNewLine(enemyRelatedLines.GetRandomLine());
    }

    private void PlayAppearAnimation()
    {
        showingLine = true;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);

        transform.DOScaleX(-1.1f, 0.4f).onComplete += delegate { transform.DOScaleX(-1f, 0.1f); };
        transform.DOScaleY(1.1f, 0.4f).onComplete += delegate { transform.DOScaleY(1f, 0.1f).onComplete += ScheduleDisappearing; };
    }


    private void ScheduleDisappearing()
    {
        transform.DOScaleX(-1.1f, 0.4f).SetDelay(SHOWING_TIME).onComplete += delegate { transform.DOScaleX(0, 0.4f); };
        transform.DOScaleY(1.1f, 0.4f).SetDelay(SHOWING_TIME).onComplete += delegate { transform.DOScaleY(0, 0.4f).onComplete += FinishShowing; };
    }

    private void FinishShowing()
    {
        gameObject.SetActive(false);
        showingLine = false;
        ShowNextLine();
    }
}
