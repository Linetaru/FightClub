using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LineSmear
{
	public Vector3 verticeBase;
	public Vector3 verticeTip;
}


public class SmearsControllerSword : MonoBehaviour
{
	[SerializeField]
	CharacterBase character = null;

	[SerializeField]
	bool active = true;
	[SerializeField]
	Transform startPoint = null;
	[SerializeField]
	Transform endPoint = null;

	[SerializeField]
	float time = 0.4f;

	[SerializeField]
	int smoothPoint = 2;
	[SerializeField]
	float smoothRatio = 0.25f;


	Mesh mesh;
	List<LineSmear> lineSmears = new List<LineSmear>();

	float t = 0f;
	bool pause = false;


	private void Start()
	{
		mesh = new Mesh();
		//meshRenderer.material = mat;
		GetComponent<MeshFilter>().mesh = mesh;
		this.transform.SetParent(null);
		this.transform.localScale = Vector3.one;
		this.transform.position = Vector3.zero;
		this.transform.eulerAngles = Vector3.zero;

		if (character != null)
		{
			character.SmearsControllerSword = this;
			character.OnMotionSpeed += PauseSmears;
		}

	}

	public void SmearsActive(bool b)
	{
		if (!active && b)
			t = 0f;
		active = b;
	}

	public void PauseSmears(float newValue)
	{
		if (newValue == 0)
			pause = true;
		else
			pause = false;
		t = time;
	}


	private void LateUpdate()
	{
		if (startPoint == null)
			return;
		if (pause == true)
			return;


		if (active)
		{
			lineSmears.Add(new LineSmear());
			lineSmears[lineSmears.Count - 1].verticeBase = startPoint.position;
			lineSmears[lineSmears.Count - 1].verticeTip = endPoint.position;
			if (t > time)
			{
				lineSmears.RemoveAt(0);
			}
			else if (t < time)
			{
				t += Time.deltaTime;
			}
		}
		else
		{
			if(lineSmears.Count != 0)
			{
				lineSmears.Add(new LineSmear());
				lineSmears[lineSmears.Count - 1].verticeBase = startPoint.position;
				lineSmears[lineSmears.Count - 1].verticeTip = endPoint.position;
				lineSmears.RemoveAt(0);
				lineSmears.RemoveAt(0);
			}
		}



		if (lineSmears.Count > 1)
		{
			List<LineSmear> smoothLineSmears = lineSmears;
			for (int i = 0; i < smoothPoint; i++)
			{
				smoothLineSmears = SmoothLine(smoothLineSmears);
			}
			DrawLine(smoothLineSmears);

		}
		else
		{
			mesh.Clear();
		}

		/*float uvRatio = (float)n / pointsToUse.Count;
		newUV[n * 2] = new Vector2(uvRatio, 0);
		newUV[(n * 2) + 1] = new Vector2(uvRatio, 1);*/


	}

	private void OnDestroy()
	{
		if (character != null)
			character.OnMotionSpeed -= PauseSmears;
	}



	private void DrawLine(List<LineSmear> line)
	{
		Vector3[] vertices = new Vector3[line.Count * 2];
		Vector2[] uv = new Vector2[line.Count * 2];
		int[] triangles = new int[(line.Count) * 12];
		Color[] colors = new Color[line.Count * 2];



		for (int i = 0; i < line.Count; i++)
		{
			colors[i * 2] = Color.clear; // Color.Lerp(Color.white, Color.clear, i / lineSmears.Count);
			colors[(i * 2) + 1] = Color.Lerp(Color.clear, Color.white, i / line.Count);

			vertices[i * 2] = line[i].verticeBase;
			vertices[(i * 2) + 1] = line[i].verticeTip;

			float uvRatio = (float)i / line.Count;
			uv[i * 2] = new Vector2(uvRatio, 0);
			uv[(i * 2) + 1] = new Vector2(uvRatio, 1);

			// Les triangles
			// index des vertices
			if (i > 0)
			{
				triangles[(i - 1) * 6] = (i * 2) - 2;       // base
				triangles[((i - 1) * 6) + 1] = (i * 2) - 1; // tip
				triangles[((i - 1) * 6) + 2] = i * 2;       // base

				triangles[((i - 1) * 6) + 3] = (i * 2) + 1; // tip
				triangles[((i - 1) * 6) + 4] = i * 2;       // base
				triangles[((i - 1) * 6) + 5] = (i * 2) - 1; // tip
			}
		}

		// Inert normals
		for (int i = line.Count; i < line.Count * 2; i++)
		{
			/*vertices[i * 2] = lineSmears[i - lineSmears.Count].verticeBase;
			vertices[(i * 2) + 1] = lineSmears[i - lineSmears.Count].verticeTip;

			float uvRatio = (float)(i - lineSmears.Count) / lineSmears.Count;
			uv[i * 2] = new Vector2(uvRatio, 0);
			uv[(i * 2) + 1] = new Vector2(uvRatio, 1);*/

			if (i > line.Count)
			{
				triangles[((i - 1) * 6)] = (i - line.Count) * 2;          // base
				triangles[((i - 1) * 6) + 1] = ((i - line.Count) * 2) - 1; // tip
				triangles[((i - 1) * 6) + 2] = ((i - line.Count) * 2) - 2; // base


				triangles[((i - 1) * 6) + 3] = ((i - line.Count) * 2) - 1; // tip
				triangles[((i - 1) * 6) + 4] = (i - line.Count) * 2;       // base
				triangles[((i - 1) * 6) + 5] = ((i - line.Count) * 2) + 1; // tip
			}
		}
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.colors = colors;
		mesh.uv = uv;
		mesh.triangles = triangles;
	}





	private List<LineSmear> SmoothLine(List<LineSmear> lineToSmooth)
	{
		Vector3 smoothStart;
		Vector3 smoothEnd;
		List<LineSmear> smoothLine = new List<LineSmear>(((lineToSmooth.Count-1) * 3) + 1);

		for (int i = 0; i < lineToSmooth.Count-1; i++)
		{
			//smoothLine.Add(lineToSmooth[i]);
			//debug[0].transform.position = smoothLine[smoothLine.Count - 1].verticeTip;

			smoothStart = lineToSmooth[i].verticeBase + (lineToSmooth[i + 1].verticeBase - lineToSmooth[i].verticeBase) * smoothRatio;
			smoothEnd = lineToSmooth[i].verticeTip + (lineToSmooth[i+1].verticeTip - lineToSmooth[i].verticeTip) * smoothRatio;
			smoothLine.Add(new LineSmear());
			smoothLine[smoothLine.Count - 1].verticeBase = smoothStart;
			smoothLine[smoothLine.Count - 1].verticeTip = smoothEnd;

			smoothStart = lineToSmooth[i+1].verticeBase + (lineToSmooth[i].verticeBase - lineToSmooth[i+1].verticeBase) * smoothRatio;
			smoothEnd = lineToSmooth[i+1].verticeTip + (lineToSmooth[i].verticeTip - lineToSmooth[i+1].verticeTip) * smoothRatio;
			smoothLine.Add(new LineSmear());
			smoothLine[smoothLine.Count - 1].verticeBase = smoothStart;
			smoothLine[smoothLine.Count - 1].verticeTip = smoothEnd;

		}
		smoothLine.Add(lineToSmooth[lineToSmooth.Count-1]);

		return smoothLine;
	}



}
