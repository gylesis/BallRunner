using System.Collections.Generic;

namespace Project.Level
{
    public class LevelReachedDispatcher
    {
        private readonly List<ILevelReachedListener> _listeners = new List<ILevelReachedListener>();

        public void AddListener(ILevelReachedListener listener)
        {
            _listeners.Add(listener);
        }
        
        public void LevelReach(LevelReachedContext levelReachedContext)
        {
            foreach (ILevelReachedListener levelReachedListener in _listeners)
            {
                levelReachedListener.OnLevelReached(levelReachedContext);
            }
        }
    }
}