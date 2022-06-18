using System;
using System.Threading.Tasks;
using DG.Tweening;
using Project.Utils;
using UniRx;
using UnityEngine;

namespace Project.Ball
{
    public class BallShooter : MonoBehaviour
    {
        [SerializeField] private Transform _arrow;
        [SerializeField] private Rigidbody _ballRigidbody;
        
        private IDisposable _disposable;
        private CameraController _cameraController;
        private BallsMover _ballsMover;
        private ScreenTapWatcher _screenTapWatcher;
        private float _cos;
        private BallsPool _ballsPool;

        public void Init(BallsMover ballsMover, CameraController cameraController, ScreenTapWatcher screenTapWatcher, BallsPool ballsPool)
        {
            _ballsPool = ballsPool;
            _screenTapWatcher = screenTapWatcher;
            _ballsMover = ballsMover;
            _cameraController = cameraController;
        }
        
        public async Task<float> Shoot(Level.Level level)  
        {
            _ballsMover.StopMovement(); 

            Vector3 pullPoint = level.LevelFinalZone.BallsPullPoint.position;

            _ballRigidbody.gameObject.SetActive(true);
            
            var sphere = _ballRigidbody;

            sphere.transform.position = pullPoint;
            sphere.transform.localScale *= 0.01f; 

            _cameraController.HasToFollow = false;
            _cameraController.MoveTo(pullPoint);
            
            for (var index = _ballsMover.Balls.Count - 1; index >= 0; index--)
            {
                Ball ball = _ballsMover.Balls[index];
                ball.Rigidbody.useGravity = false;
                ball.Collider.enabled = false;

                ball.transform.DOMove(pullPoint, 1).OnComplete((() => _ballsPool.ReturnToPool(ball)));
            }

            Vector3 sphereScale = Vector3.one * 3; // magic number

            await sphere.transform.DOScale(sphereScale, 2).AsyncWaitForCompletion();
            
            _arrow.gameObject.SetActive(true);
            _arrow.position = pullPoint;

            Sequence sequence = DOTween.Sequence();

            Tweener moveArrow = MoveArrow(0, -180);
            
            sequence.Append(moveArrow);
            sequence.Append(MoveArrow(-180,0));

            sequence.SetLoops(999);

            _disposable = Observable.EveryUpdate().Subscribe((l =>
            {
                var signedAngle = Vector3.SignedAngle(_arrow.transform.right, transform.forward,Vector3.up);

                _cos = Mathf.Cos(signedAngle);
            }));

            await _screenTapWatcher.CheckForTap((() =>
            {
                _disposable.Dispose();
                Shoot();
                sequence.Kill();
            } ));

            return _cos;
        }

        public void Shoot()
        {
            _ballRigidbody.velocity = _arrow.transform.right * 3; // magic number
        }
        
        private Tweener MoveArrow(float from, float to)
        {
            Tweener tweener = DOVirtual.Float(@from, to, 3, (value => // magic number
            {
                Vector3 rotationEulerAngles = _arrow.transform.rotation.eulerAngles;
                rotationEulerAngles.y = value;
                _arrow.transform.rotation = Quaternion.Euler(rotationEulerAngles);
            })).SetEase(Ease.Linear);

            return tweener;
        }

        public void Stop()
        {
            _arrow.gameObject.SetActive(false);
            _ballRigidbody.velocity = Vector3.zero;
            _ballRigidbody.gameObject.SetActive(false);
        }
        
    }
}