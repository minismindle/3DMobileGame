using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    TextMeshPro _damageText;

    public void SetInfo(Vector3 pos,float damage = 0,Transform parent = null, bool isCritical = false)
    {
        _damageText = GetComponent<TextMeshPro>();
        transform.position = pos;

        _damageText.text = $"{Mathf.RoundToInt(damage)}";
        _damageText.color = Color.white;

        _damageText.alpha = 1.0f;   

        if(parent != null)
        {
            GetComponent<MeshRenderer>().sortingOrder = 321;
        }
        DoAnimation();
    }

    private void OnEnable()
    {
        
    }

    private void DoAnimation()
    {
        Sequence seq = DOTween.Sequence();

        transform.localScale = new Vector3(0, 0, 0);

        seq.Append(transform.DOScale(1.3f, 0.3f).SetEase(Ease.InOutBounce))
            .Join(transform.DOMove(transform.position + Vector3.up, 0.3f).SetEase(Ease.Linear))
            .Append(transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutBounce))
            .Join(transform.GetComponent<TMP_Text>().DOFade(0, 0.3f).SetEase(Ease.InQuint))
            .OnComplete(() =>
            {
                Managers.Resource.Destroy(gameObject);
            });
    }
}
