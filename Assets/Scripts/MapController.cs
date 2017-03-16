using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	private MapGenerator map;
	private Transform player;
	private BoxCollider collider;

	void Start ()
	{
		collider = gameObject.AddComponent <BoxCollider> ();
		collider.isTrigger = true;
	}

	void Update ()
	{
		if (player == null)
		{
			player = GameObject.FindGameObjectWithTag ("Player").transform;
		}

		if (map == null)
		{
			map = GameObject.FindGameObjectWithTag ("GameController").GetComponent <MapGenerator> ();;
			collider.size = new Vector3 (map.map_width, 1, map.map_height);
		}

		Vector3 distance = player.position - transform.position;

		if (distance.sqrMagnitude > 6400)
		{
			map.RemoveMap (transform.position.x, transform.position.z);
			Destroy (gameObject);
		}
	}
}
