using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] RectTransform uiImage; 
    [SerializeField] float rotationTime = 1f;
    [SerializeField] int rotDir = 1;

    Tween rotateUITween;

    private void OnEnable()
    {
        uiImage.rotation = Quaternion.identity;
        rotateUITween = uiImage
            .DORotate(new Vector3(0, 0, 360 * rotDir), rotationTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnDisable()
    {
        if (rotateUITween != null && rotateUITween.IsActive())
        {
            rotateUITween.Kill();
        }
    }

}
