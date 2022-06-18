using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Utils
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void FadeOut(Action onComplete)
        {
            _image.DOFade(0,1).OnComplete((() =>
            {
                _image.enabled = false;
                onComplete.Invoke();
            }));
        }
        
        public void FadeIn(Action onComplete)
        {   
            _image.enabled = true;
            _image.DOFade(1,1).OnComplete(onComplete.Invoke);
        }
        
    }
}