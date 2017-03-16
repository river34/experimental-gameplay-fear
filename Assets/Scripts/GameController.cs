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
	public float playerStrength;
	public float playerFear;
	public int playerComplete;
	public int level;

	// UI
	public GameObject UI_Title;
	public GameObject UI_Block;
	public GameObject UI_End;
	public GameObject UI_Game;
	public GameObject UI_Strength;
	public GameObject UI_Courage;
	public GameObject UI_Complete;
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
	private bool doingSetup;
	private float gameStartDelay = 0.5f;

	// quest
	public QuestManager questManager;

	// intro
	public IntroManager introManeger;

	// state
	public int state;

	// color
	private Color halfBlack = new Color (0, 0, 0, 0.7f);

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

		UI_StrengthBar = UI_Strength.transform.Find ("Bar").gameObject.GetComponent<RectTransform>();
		UI_CourageBar = UI_Courage.transform.Find ("Bar").gameObject.GetComponent<RectTransform>();
		UI_CompleteBar = UI_Complete.transform.Find ("Bar").gameObject.GetComponent<RectTransform>();

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

			if (Input.anyKey)
			{
				StartIntro ();
				state = States.INTRO;
			}
		}
		else if (state == States.INTRO)
		{
			if (UI_Title.GetComponent <Image> ().color.a > Mathf.Epsilon)
			{
				UI_Title.GetComponent <Image> ().color -= new Color (0, 0, 0, Time.deltaTime);
			}

			if (Input.GetKey ("p"))
			{
				EndIntro ();
				state = States.START;
			}
		}
		else if (state == States.START)
		{
			InitGame ();
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
			UI_End.transform.Find ("Text").GetComponent<Text>().text = "You are ready";

			// state = States.END;
			if (Input.GetKey ("p"))
			{
				EndIntro ();
				state = States.START;
			}
		}
		else if (state == States.FAIL)
		{
			UI_End.transform.Find ("Text").GetComponent<Text>().text = "You are not ready";
			state = States.END;
		}
		else if (state == States.END)
		{
			GameEnd ();

			if (Input.GetKey ("space"))
			{
				InitGame ();
			}
		}
	}

	public void FinishLines ()
	{
		if (state == States.INTRO)
		{
			state = States.START;
		}
		else if (state == States.COMPLETE)
		{
			state = States.END;
		}
	}

	public void GameOver ()
	{
		UI_Game.SetActive (false);
		state = States.FAIL;
	}

	public void GameComplete ()
	{
		UI_Game.SetActive (false);
		state = States.COMPLETE;
		StartComplete ();
	}

	void GameEnd ()
	{
		UI_Block.SetActive (false);
		UI_End.SetActive (true);
		Destroy (playerObject);
		Destroy (mapObject);
		playerObject = null;
		mapObject = null;
		camera.enabled = false;
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
			state = States.QUEST;
		}

		doingSetup = true;

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
		questManager.enabled = true;
		playerObject.GetComponent <PlayerController>().enabled = true;
		UI_Block.SetActive (false);
		UI_Game.SetActive (true);
		doingSetup = false;
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
