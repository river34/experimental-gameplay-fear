using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest
{
	// assigned values
	public int id;				// index
	public string name;			// give it a name
	public string text;			// short description
	public string tag;			// tag of objects to interact with
	public int num;				// num of objects to interact with
	public bool isOpen;			// is available now
	public int prerequisite;	// index of the previous quest; -1 if any
	public int next;			// index of the next quest; null if any
	public float strength;
	public float fear;
	public bool strengthStar;
	public bool courageStar;
	public bool wisdomTree;
	public bool spiritTree;
	public bool monster;

	public bool isFinished;
	public float startTime;
	public float finishTime;
	public List<GameObject> objects;

	public Quest (int _id, string _name, string _text, string _tag, int _num, bool _isOpen, int _prerequisite, int _next, float _strength, float _fear, bool _strengthStar, bool _courageStar, bool _wisdomTree, bool _spiritTree, bool _monster)
	{
		id = _id;
		name = _name;
		text = _text;
		tag = _tag;
		num = _num;
		isOpen = _isOpen;
		prerequisite = _prerequisite;
		next = _next;
		strength = _strength;
		fear = _fear;
		strengthStar = _strengthStar;
		courageStar = _courageStar;
		wisdomTree = _wisdomTree;
		spiritTree = _spiritTree;
		monster = _monster;

		isFinished = false;
		startTime = Time.deltaTime;
		objects = new List<GameObject> ();
	}
}

public class QuestManager : MonoBehaviour {

	public static QuestManager instance = null;

	public List<Quest> quests;

	public GameObject UI_Quest;

	private Text questName;
	private Text questText;
	private Quest quest;
	private MapGenerator map;

	void Awake ()
	{
		map = gameObject.GetComponent<MapGenerator> ();
		questName = UI_Quest.transform.Find ("Name").gameObject.GetComponent<Text>();
		questText = UI_Quest.transform.Find ("Text").gameObject.GetComponent<Text>();

		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}

		quests = new List<Quest> ();

