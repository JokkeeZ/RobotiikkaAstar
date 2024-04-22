using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
	private readonly List<AStarNode> openList = new();
	private readonly List<AStarNode> closedList = new();
	private AStarNode startNode;
	private AStarNode endNode;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
		{
			Debug.Log("Polun etsintä aloitettu!");
			StartCoroutine(nameof(StartPathfinder));
		}
    }

	private IEnumerator StartPathfinder()
	{
		startNode = FindObjectsOfType<AStarNode>().First(x => x.State == NodeState.Start);
		endNode = FindObjectsOfType<AStarNode>().First(x => x.State == NodeState.End);

		startNode.G = CalculateDistanceCost(startNode, endNode);

		startNode.SetColor(NodeColor.OpenList);
		openList.Add(startNode);

		while (openList.Count > 0)
		{
			var current = openList.OrderBy(x => x.F).First();
			current.SetColor(NodeColor.ClosedList);

			openList.Remove(current);
			closedList.Add(current);

			if (current.State == NodeState.End)
			{
				Debug.Log("Loppupiste saavutettu! Reitti löytyi!");
				BuildPath();
				yield break;
			}

			foreach (var neighbor in current.Neighbors)
			{
				if (closedList.Contains(neighbor))
				{
					continue;
				}

				var newGScore = current.G + CalculateDistanceCost(current, neighbor);

				// Jos uusi G on pienempi kuin naapurin G
				// TAI naapuri ei ole vielä open listassa.
				if (newGScore < neighbor.G || !openList.Contains(neighbor))
				{
					neighbor.G = newGScore;
					neighbor.H = CalculateDistanceCost(neighbor, endNode);
					neighbor.F = neighbor.G + neighbor.H;
					neighbor.Parent = current;

					if (!openList.Contains(neighbor))
					{
						neighbor.SetColor(NodeColor.OpenList);
						openList.Add(neighbor);
					}
				}
			}

			yield return new WaitForSeconds(0.005f);
		}
	}

	private void BuildPath()
	{
		endNode.SetColor(NodeColor.Path);

		var node = endNode.Parent;
		while (node.name != startNode.name)
		{
			node.SetColor(NodeColor.Path);
			node = node.Parent;
		}
	}

	private float CalculateDistanceCost(AStarNode n1, AStarNode n2)
	{
		var x = Mathf.Abs(n2.transform.position.x - n1.transform.position.x);
		var z = Mathf.Abs(n2.transform.position.z - n1.transform.position.z);

		var diagonalCost = 14 * Mathf.Min(x, z);
		var straightCost = 10 * (Mathf.Max(x, z) - Mathf.Min(x, z));

		return diagonalCost + straightCost;
	}
}
