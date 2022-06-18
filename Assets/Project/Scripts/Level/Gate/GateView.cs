using TMPro;
using UnityEngine;

namespace Project.Level.Gate
{
    public class GateView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Renderer _renderer;
        
        public void Init(GateCommand gateCommand)
        {
            GateOperation gateOperation = gateCommand.Operation;

            string sign = "";
            Color color = Color.white;
            
            switch (gateOperation)
            {
                case GateOperation.Multiplication:
                    sign = "*";
                    break;
                case GateOperation.Addition:
                    sign = "+";
                    break;
                case GateOperation.Subtraction:
                    color = Color.red;
                    sign = "-";
                    break;
                case GateOperation.Divide:
                    color = Color.red;
                    sign = "/";
                    break;
            }

            Color materialColor = _renderer.material.color;

            materialColor.r = color.r;
            materialColor.g = color.g;
            materialColor.b = color.b;

            _renderer.material.color = materialColor;

            // _text.color = color;
            _text.text = $"{sign}{gateCommand.Value}";
        }
        
    }
}