using System;
using UniRx;
using UnityEngine;

namespace Project.Utils
{
    public class BoostZone : MonoBehaviour
    {
        [SerializeField] private float _boostModifier;

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
               // ball.IsOnBoost = true;
                ball.Rigidbody.velocity += transform.forward * _boostModifier;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Ball.Ball>(out var ball))
            {
                Observable.Timer(TimeSpan.FromMilliseconds(200)).Take(1).Subscribe((l => ball.IsOnBoost = false));
            }
        }
    }
}