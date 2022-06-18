using DG.Tweening;
using Project.Ball;
using UnityEngine;

namespace Project
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _cameraParent;
        [SerializeField] private float _lerpSpeed;
      
        private BallsMover _ballsMover;

        public bool HasToFollow { get; set; } = true;
        
        public void Init(BallsMover ballsMover)
        {
            _ballsMover = ballsMover;
        }
        
        private void Update()
        {
            if(HasToFollow == false) return;
            
            _cameraParent.position = Vector3.Lerp(_cameraParent.position ,_ballsMover.GetMidPos(), _lerpSpeed * Time.deltaTime);
        }

        public void SetPos(Vector3 pos)
        {
            _cameraParent.transform.position = pos;
        }
        
        public void MoveTo(Vector3 pos)
        {
            _cameraParent.transform.DOMove(pos, 1);
        }

        public void LookAt(Vector3 targetPos)
        {
            Vector3 direction = targetPos - transform.position;

            transform.DOLookAt(direction, 1);
        }
        
    }
}