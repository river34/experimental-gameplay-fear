using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class States {
	public const int TITLE = 0;
	public const int INTRO = 10;
	public const int START = 15;
	public const int QUEST = 20;
	public const int COMPLETE = 30;
	public const int FAIL = 40;
	public const int END = 50;
}

public class GameController : MonoBehaviour {

	public static GameController instance = null;
	public CameraController camera;
	public GameObject player;

	[HideInInspector]
	public float playerStrength;
	[HideInInspector]
	public float playerFear;
	[HideInInspector]
	public int playerComplete;
	[HideInInspector]
	public int level;

	// UI
	public GameObject UI_Title;
	public GameObject UI_Block;
	public GameObject UI_End;
	public GameObject UI_Game;
	public GameObject UI_Strength;
	public GameObject UI_Courage;
	public GameObject UI_Complete;
	public GameObject UI_Nav;
	public Image UI_Mask;
	private RectTransform UI_StrengthBar;
	private RectTransform UI_CourageBar;
	private RectTransform UI_CompleteBar;
	private float num2bar = 0.001f * 40;
	private float reverseMul = 0.001f;

	[HideInInspector]
	public float map_width;
	[HideInInspector]
	public float map_height;

	private GameObject playerObject;
	private GameObject mapObject;
	private MapGenerator mapGenerator;
	// private bool doingSetup;
	private float gameStartDelay = 0.5f;

	// quest
	[HideInInspector]
	public QuestManager questManager;

	// intro
	[HideInInspector]
	public IntroManager introManeger;

	// sound
	[HideInInspector]
	public SoundManager soundManager;

	// state
	// [HideInInspector]
	public int state;

	// color
	private Color halfBlack = new Color (0, 0, 0, 0.7f);

	// time control
	private float time;

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		mapGenerator = GetComponent <MapGenerator>();
		questManager = GetComponent <QuestManager>();
		introManeger = GetComponent <IntroManager>();
		soundManager = GetComponent <SoundManager>();

		UI_StrengthBar = UI_Strength.transform.Find ("Bar").gameObject.GetComponent<RectTransform>();
		UI_CourageBar = UI_Courage.transform.Find ("Bar").gameObject.GetComponent<RectTransform>();
		UI_CompleteBar = UI_Complete.transform.Find ("Bar").gameObject.GetComponent<RectTransform>();

