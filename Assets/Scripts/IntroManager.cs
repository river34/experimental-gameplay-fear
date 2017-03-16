using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour {

	public GameObject UI_Intro;
	private Text text;
	private List<string> lines;
	private float interval = 0.25f;
	private int current;
	private GameController game;
	private float startTime;

	void Awake ()
	{
		text = UI_Intro.transform.Find ("Text").GetComponent <Text> ();

		game = GetComponent <GameController> ();

		lines = new List<string> ();

		startTime = -1;

		enabled = false;
	}

	public void Init (string tag)
	{
		lines.Clear ();

		if (tag == "Intro")
		{
			lines.Add ("Ima, it is time now. you are no longer a child.");
			lines.Add ("You need to prove that you are a true Kumu.");
			lines.Add ("Under the stars, you will go to the dark forest.");
			lines.Add ("You will discover the secret of courage and fear.");
			lines.Add ("Then, and only then, you will become a Kumu.");
		}
		else if (tag == "Complete")
		{
			lines.Add ("Ima, you are a true Kumu now.");
			lines.Add ("The dark forest has witnessed your fear and courage.");
			lines.Add ("You already know all the things a Kumu needs to know.");
			lines.Add ("Now, you need to bring back the knowledge to your tribe.");
			lines.Add ("It is time to go home.");
		}
	}

	public void StartLine ()
	{
		current = -1;
		startTime = Time.time;
	}

	void Update ()
	{
		if (startTime < 0)
		{
			return;
		}

		if (!UI_Intro.activeSelf)
		{
			UI_Intro.SetActive (true);
		}

		int next = (int) Mathf.Floor ((Time.time - startTime) * interval);
		if (current != next && next < lines.Count)
		{
			current = next;
			text.text = lines [next];
		}
		if (next >= lines.Count)
		{
			game.FinishLines ();
			UI_Intro.SetActive (false);
			enabled = false;
		}
	}

	public void End ()
	{
		startTime = -1;
		if (UI_Intro.activeSelf)
		{
			UI_Intro.SetActive (false);
		}
		enabled = false;
	}
}
