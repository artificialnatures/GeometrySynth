﻿using UnityEngine;
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
			set 
            {
                if (value != shape)
                {
                    Debug.Log("Setting shape...");
                    shape = value;
                    prefab = controller.GetPrefabForShape(shape);
                    DestroyChildren();
                    CreateChild(Vector3.zero);
                    arraySize = new int[] { 1, 1, 1 };
                }
            }
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
                DestroyChildren();
                arraySpacing = new float[] { scale.x, scale.y, scale.z };
                var startX = -((arraySize[0] * arraySpacing[0]) / 2.0f);
                var startY = -((arraySize[1] * arraySpacing[1]) / 2.0f);
                var startZ = -((arraySize[2] * arraySpacing[2]) / 2.0f);
                var startingTranslation = Vector3.zero;
                for (int x = 0; x < countX; x++)
                {
                    for (int y = 0; y < countY; y++)
                    {
                        for (int z = 0; z < countZ; z++)
                        {
                            startingTranslation = new Vector3(
                                startX + (x * arraySpacing[0]),
                                startY + (y * arraySpacing[1]),
                                startZ + (z * arraySpacing[2])
                            );
                            CreateChild(startingTranslation);
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
            if (isArrayed)
            {
                foreach (var child in children)
                {
                    child.Translate(translation);
                }
            } else {
                transform.position = translation;
            }
            scalar = 1.0f;
			return true;
		}
		public bool Rotate(float x, float y, float z)
		{
            rotation = Quaternion.Euler(x * scalar, y * scalar, z * scalar);
            if (isArrayed)
            {
                foreach (var child in children)
                {
                    child.Rotate(rotation);
                }
            } else {
                transform.localRotation = rotation;
            }
            scalar = 1.0f;
            return true;
		}
		public bool Scale(float x, float y, float z)
		{
            scale = new Vector3(x * scalar, y * scalar, z * scalar);
            if (isArrayed)
            {
                foreach (var child in children)
                {
                    child.Scale(scale);
                }
            } else {
                transform.localScale = scale;
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
        public void Initialize(SceneController sceneController, Creator shapeModule, GameObject shapePrefab)
		{
            controller = sceneController;
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
            children = new List<ShapeNode>();
            CreateChild(Vector3.zero);
		}

        private void DestroyChildren()
        {
			foreach (var child in children)
			{
				Destroy(child.gameObject);
			}
			children.Clear();
        }

        private void CreateChild(Vector3 startingTranslation)
        {
            var childShape = Instantiate(prefab, startingTranslation, Quaternion.identity) as GameObject;
			var shapeNode = childShape.AddComponent<ShapeNode>();
			children.Add(shapeNode);
			childShape.transform.SetParent(transform);
        }

        private SceneController controller;
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
