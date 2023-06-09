using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Parallax
{
    public class InfiniteParallaxLayer 
    {
    
        private readonly float  _speed;
        private readonly List<Transform> _layers;
        private static float _layerHorizontalSize = -1;

        public InfiniteParallaxLayer(SpriteRenderer initialPart, float speed, Transform parentTranform)
        {
            _speed = speed;
            if (_layerHorizontalSize < 0)
            {
                Sprite sprite = initialPart.sprite;
                _layerHorizontalSize = initialPart.transform.localScale.x *
                    (
                        initialPart.drawMode == SpriteDrawMode.Tiled ?
                        initialPart.size.x :
                        sprite.texture.width / sprite.pixelsPerUnit
                    );
            }
            _layers = new List<Transform>
                {
                    initialPart.transform
                };

            Vector2 secondPartPosition = ((Vector2)_layers[0].position) + new Vector2(_layerHorizontalSize, 0);
            Transform secondPart = Object.Instantiate(initialPart, secondPartPosition, Quaternion.identity).transform;
            secondPart.parent = parentTranform;
            _layers.Add(secondPart);
        }


        public void UpdateLayer(float targetPosition,float previousTargetPosition)
        {
            MoveParts(previousTargetPosition - targetPosition);
            FixLayersPosition(targetPosition);
          
        }

        private void MoveParts(float deltaMovement)
        {
            foreach (var layer in _layers)
            {
                Vector2 layerPosition = layer.position;
                layerPosition.x += deltaMovement * _speed;
                layer.position = layerPosition;
            }
        }

        private void FixLayersPosition(float targetPosition)
        {
            Transform activeLayer = _layers.Find(layer => IsLayerActive(layer, targetPosition));
            Transform layerToMove = _layers.Find(layer => !IsLayerActive(layer, targetPosition));
            if (activeLayer == null ||  layerToMove == null)
                return;

            float relativePosition = activeLayer.position.x;
            int direction = relativePosition > targetPosition ? -1 : 1;
            if (layerToMove.position.x > relativePosition && direction > 0 ||
                layerToMove.position.x < relativePosition && direction < 0)
                return;

            Vector2 layerToMoveNewPosition =
                new(relativePosition + _layerHorizontalSize * direction, layerToMove.position.y);
            layerToMove.position = layerToMoveNewPosition;
        }

        private bool IsLayerActive(Transform layer, float targetPosition) =>
            Mathf.Abs(layer.position.x - targetPosition) <= _layerHorizontalSize / 1.5;
    }
}


