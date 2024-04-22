using UnityEditor;
using UnityEngine;

public class AStarEditor : MonoBehaviour
{
	[MenuItem("AStar/1. Generoi ruudukko")]
	static void GenerateMap()
	{
		for (var x = 0; x < 50; ++x)
		{
			for (var z = 0; z < 50; ++z)
			{
				var node = Instantiate((GameObject)Resources.Load("Node"));

				node.transform.name = $"Node_{x}_{z}";
				node.transform.position = new(x, 0f, z);
			}
		}

		Debug.Log($"Ruudukko ladattu!");
	}

	[MenuItem("AStar/2. Tarkasta esteet")]
	static void CheckObstacles()
	{
		var startFound = false;
		var endFound = false;

		foreach (var node in FindObjectsOfType<AStarNode>())
		{
			if (Physics.Raycast(node.transform.position, Vector3.up, out _))
			{
				node.State = NodeState.Closed;
				node.SetColor(NodeColor.Obstacle);
			}

			if (node.State == NodeState.Start)
			{
				node.SetColor(NodeColor.Start);
				startFound = true;
			}

			if (node.State == NodeState.End)
			{
				node.SetColor(NodeColor.End);
				endFound = true;
			}
		}

		if (!startFound || !endFound)
		{		
			Debug.LogError(!startFound && endFound
				? "Aloituspistettä ei ole asetettu!"
				: startFound && !endFound
				? "Lopetuspistettä ei ole asetettu!"
				: "Aloitus- ja lopetuspistettä ei ole asetettu!");
			return;
		}

		Debug.Log($"Esteet ladattu!");
	}

	[MenuItem("AStar/3. Etsi naapuri nodet")]
	static void SearchNeighborNodes()
	{
		foreach (var node in FindObjectsOfType<AStarNode>())
		{
			foreach (var neighbor in FindObjectsOfType<AStarNode>())
			{
				// Skipataan noden lisäämine naapureihin jos:
				// - Siinä ei voi kulkea
				// - Se on alkupiste
				// - Se on nykyinen node
				if (neighbor.State == NodeState.Closed
					|| neighbor.State == NodeState.Start
					|| neighbor.name == node.name)
				{
					continue;
				}

				// Tarkistetaan viereisen noden etäisyys (alle 1.8 == naapuri)			
				if (Vector3.Distance(node.transform.position, neighbor.transform.position) < 1.8f)
				{
					node.Neighbors.Add(neighbor);
				}
			}
		}

		Debug.Log("Naapurinodet ladattu!");
	}

	[MenuItem("AStar/4. Poista ruudukko")]
	static void DeleteMap()
	{
		foreach (var node in GameObject.FindGameObjectsWithTag("AStarNode"))
		{
			DestroyImmediate(node);
		}
	}
}
