using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	private MapGenerator map;
	private Transform player;
	private BoxCollider collider;
	private float maxDistance = 1600f;

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
			float maxView = GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerController> ().maxView;
			maxDistance = maxView * maxView * 16;
		}

		if (map == null)
		{
			map = GameObject.FindGameObjectWithTag ("GameController").GetComponent <MapGenerator> ();;
			collider.size = new Vector3 (map.map_width, 1, map.map_height);
		}

		Vector3 distance = player.position - transform.position;

		if (distance.sqrMagnitude > maxDistance)
		{
			map.RemoveMap (transform.position.x, transform.position.z);
			Destroy (gameObject);
		}
	}
}
