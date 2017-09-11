using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scrollParallax : MonoBehaviour {
	public float speed;
	private List<Transform> backgroundPart = new List<Transform>();
	private List<List<Transform>> backgroundsContinue = new List<List<Transform>>();
	private List<float> limits = new List<float>();
	private List<float> origins = new List<float>();


	void Start()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.GetComponent<Transform>() != null)
			{
				backgroundPart.Add(child);

				List<Transform> sprites = new List<Transform> ();
				for (int loopSprites = 0; loopSprites < child.childCount; loopSprites++) {
					var loopLayer = child.GetChild (loopSprites);
					sprites.Add(loopLayer);
				} 
				sprites = sprites.OrderBy (t => t.position.x).ToList ();
				backgroundsContinue.Add (sprites);
				var size = sprites.LastOrDefault ().position.x - sprites.FirstOrDefault ().position.x;
				limits.Add (size);
				float origin = sprites.FirstOrDefault ().position.x;
				origins.Add (origin);
				Debug.Log ("size " + size + " "+origin);
			}
		}
		backgroundPart = backgroundPart.OrderByDescending (t => t.position.x).ToList ();
	}

	void Update()
	{
		float layerSpeed = speed;
		int position = 0;
		Debug.Log (backgroundPart.Count + " " + backgroundsContinue.Count + " " + limits.Count);
		foreach (var transform in backgroundPart) {
			transform.Translate (Vector2.left * Time.deltaTime * layerSpeed);	
			layerSpeed = layerSpeed * 2 / 3;
			checkLayers (backgroundsContinue [position],limits[position], origins[position]);
			position++;
		}
	}

	void checkLayers (List<Transform> list, float limit, float origin)
	{

		var first = list.FirstOrDefault();
		if (first != null)
		{
			if (first.position.x < origin - limit)
			{
				var last = list.LastOrDefault();
				first.position = new Vector3(last.position.x + limit, first.position.y, first.position.z);

				list.Remove(first);
				list.Add(first);
			}
		}
	}
}
