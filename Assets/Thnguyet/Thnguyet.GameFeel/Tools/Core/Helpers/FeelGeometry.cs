using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A helper class to handle geometry related operations    
	/// </summary>    
	public static class FeelGeometry
	{
		// Based on https://answers.unity.com/questions/1019436/get-outeredge-vertices-c.html
		public struct Edge
		{
			public int Vertice1;
			public int Vertice2;
			public int TriangleIndex;
			public Edge(int aV1, int aV2, int aIndex)
			{
				Vertice1 = aV1;
				Vertice2 = aV2;
				TriangleIndex = aIndex;
			}
		}

		public static List<Edge> GetEdges(int[] indices)
		{
			List<Edge> edgeList = new List<Edge>();
			for (int i = 0; i < indices.Length; i += 3)
			{
				int vertice1 = indices[i];
				int vertice2 = indices[i + 1];
				int vertice3 = indices[i + 2];
				edgeList.Add(new Edge(vertice1, vertice2, i));
				edgeList.Add(new Edge(vertice2, vertice3, i));
				edgeList.Add(new Edge(vertice3, vertice1, i));
			}
			return edgeList;
		}

		public static List<Edge> FindBoundary(this List<Edge> edges)
		{
			List<Edge> edgeList = new List<Edge>(edges);
			for (int i = edgeList.Count - 1; i > 0; i--)
			{
				for (int n = i - 1; n >= 0; n--)
				{
					// if we find a shared edge we remove both
					if (edgeList[i].Vertice1 == edgeList[n].Vertice2 && edgeList[i].Vertice2 == edgeList[n].Vertice1)
					{
						edgeList.RemoveAt(i);
						edgeList.RemoveAt(n);
						i--;
						break;
					}
				}
			}
			return edgeList;
		}
		public static List<Edge> SortEdges(this List<Edge> edges)
		{
			List<Edge> edgeList = new List<Edge>(edges);
			for (int i = 0; i < edgeList.Count - 2; i++)
			{
				Edge E = edgeList[i];
				for (int n = i + 1; n < edgeList.Count; n++)
				{
					Edge a = edgeList[n];
					if (E.Vertice2 == a.Vertice1)
					{                        
						if (n == i + 1)
						{
							// if they're already in order, we move on
							break;
						}
						else
						{
							// otherwise we swap
							edgeList[n] = edgeList[i + 1];
							edgeList[i + 1] = a;
							break;
						} 
					}
				}
			}
			return edgeList;
		}
	}
}
