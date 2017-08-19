using UnityEngine;

namespace GeometrySynth.UI
{
	public class LightController : MonoBehaviour
	{
		public float rotationSpeed;

		void Start()
		{
			isRotating = false;
			rotationAxis = Vector3.up;
		}
		void Update()
		{
			if (Input.GetKeyUp(KeyCode.L))
			{
				isRotating = !isRotating;
			}
			if (isRotating)
			{
				transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
			}
		}
		private bool isRotating;
		private Vector3 rotationAxis;
	}
}