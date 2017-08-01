using UnityEngine;
using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.Control
{
    public class SceneNode : MonoBehaviour, Transformable
    {
		public Creation Shape
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
        public bool Create(GameObject prefab)
        {
            //TODO: detect change, clear old objects, create new, handle array...
            var childObject = Instantiate(prefab) as GameObject;
            childObject.transform.SetParent(transform);
            material = new Material(childObject.GetComponent<Renderer>().material);
            childObject.GetComponent<Renderer>().material = material;
            childObjects.Add(childObject);
            return true;
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
            //transform.localPosition = translation;
            //TODO: handle array...
            foreach (var child in childObjects)
            {
                child.transform.localPosition = translation;
            }
            return true;
		}
		public bool Rotate(float x, float y, float z)
		{
            //TODO: handle array, handle mapping, handle offset/scalar
            var rotation = new Vector3(x * scalar, y * scalar, z * scalar);
            transform.localEulerAngles = rotation;
            /*
            foreach (var child in childObjects)
            {
                child.transform.localEulerAngles = rotation;
            }
            */
            return true;
		}
		public bool Scale(float x, float y, float z)
		{
            //TODO: handle array, handle mapping, handle offset/scalar
            var scale = new Vector3(x * Mathf.Abs(scalar), y * Mathf.Abs(scalar), z * Mathf.Abs(scalar));
            //transform.localScale = scale;
            foreach (var child in childObjects)
            {
                child.transform.localScale = scale;
            }
			return true;
		}
        public bool Color(float r, float g, float b)
        {
            var colorScalar = (scalar + 1.0f) * 0.5f;
            var color = new Color(r * colorScalar, g * colorScalar, b * colorScalar);
            material.color = color;
            return true;
        }
		public SceneNode()
		{
            shape = Creation.CUBE;
            scalar = 1.0f;
            isArrayed = false;
            arraySize = new int[] { 1, 1, 1 };
            arraySpacing = new float[] { 1.0f, 1.0f, 1.0f };
            childObjects = new List<GameObject>();
		}
		private Creation shape;
		private float scalar;
        private bool isArrayed;
        private int[] arraySize;
        private float[] arraySpacing;
        private List<GameObject> childObjects;
        private Material material;
    }
}
