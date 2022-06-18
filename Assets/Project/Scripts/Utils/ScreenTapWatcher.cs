using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Utils
{
    public class ScreenTapWatcher : MonoBehaviour
    {
        private IInputService _inputService;
        private IDisposable _disposable;

        private bool _stop;
        
        public void Init(IInputService inputService)
        {
            _inputService = inputService;
        }

        public async Task CheckForTap(Action onTap)
        {
            _stop = false;

            await Task.Delay(500);
            
            while (_stop == false)
            {
                if (_inputService.Touched)
                {
                    onTap.Invoke();
                    _stop = true;
                    break;
                }
                await Task.Yield();
            }
            
            await Task.CompletedTask;
        }
      
    }
}