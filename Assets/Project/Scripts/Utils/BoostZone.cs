using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Project.Utils
{
    public class BoostZone : MonoBehaviour
    {
        [SerializeField] private float _jumpDuration;
        [SerializeField] private float _boostModifier = 0.5f;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Ball.Ball>(out var ball))
            {
                ball.IsOnBoost = true;
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<Ball.Ball>(out var ball))
            {
                 ball.IsOnBoost = true;
                ball.Rigidbody.velocity += transform.forward * _boostModifier;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Ball.Ball>(out var ball))
            {
                Observable.Timer(TimeSpan.FromMilliseconds(_jumpDuration)).Take(1).Subscribe((l => ball.IsOnBoost = false));
            }
        }
    }
}