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
        }
        public bool Array(int countX, int countY, int countZ, float spacingX, float spacingY, float spacingZ)
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
            return returnValue;
        }
		public bool Translate(float x, float y, float z)
		{
            var translation = new Vector3(x * scalar, y * scalar, z * scalar);
            foreach (Transform child in transform)
            {
                child.localPosition = translation;
            }
			return true;
		}
		public bool Rotate(float x, float y, float z)
		{
            //TODO: handle array, handle mapping, handle offset/scalar
            var rotation = new Vector3(x * scalar, y * scalar, z * scalar);
            foreach (Transform child in transform)
            {
                child.localEulerAngles = rotation;
            }
            return true;
		}
		public bool Scale(float x, float y, float z)
		{
            //TODO: handle array, handle mapping, handle offset/scalar
            var scale = new Vector3(x * Mathf.Abs(scalar), y * Mathf.Abs(scalar), z * Mathf.Abs(scalar));
            foreach (Transform child in transform)
            {
                child.localScale = scale;
            }
			return true;
		}
        public bool Color(float r, float g, float b)
        {
            var colorScalar = (scalar + 1.0f) * 0.5f;
            var color = new Color(r * colorScalar, g * colorScalar, b * colorScalar);
            foreach (Transform child in transform)
            {
                child.GetComponent<Renderer>().material.color = color;
            }
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
            var initialShape = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
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
        private Material material;
    }
}
