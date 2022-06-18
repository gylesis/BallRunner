using Project.Ball;
using Project.Level;
using TMPro;
using UniRx;
using UnityEngine;

namespace Project
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelSign;
        [SerializeField] private TapToPlaySign _tapToPlaySign;

        private BallsMover _ballsMover;

        public void Init(BallsMover ballsMover)
        {
            _ballsMover = ballsMover;

            _tapToPlaySign.Tapped.TakeUntilDestroy(this).Subscribe((OnTapToPlayTapped));
        }

        private void OnTapToPlayTapped(Unit _)
        {
            _tapToPlaySign.Disable();

            _levelSign.enabled = false;
            _ballsMover.IsAllowedToPlay = true;
        }

        public void OnLevelLoaded(LevelData levelData)
        {
            _tapToPlaySign.Activate();
            
            _levelSign.enabled = true;
            _levelSign.text = $"Level {levelData.Id}";
        }

    }
    
    
}