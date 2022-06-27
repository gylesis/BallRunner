using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Ball
{
    public class Test : MonoBehaviour
    {
        [Min(0)] [SerializeField] private float _angle = 60;
        [Min(0)] [SerializeField] private float _radius = 5;
        [Min(0)] [SerializeField] private int _countz = 1;
        [Min(0)] [SerializeField] private float _offset = 5;

        [SerializeField] private float _polzunok = 2;

        [SerializeField] private float _angleModifier = 1;
        [SerializeField] private GameObject _ballTransform;
        
        private Vector3 _prevPos;

        private Dictionary<Node, List<Node>> _nodeNeighbours = new Dictionary<Node, List<Node>>();
        private List<Node> _nodes = new List<Node>();

        private List<GameObject> _gameObjects = new List<GameObject>();
        
        private void OnDrawGizmos()
        {
            _nodes.Clear();
            _nodeNeighbours.Clear();
            
            float angle = 0;

            var circlesCount = _countz / 6;

            foreach (Node node in _nodes)
            {
                Gizmos.DrawSphere(node.Position, _ballTransform.transform.localScale.y / 2);
            }

            /*var x = _radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            var z = _radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            var pos = new Vector3(x, 0, z);

            var startNode = new Node();

            startNode.Angle = angle;
            startNode.Radius = _radius;
            startNode.Position = pos + originPos;

            _nodes.Add(startNode);

            CreateNeighbours(startNode);

            var nodes = GetNeighbours(startNode);

            for (var index = 0; index < nodes.Count; index++)
            {
                Node node = nodes[index];
                
                CreateNeighbours(node);

                var neighbours = GetNeighbours(node);

                for (var i = 0; i < neighbours.Count; i++)
                {
                    Node neighbour = neighbours[i];

                }
            }*/


            /*
            for (int i = 1; i <= _count; i++)
            {
                Node node = _unInitNodes.Pop();

                CreateNeighbours(node);

                var neighbours = GetNeighbours(node);
                
                // angle += _angle;

                //  var neighbours = GetNeighbours(startNode);

                //originPos = node.Position;
            }
            */

            
            /*for (int i = 1; i <= _count; i++)
            {
                Vector3 position = transform.position;

                angle += _angle * _ballRadius;

                radius += _radius * _offset / _polzunok;
                
                position.x += radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                position.z += radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                //Gizmos.DrawLine(_prevPos, position);
                Gizmos.DrawSphere(position,_ballRadius);
                
                _prevPos = position;
            }*/
        }

        [ContextMenu(nameof(Create))]
        private void Create()
        {
            _gameObjects.ForEach(DestroyImmediate);
            _gameObjects.Clear();
            
            Vector3 originPos = transform.position;

            var neighbours = CreateNeighbours(originPos);

            foreach (Vector3 neighbour in neighbours)
            {
                var vector3s = CreateNeighbours(neighbour);

                /*foreach (Vector3 vector3 in vector3s)
                {
                    var list = CreateNeighbours(vector3);

                    foreach (Vector3 vector4 in list)
                    {
                        var neighbours1 = CreateNeighbours(vector4);

                        foreach (Vector3 vector5 in neighbours1)
                        {
                            var vector3s1 = CreateNeighbours(vector5);

                            foreach (Vector3 vector6 in vector3s1)
                            {
                                var list1 = CreateNeighbours(vector6);

                                /*foreach (Vector3 vector7 in list1)
                                {
                                    var neighbours2 = CreateNeighbours(vector7);

                                    foreach (Vector3 vector8 in neighbours2)
                                    {
                                        CreateNeighbours(vector8);
                                    }
                                }#1#
                            }
                        }
                    }
                }*/
            }
        }

        private List<Vector3> CreateNeighbours(Vector3 pos)
        {
            var originPos = pos;

            float angle = 0;

            List<Vector3> neighbours = new List<Vector3>();

            for (int i = 1; i <= 6; i++)
            {
                var node = new Node();

                var x = _radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                var z = _radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                var polarPos = new Vector3(x, 0, z);
                Vector3 nodePosition = originPos + polarPos;

                var overlapSphere = Physics.OverlapSphere(nodePosition, 0.3f);

                if (overlapSphere.Length > 0)
                {
                    Debug.Log(i);
                }
                
                node.Angle = angle;
                node.Radius = _radius;
                node.Position = nodePosition;

                neighbours.Add(nodePosition);

                GameObject item = Instantiate(_ballTransform, nodePosition, Quaternion.identity);

                Debug.Log($"count {overlapSphere.Length}", item);
                
                _gameObjects.Add(item);
                //_unInitNodes.Push(node);
                _nodes.Add(node);
                
                angle += _angle;
            }

            return neighbours;
        }

        private void CreateNeighbours(Node targetNode)
        {
            var originPos = targetNode.Position;

            float angle = 0;

            List<Node> neighbours = new List<Node>();

            for (int i = 1; i <= 6; i++)
            {
                var node = new Node();

                var x = _radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                var z = _radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                var pos = new Vector3(x, 0, z);
                Vector3 nodePosition = originPos + pos;

                if(_nodes.Any(x => x.Position == nodePosition)) continue;
                
                node.Angle = angle;
                node.Radius = _radius;
                node.Position = nodePosition;

                neighbours.Add(node);
                
                //_unInitNodes.Push(node);
                _nodes.Add(node);
                
                angle += _angle;
            }

            _nodeNeighbours.Add(targetNode, neighbours);
        }

        private List<Node> GetNeighbours(Node node) => _nodeNeighbours[node];

        private Vector3 Rotate(Vector3 origin, Vector3 direction, float angle)
        {
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 newVector3 = direction;

            newVector3.x = direction.x * cos - direction.z * sin;
            newVector3.z = direction.x * sin + direction.z * cos;

            return newVector3 + origin;
        }
    }

    public class Node
    {
        public Vector3 Position;
        public bool IsBusy;
        public float Angle;
        public float Radius;
    }
}