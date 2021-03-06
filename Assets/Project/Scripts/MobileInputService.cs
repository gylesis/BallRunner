using UnityEngine;

namespace Project
{
    public class MobileInputService : IInputService, IUpdatable
    {
        public bool Touched { get; private set; }
        public Vector2 InputVector { get; private set; }

        private Vector2 _lastPos;

        public void Update()
        {
            var touches = Input.touches;
            
            Vector2 input = Vector2.zero;

            foreach (Touch touch in touches)
            {
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
                {
                    _lastPos = touch.position; 
                }

                input = (touch.position - _lastPos).normalized;
            }
            
            InputVector = input;

            Touched = touches.Length > 0;
        }
    }
}