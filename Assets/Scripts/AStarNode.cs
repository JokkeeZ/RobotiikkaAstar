using System.Collections.Generic;
using UnityEngine;

public enum NodeColor
{
	Start,
	ClosedList,
	Obstacle,
	End,
	OpenList,
	Open,
	Path
}

public enum NodeState
{
	Open,
	Start,
	End,
	Closed
}

public class AStarNode : MonoBehaviour
{
	public NodeState State;
	public float F;
	public float G;
	public float H;
	public AStarNode Parent;

	public readonly List<AStarNode> Neighbors = new();
	public Material[] ColorMaterials;

	public void SetColor(NodeColor color) 
		=> GetComponent<MeshRenderer>().material = ColorMaterials[(int)color];
}
