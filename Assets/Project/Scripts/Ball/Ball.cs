using System;
using UniRx;
using UnityEngine;

namespace Project.Ball
{
    public class Ball : MonoBehaviour, IPoolable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LayerMask _ballLayer;
        [SerializeField] private Collider _collider;

        public Collider Collider => _collider;
        public Rigidbody Rigidbody => _rigidbody;

        public Subject<BallCollisionContext> Collision { get; } = new Subject<BallCollisionContext>();

        public bool IsOnBoost { get; set; }

        public bool IsInCrowd()
        {
            var checkSphere =  Physics.OverlapSphere(transform.position,
                transform.localScale.x / 2);
            
            Debug.Log(checkSphere.Length, gameObject);
            
            return checkSphere.Length > 1;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<Obstacle>(out var obstacle))
            {
                var ballCollisionContext = new BallCollisionContext();

                ballCollisionContext.Collision = other;
                ballCollisionContext.Sender = this;

                Collision.OnNext(ballCollisionContext);
            }
        }

        public void OnEnterPool()
        {
            // some visual things mb
        }

        public void OnExitPool()
        {
            // some visual things mb
        }
    }

    public interface IPoolable
    {
        void OnEnterPool();
        void OnExitPool();
    }

    public struct BallCollisionContext
    {
        public Ball Sender;
        public Collision Collision;
    }
}