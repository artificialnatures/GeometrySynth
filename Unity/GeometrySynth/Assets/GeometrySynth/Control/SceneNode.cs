using UnityEngine;
using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.FunctionModules;

namespace GeometrySynth.Control
{
    public class SceneNode : MonoBehaviour, Transformable
    {
        public Connectable Module
        {
            get { return module; }
        }
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Shape Shape
		{
			get { return shape; }
			set { shape = value; }
		}
		public float Scalar
		{
			get { return scalar; }
			set { scalar = value; }
		}
        public bool IsArrayed
        {
            get { return isArrayed; }
            set { isArrayed = value; }
        }
        public bool Array(int countX, int countY, int countZ)
        {
            bool returnValue = false;
            if (countX != arraySize[0])
            {
                arraySize[0] = countX;
                returnValue = true;
            }
			if (countY != arraySize[1])
			{
				arraySize[1] = countY;
				returnValue = true;
			}
			if (countZ != arraySize[2])
			{
				arraySize[2] = countZ;
				returnValue = true;
			}
            if (returnValue)
            {
                foreach (var child in children)
                {
                    Destroy(child.gameObject);
                }
                children.Clear();
                arraySpacing = new float[] { scale.x, scale.y, scale.z };
                var startX = -((arraySize[0] * arraySpacing[0]) / 2.0f);
                var startY = -((arraySize[1] * arraySpacing[1]) / 2.0f);
                var startZ = -((arraySize[2] * arraySpacing[2]) / 2.0f);
                var startingPosition = Vector3.zero;
                for (int x = 0; x < countX; x++)
                {
                    for (int y = 0; y < countY; y++)
                    {
                        for (int z = 0; z < countZ; z++)
                        {
                            startingPosition = new Vector3(
                                startX + (x * arraySpacing[0]),
                                startY + (y * arraySpacing[1]),
                                startZ + (z * arraySpacing[2])
                            );
                            var shapeObject = Instantiate(prefab, startingPosition, Quaternion.identity) as GameObject;
                            shapeObject.transform.SetParent(transform);
                            var shapeNode = shapeObject.AddComponent<ShapeNode>();
                            shapeNode.StartingPosition = startingPosition;
                            children.Add(shapeNode);
                        }
                    }
                }
            }
            isArrayed = true;
            scalar = 1.0f;
            return returnValue;
        }
		public bool Translate(float x, float y, float z)
		{
            translation = new Vector3(x * scalar, y * scalar, z * scalar);
            foreach (var child in children)
            {
                child.Translate(translation);
            }
            scalar = 1.0f;
			return true;
		}
		public bool Rotate(float x, float y, float z)
		{
            rotation = Quaternion.Euler(x * scalar, y * scalar, z * scalar);
            foreach (var child in children)
            {
                child.Rotate(rotation);
            }
            scalar = 1.0f;
            return true;
		}
		public bool Scale(float x, float y, float z)
		{
            scale = new Vector3(x * scalar, y * scalar, z * scalar);
            foreach (var child in children)
            {
                child.Scale(scale);
            }
            scalar = 1.0f;
			return true;
		}
        public bool ApplyColor(float r, float g, float b)
        {
            var colorScalar = Mathf.Clamp(scalar, 0.0f, 1.0f);
            color = new Color(r * colorScalar, g * colorScalar, b * colorScalar);
            foreach (var child in children)
            {
                child.ApplyColor(color);
            }
            scalar = 1.0f;
            return true;
        }
        public void Initialize(Creator shapeModule, GameObject shapePrefab)
		{
            module = shapeModule;
            prefab = shapePrefab;
            isActive = true;
            shape = Shape.CUBE;
            scalar = 1.0f;
            isArrayed = false;
            arraySize = new int[] { 1, 1, 1 };
            arraySpacing = new float[] { 1.0f, 1.0f, 1.0f };
            translation = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one;
            color = Color.white;
            var initialShape = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            var shapeNode = initialShape.AddComponent<ShapeNode>();
            children = new List<ShapeNode>();
            children.Add(shapeNode);
            initialShape.transform.SetParent(transform);
		}
        private Creator module;
        private GameObject prefab;
        private bool isActive;
		private Shape shape;
		private float scalar;
        private bool isArrayed;
        private int[] arraySize;
        private float[] arraySpacing;
        private Vector3 translation;
        private Quaternion rotation;
        private Vector3 scale;
        private Color color;
        private List<ShapeNode> children;
    }
}
