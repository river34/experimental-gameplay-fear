using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {

	public float speed;

	private Transform target;
	private PlayerController player;
	private bool FindPlayer;
	private bool lastFindPlayer;
	private float farDistance;
	private float closeDistance;
	private float angle;
	private float rad;
	private SpriteRenderer render;
	private float reverseMul = 0.001f;
	private float maxScale = 2f;
	private float scaleSpeed = 0.3f;
	private float radSpeed = 0.1f;
	private float maxRad = 1f;
	private Color highClear = Color.clear * 0.75f;
	private Color halfClear = Color.clear * 0.5f;
	private Color lowClear = Color.clear * 0.25f;

	void Awake ()
	{
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		player = target.GetComponent <PlayerController> ();
		render = transform.Find ("Sprite").GetComponent <SpriteRenderer>();
		farDistance = 15f * 15f;
		closeDistance = Random.Range (4f, 16f);
	}

	void Update () {
		Vector3 distance = Vector3.zero;
		distance.x = target.position.x - transform.position.x;
		distance.z = target.position.z - transform.position.z;
		float sqrLen =  distance.sqrMagnitude;

		if (sqrLen < farDistance)
		{
			Vector3 targetPosition = target.position;
			targetPosition.y = transform.position.y;

			Vector3 offset = Vector3.zero;
			offset.x = targetPosition.x - transform.position.x;
			offset.z = targetPosition.z - transform.position.z;
			sqrLen =  offset.sqrMagnitude;

			if (sqrLen < closeDistance)
			{
				FindPlayer = true;
			}
			else
			{
				FindPlayer = false;
			}

			transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);

			if (FindPlayer)
			{
				if (GameController.instance.playerFear > player.high_fear)
				{
					if (transform.localScale.x < maxScale)
					{
						transform.localScale += Vector3.one * GameController.instance.playerFear * reverseMul * Time.deltaTime * scaleSpeed;
					}
				}
				else if (GameController.instance.playerFear > player.mid_fear)
				{
					render.color = Color.Lerp (render.color, halfClear, (1f - GameController.instance.playerFear * reverseMul) * Time.deltaTime);
				}
				else if (GameController.instance.playerFear > player.low_fear)
				{
					render.color = Color.Lerp (render.color, lowClear, (1f - GameController.instance.playerFear * reverseMul) * Time.deltaTime);
				}
				else if (GameController.instance.playerFear >= player.min_fear)
				{
					render.color = Color.Lerp (render.color, Color.clear, (1f - GameController.instance.playerFear * reverseMul) * Time.deltaTime);
				}

				// transform.position = new Vector3 (transform.position.x, Mathf.Sin(Time.time * 5f) * .2f, transform.position.z);
				if (!FindPlayer)
				{
					angle = 0;
					rad = 0;
				}
				angle += Time.deltaTime;
				if (rad < maxRad)
				{
					rad += Time.deltaTime * radSpeed;
				}
				transform.Find ("Sprite").localPosition = new Vector3 (Mathf.Sin(angle)*rad, Mathf.Cos(angle)*rad, Mathf.Cos(angle)*rad);
			}
			else
			{
				transform.Find ("Sprite").localPosition = Vector3.zero;
			}
		}
	}

	void LateUpdate ()
	{
		lastFindPlayer = FindPlayer;

		if (transform.localScale.x < Mathf.Epsilon)
		{
			player.RemoveMonster (gameObject);
			Destroy (gameObject);
		}

		if (render.color.a < Mathf.Epsilon)
		{
			player.RemoveMonster (gameObject);
			Destroy (gameObject);
		}
	}
}