		InitQuest ();
	}

	public void InitQuest ()
	{
		quests.Clear ();

		// quest 0 : collect yellow star
		quest = new Quest (quests.Count, "Strength 1", "Ima needs physical strength to explore the forest.\r\nYellow Stars can help to increase that.  Now, find a Yellow Star.",
		"Strength", 1, true, quests.Count-1, quests.Count+1, -1, -1,
		true, false, false, false, false);
		quests.Add (quest);

		// quest 1 : strength
		quest = new Quest (quests.Count, "Strength 2", "Nice!  With enough physical strengh, Ima's existence becomes clearer.",
		"Strength", 1, false, quests.Count-1, quests.Count+1, 1000, -1,
		true, false, false, false, false);
		quests.Add (quest);

		// quest 2 : find the wisdom tree
		quest = new Quest (quests.Count, "Wisdom", "But physical strength is not the only thing that is needed.\r\nIf a Kumu has questions, he or she will seek help from the Wisdom Tree.",
		"WisdomTree", 1, false, quests.Count-1, quests.Count+1, -1, -1,
		true, false, true, false, false);
		quests.Add (quest);

		// quest 3 : collect red star
		quest = new Quest (quests.Count, "Courage 1", "Wisdom Tree says, a true Kumu needs courage to survive the Fear Forest.\r\nCourage can be gained from Red Stars.  Now, find a Red Star.",
		"Courage", 1, false, quests.Count-1, quests.Count+1, -1, -1,
		true, true, false, false, false);
		quests.Add (quest);

		// quest 4 : courage
		quest = new Quest (quests.Count, "Courage 2", "Courage lets Ima see things more clear. That's excatly what Ima needs now.",
		"Courage", 1, false, quests.Count-1, quests.Count+1, -1, 800,
		true, false, false, false, false);
		quests.Add (quest);

		// quest 5 : ghosts
		quest = new Quest (quests.Count, "Ghosts", "Wisdom Tree also says, in the Fear Forest there live Ghosts.  If Ima gets too close, \r\nthey will take away Ima's physical strength.  Shift / Space / Right Click to run.",
		"Monster", 5, false, quests.Count-1, quests.Count+1, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest 6 : fear
		quest = new Quest (quests.Count, "Fear", "Fear is consuming poor Ima.  Ima's vision gets worse and worse, so does Ima's strength.",
		"Monster", 10, false, quests.Count-1, quests.Count+1, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest 7 : stay with ghosts
		quest = new Quest (quests.Count, "Stay with Ghosts 1", "Ghosts are everywhere.  There is no way to get rid of them. Is there?",
		"Monster", 20, false, quests.Count-1, quests.Count+1, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest 8 : stay with ghosts
		quest = new Quest (quests.Count, "Stay with Ghosts 2", "Wait, some Ghosts are fading away.",
		"Monster", 25, false, quests.Count-1, quests.Count+1, -1, 800,
		true, false, false, false, true);
		quests.Add (quest);

		// quest 9 : stay with ghosts
		quest = new Quest (quests.Count, "Stay with Ghosts 3", "Whatever Ima did works! Ghosts are fading even more.",
		"Monster", 25, false, quests.Count-1, quests.Count+1, -1, 600,
		true, false, false, false, true);
		quests.Add (quest);

		// quest 10 : stay with ghosts
		quest = new Quest (quests.Count, "Stay with Ghosts 4", "What if Ima can find a way to be with them, without losing too much strength?",
		"Monster", 30, false, quests.Count-1, quests.Count+1, -1, 300,
		true, false, false, false, true);
		quests.Add (quest);

		// quest 11 : ask for the secret of ghosts
		quest = new Quest (quests.Count, "Secret of Ghosts", "That is weird.  Maybe Wisdom Tree knows the answer.",
		"WisdomTree", 1, false, quests.Count-1, quests.Count+1, -1, -1,
		true, false, true, false, true);
		quests.Add (quest);

		// quest 12 : find the secret of ghosts
		quest = new Quest (quests.Count, "Find Secret", "Wisdom Tree: A true Kumu will not fear Ghosts. They are just a reflection of your own fear. \r\nLet them stay with you, and find your courage from fear.",
		"Monster", 35, false, quests.Count-1, quests.Count+1, -1, 0,
		true, false, false, false, true);
		quests.Add (quest);

		// quest 13 : find spirit tree
		quest = new Quest (quests.Count, "Sprite", "Is that the secret of courage? Maybe Spirit Tree knows.",
		"SpiritTree", 1, false, quests.Count-1, -1, -1, -1,
		true, false, false, true, true);
		quests.Add (quest);

		// foreach (Quest quest in quests)
		// {
		// 	// Debug.Log (quest.id + " : " + quest.next);
		// 	Debug.Log (quest.id + " : " + quest.text);
		// }

		enabled = false;
	}

	void OnEnable ()
	{
		if (quests.Count <= 0)
		{
			return;
		}

		foreach (Quest quest in quests)
		{
			if ((quest.prerequisite < 0 || quest.isOpen) && !quest.isFinished)
			{
				UI_Quest.SetActive (true);
				quest.isOpen = true;
				questName.text = quest.name;
				questText.text = quest.text;
				StartQuest (quest.id);
				break;
			}
		}
	}

	public void CompleteQuest (int id)
	{
		if (id > quests.Count)
		{
			return;
		}

		if (quests[id].isFinished || !quests[id].isOpen)
		{
			return;
		}

		quests[id].isFinished = true;
		quests[id].finishTime = Time.deltaTime;

		GameController.instance.level += 1;

		if (quests[id].next != -1)
		{
			if (quests[id].next < quests.Count)
			{
				quest = quests[quests[id].next];
				quest.isOpen = true;
				questName.text = quest.name;
				questText.text = quest.text;
				StartQuest (quest.id);
			}
		}
		else
		{
			GameController.instance.GameComplete ();
		}
	}

	void StartQuest (int id)
	{
		if (id > quests.Count)
		{
			return;
		}

		if (quests[id].isFinished || !quests[id].isOpen)
		{
			return;
		}

		if (quests[id].strengthStar)
		{
			map.SetNoStrength (false);
		}
		else
		{
			map.SetNoStrength (true);
		}

		if (quests[id].courageStar)
		{
			map.SetNoCourage (false);
		}
		else
		{
			map.SetNoCourage (true);
		}

		if (quests[id].wisdomTree)
		{
			map.SetNoWisdomTree (false);
		}
		else
		{
			map.SetNoWisdomTree (true);
		}

		if (quests[id].spiritTree)
		{
			map.SetNoSpiritTree (false);
		}
		else
		{
			map.SetNoSpiritTree (true);
		}

		if (quests[id].monster)
		{
			map.SetNoMonster (false);
		}
		else
		{
			map.SetNoMonster (true);
		}

		quests[id].startTime = Time.time;
	}

	public bool CheckForComplete (int id, PlayerController player)
	{
		if (id > quests.Count)
		{
			return false;
		}

		if (quests[id].isFinished || !quests[id].isOpen)
		{
			return false;
		}

		if (quests[id].objects.Count >= quests[id].num)
		{
			if (quests[id].strength != -1)
			{
				if (player.strength >= quests[id].strength)
				{
					CompleteQuest (id);
					return true;
				}
			}
			else if (quests[id].fear != -1)
			{
				if (player.fear <= quests[id].fear)
				{
					CompleteQuest (id);
					return true;
				}
			}
			else
			{
				CompleteQuest (id);
				return true;
			}
		}

		return false;
	}
}
