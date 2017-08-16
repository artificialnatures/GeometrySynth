using UnityEngine;

public class ShapeNode : MonoBehaviour 
{
	public Vector3 StartingPosition
    {
        get { return startingPosition; }
        set { startingPosition = value; }
    }
    public void Translate(Vector3 nodeTranslation)
    {
        transform.localPosition = startingPosition + nodeTranslation;
    }
    public void Rotate(Quaternion nodeRotation)
    {
        transform.localRotation = nodeRotation;
    }
    public void Scale(Vector3 nodeScale)
    {
        transform.localScale = nodeScale;
    }
    public void ApplyColor(Color nodeColor)
    {
        gameObject.GetComponent<Renderer>().material.color = nodeColor;
    }

    void Start () 
    {
        startingPosition = Vector3.zero;
	}
	
	void Update () 
    {
	    	
	}

    private Vector3 startingPosition;
}