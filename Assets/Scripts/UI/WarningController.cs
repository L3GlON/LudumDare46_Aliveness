using UnityEngine;
using DG.Tweening;

public class WarningController : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOScale(1.2f, 0.3f).SetLoops(int.MaxValue, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
