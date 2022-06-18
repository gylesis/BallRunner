using System;
using UnityEngine;

namespace Project.Level.Gate
{
    [Serializable]
    public class GateCommand 
    {
        [SerializeField] private GateOperation _operation;
        [SerializeField] private int _value;

        public GateOperation Operation => _operation;
        public int Value => _value;
    }
    
    public enum GateOperation
    {
        Multiplication,
        Addition,
        Subtraction,
        Divide
    }
    
}