		time = Time.time;
		state = States.TITLE;
	}

	void Update ()
	{
		if (state == States.TITLE)
		{
			if (!UI_Title.activeSelf)
			{
				UI_Title.SetActive (true);
			}

			if (Time.time - time > 0.5f && Input.anyKey)
			{
				StartIntro ();
				time = Time.time;
				state = States.INTRO;
			}
		}
		else if (state == States.INTRO)
		{
			if (UI_Title.GetComponent <Image> ().color.a > Mathf.Epsilon)
			{
				UI_Title.GetComponent <Image> ().color -= new Color (0, 0, 0, Time.deltaTime);
			}

			if (Time.time - time > 0.5f && Input.GetKey ("p"))
			{
				EndIntro ();
				soundManager.PlayBackground ();
				time = Time.time;
				state = States.START;
			}
		}
		else if (state == States.START)
		{
			InitGame ();
			time = Time.time;
			state = States.QUEST;
		}
		else if (state == States.QUEST)
		{
			UI_StrengthBar.sizeDelta = new Vector2 (playerStrength * num2bar, UI_StrengthBar.sizeDelta.y);
			UI_CourageBar.sizeDelta = new Vector2 ((1000 - playerFear) * num2bar, UI_CourageBar.sizeDelta.y);
			UI_CompleteBar.sizeDelta = new Vector2 (playerComplete * num2bar, UI_CompleteBar.sizeDelta.y);
			UI_Mask.color = Color.Lerp (Color.clear, halfBlack, playerFear * reverseMul);
		}
		else if (state == States.COMPLETE)
		{
			string[] completes = new string[1];
			completes[0] = "May courgae always be with you";
			UI_End.transform.Find ("Text").GetComponent<Text>().text = completes [Random.Range (0, completes.Length)];
			UI_End.transform.Find ("Restart").GetComponent<Text>().text = "Space to replay";

			if (Time.time - time > 0.5f && Input.GetKey ("p"))
			{
				EndIntro ();
				time = Time.time;
				state = States.END;
			}
		}
		else if (state == States.FAIL)
		{
			string[] fails = new string[2];
			fails[0] = "Fear is temporary. You can do better";
			fails[1] = "A Kumu will never give up and neither shall you";
			UI_End.transform.Find ("Text").GetComponent<Text>().text = fails [Random.Range (0, fails.Length)];
			UI_End.transform.Find ("Restart").GetComponent<Text>().text = "Space to try again";
			time = Time.time;
			state = States.END;
		}
		else if (state == States.END)
		{
			GameEnd ();
		}
	}

	public void FinishLines ()
	{
		if (state == States.INTRO)
		{
			time = Time.time;
			state = States.START;
		}
		else if (state == States.COMPLETE)
		{
			time = Time.time;
			state = States.END;
		}
	}

	public void GameOver ()
	{
		Destroy (playerObject);
		// Destroy (mapObject);
		playerObject = null;
		mapObject = null;
		UI_Game.SetActive (false);
		time = Time.time;
		state = States.FAIL;
	}

	public void GameComplete ()
	{
		Destroy (playerObject);
		// Destroy (mapObject);
		playerObject = null;
		mapObject = null;
		UI_Game.SetActive (false);
		soundManager.StopBackground ();
		time = Time.time;
		state = States.COMPLETE;
		StartComplete ();
	}

	void GameEnd ()
	{
		UI_Block.SetActive (false);
		UI_End.SetActive (true);
		camera.enabled = false;
		questManager.enabled = false;

		if (Time.time - time > 1 && Input.GetKey ("space"))
		{
			Invoke ("InitGame", gameStartDelay);
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization ()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static private void OnSceneLoaded (Scene arg0, LoadSceneMode arg1)
    {
        instance.InitGame ();
    }

	void InitGame ()
	{
		if (state == States.END)
		{
			time = Time.time;
			state = States.QUEST;
		}

		// doingSetup = true;

		level = 1;

		UI_Block.SetActive (true);
		UI_End.SetActive (false);
		UI_Game.SetActive (false);

		if (playerObject == null)
			playerObject = Instantiate (player);

		camera.player = playerObject.transform;
		camera.InitCamera ();
		camera.enabled = true;

		if (mapObject == null)
			mapObject = new GameObject ("Map");

		playerStrength = 500;
		playerFear = 1000;
		playerComplete = 0;

		mapGenerator.InitMap ();
		mapGenerator.GenerateMap (0, 0);
		map_width = mapGenerator.map_width;
		map_height = mapGenerator.map_height;

		questManager.enabled = true;
		questManager.InitQuest ();

		Invoke ("StartQuest", gameStartDelay);
	}

	void StartIntro ()
	{
		introManeger.enabled = true;
		introManeger.Init ("Intro");
		introManeger.StartLine ();
	}

	void EndIntro ()
	{
		UI_Title.SetActive (false);
		introManeger.End ();
	}

	void StartComplete ()
	{
		UI_Block.SetActive (true);
		introManeger.enabled = true;
		introManeger.Init ("Complete");
		introManeger.StartLine ();
	}

	void StartQuest ()
	{
		playerObject.GetComponent <PlayerController>().enabled = true;
		UI_Block.SetActive (false);
		UI_Game.SetActive (true);
		// doingSetup = false;
	}

	public void GenerateMap (float offset_x, float offset_y)
	{
		mapGenerator.GenerateMap (offset_x, offset_y);
	}

	public Map FindMap (float x, float y)
	{
		return mapGenerator.FindMap (x, y);
	}
}
