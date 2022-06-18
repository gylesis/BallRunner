using UniRx;
using UnityEngine;

namespace Project
{
    public class StandalondeInputService : IInputService , IUpdatable
    {
        public bool Touched { get; private set; }
        public Vector2 InputVector { get; private set; }

        public void Update()
        {
            var xSpeed = Input.GetAxis("Horizontal");
            
            InputVector = new Vector2(xSpeed,0);

            Touched = Input.anyKeyDown;
        }
    }


    public interface IUpdatable
    {
        void Update();
    }

    public interface IInputService
    {
        bool Touched { get; }
        Vector2 InputVector { get; }
    }
    
}