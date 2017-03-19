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
	public string[] next;		// name of the next quest; null if any
	public float minStrength;
	public float maxStrength;
	public float minFear;
	public float maxFear;
	public bool strengthStar;
	public bool courageStar;
	public bool wisdomTree;
	public bool spiritTree;
	public bool monster;

	public bool isFinished;
	public float startTime;
	public float finishTime;
	public List<GameObject> objects;

	public Quest (int _id, string _name, string _text, string _tag, int _num, bool _isOpen, string[] _next,
		float _minStrength, float _maxStrength, float _minFear, float _maxFear, bool _strengthStar, bool _courageStar, bool _wisdomTree, bool _spiritTree, bool _monster)
	{
		id = _id;
		name = _name;
		text = _text;
		tag = _tag;
		num = _num;
		isOpen = _isOpen;
		next = _next;
		minStrength = _minStrength;
		maxStrength = _maxStrength;
		minFear = _minFear;
		maxFear = _maxFear;
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
		string thisName;
		string[] nextName;

		// quest : strength
		thisName = "Strength";
		nextName = new string[1];
		nextName[0] = "More Strength";
		quest = new Quest (quests.Count, thisName,
		"Ima needs physical strength to explore the forest.\r\nYellow Stars replenish Ima's strength.",
		"Strength", 1, true, nextName,
		-1, -1, -1, -1,
		true, false, false, false, false);
		quests.Add (quest);

		// quest : more strength
		thisName = "More Strength";
		nextName = new string[1];
		nextName[0] = "Wisdom Tree";
		quest = new Quest (quests.Count, thisName,
		"The stronger Ima is, the brighter she will appear. \r\nShift / Space / Right Click to move faster.",
		"Strength", 1, false, nextName,
		1000, 1000, -1, -1,
		true, false, false, false, false);
		quests.Add (quest);

		// quest : find the wisdom tree
		thisName = "Wisdom Tree";
		nextName = new string[1];
		nextName[0] = "Courage";
		quest = new Quest (quests.Count, thisName,
		"However, physical strength is not the only thing Ima needs. \r\nA Kumu can always ask the Wisdom Tree for advice.",
		"WisdomTree", 1, false, nextName,
		-1, -1, -1, -1,
		true, false, true, false, false);
		quests.Add (quest);

		// quest : collect red star
		thisName = "Courage";
		nextName = new string[1];
		nextName[0] = "More Courage";
		quest = new Quest (quests.Count, thisName,
		"Wisdom Tree: One needs courage to survive the Fear Forest. \r\nRed Stars give you courage.",
		"Courage", 1, false, nextName,
		-1, -1, -1, -1,
		true, true, false, false, false);
		quests.Add (quest);

		// quest : courage
		thisName = "More Courage";
		nextName = new string[3];
		nextName[0] = "Ghosts 1";
		nextName[1] = "Ghosts 2";
		nextName[2] = "Ghosts 3";
		quest = new Quest (quests.Count, thisName,
		"Courage lets Ima see things clearly. \r\nThat's exactly what Ima needs in this darkness.",
		"Courage", 1, false, nextName,
		-1, -1, 0, 800,
		true, false, false, false, false);
		quests.Add (quest);

		// quest : ghosts 1
		thisName = "Ghosts 1";
		nextName = new string[1];
		nextName[0] = "Strength Drop";
		quest = new Quest (quests.Count, thisName,
		"Wisdom Tree:  In the Fear Forest there live Ghosts.\r\nGhosts will take away Ima's physical strength.",
		"Monster", 3, false, nextName,
		0, 500, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : ghosts 2
		thisName = "Ghosts 2";
		nextName = new string[1];
		nextName[0] = "Cannot See";
		quest = new Quest (quests.Count, thisName,
		"Wisdom Tree:  In the Fear Forest there live Ghosts.\r\nGhosts will take away Ima's physical strength.",
		"Monster", 3, false, nextName,
		-1, -1, 900, 1000,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : ghosts 3
		thisName = "Ghosts 3";
		nextName = new string[1];
		nextName[0] = "More Ghosts";
		quest = new Quest (quests.Count, thisName,
		"Wisdom Tree:  In the Fear Forest there live Ghosts.\r\nGhosts will take away Ima's physical strength.",
		"Monster", 5, false, nextName,
		-1, -1, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : Strength Drop
		thisName = "Strength Drop";
		nextName = new string[1];
		nextName[0] = "More Ghosts";
		quest = new Quest (quests.Count, thisName,
		"Fear is consuming poor Ima.  Ima's strength is dropping.",
		"Monster", 5, false, nextName,
		-1, -1, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : Cannot See
		thisName = "Cannot See";
		nextName = new string[1];
		nextName[0] = "More Ghosts";
		quest = new Quest (quests.Count, thisName,
		"Ima lost all courage and therefore cannot see anything.",
		"Monster", 5, false, nextName,
		-1, -1, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : More Ghosts
		thisName = "More Ghosts";
		nextName = new string[3];
		nextName[0] = "Everywhere 1";
		nextName[1] = "Everywhere 2";
		nextName[2] = "Everywhere 3";
		quest = new Quest (quests.Count, thisName,
		"More of them are coming...",
		"Monster", 10, false, nextName,
		-1, -1, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : Everywhere 1
		thisName = "Everywhere 1";
		nextName = new string[2];
		nextName[0] = "Solution 1";
		nextName[1] = "Solution 2";
		quest = new Quest (quests.Count, thisName,
		"Ghosts are everywhere.  There is no way to get rid of them...",
		"Monster", 5, false, nextName,
		0, 500, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : Everywhere 2
		thisName = "Everywhere 2";
		nextName = new string[1];
		nextName[0] = "Disappear";
		quest = new Quest (quests.Count, thisName,
		"Ghosts are everywhere.  There is no way to get rid of them...",
		"Monster", 5, false, nextName,
		-1, -1, 0, 500,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : Everywhere 3
		thisName = "Everywhere 3";
		nextName = new string[2];
		nextName[0] = "Solution 1";
		nextName[1] = "Solution 2";
		quest = new Quest (quests.Count, thisName,
		"Ghosts are everywhere.  There is no way to get rid of them...",
		"Monster", 10, false, nextName,
		-1, -1, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : Solution 1
		thisName = "Solution 1";
		nextName = new string[1];
		nextName[0] = "Disappear";
		quest = new Quest (quests.Count, thisName,
		"There must be a way to deal with them.",
		"Monster", 5, false, nextName,
		-1, -1, 0, 300,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : Solution 2
		thisName = "Solution 2";
		nextName = new string[1];
		nextName[0] = "Closer";
		quest = new Quest (quests.Count, thisName,
		"There must be a way to deal with them.",
		"Monster", 10, false, nextName,
		-1, -1, -1, -1,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : Disappear
		thisName = "Disappear";
		nextName = new string[1];
		nextName[0] = "Ask";
		quest = new Quest (quests.Count, thisName,
		"Is that true? Some Ghosts are fading.",
		"Monster", 5, false, nextName,
		800, 1000, 0, 200,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : Closer
		thisName = "Closer";
		nextName = new string[1];
		nextName[0] = "Ask";
		quest = new Quest (quests.Count, thisName,
		"What happens if Ima approaches the Ghosts?",
		"Monster", 5, false, nextName,
		800, 1000, 0, 200,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : secret
		thisName = "Ask";
		nextName = new string[1];
		nextName[0] = "Reflection";
		quest = new Quest (quests.Count, thisName,
		"That is weird.  Maybe Wisdom Tree knows the answer.",
		"WisdomTree", 1, false, nextName,
		-1, -1, -1, -1,
		true, false, true, false, true);
		quests.Add (quest);

		// quest : Reflection
		thisName = "Reflection";
		nextName = new string[1];
		nextName[0] = "Spirit";
		quest = new Quest (quests.Count, thisName,
		"Wisdom Tree:  A true Kumu shall not be afraid of Ghosts. \r\nThey are nothing more than a reflection of your own fear.",
		"Monster", 5, false, nextName,
		950, 1000, 0, 50,
		true, false, false, false, true);
		quests.Add (quest);

		// quest : spirit
		thisName = "Spirit";
		nextName = new string[0];
		quest = new Quest (quests.Count, thisName,
		"Wisdom Tree:  Now go find the Sacred Tree, and spirits will guide you further on.",
		"SpiritTree", 1, false, nextName,
		-1, -1, -1, -1,
		true, false, false, true, false);
		quests.Add (quest);

		if (quests == null || quests.Count <= 0)
		{
			return;
		}

		foreach (Quest quest in quests)
		{
			if (quest.isOpen && !quest.isFinished)
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

		// Debug.Log ("Complete " + quests[id].name);
		quests[id].isFinished = true;
		quests[id].finishTime = Time.deltaTime;
		GameController.instance.level += 1;

		foreach (Quest quest in quests)
		{
			if (quest.isOpen)
			{
				quest.isOpen = false;
			}
		}

		if (quests[id].next.Length > 0)
		{
			foreach (string name in quests[id].next)
			{
				quest = FindByName (name);
				if (quest == null)
				{
					return;
				}
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

		// debug
		foreach (Quest quest in quests)
		{
			if (quest.isOpen)
			{
				// Debug.Log ("Open: " + quest.name);
			}
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
			if (quests[id].minStrength != -1 && quests[id].maxStrength != -1 && quests[id].minFear != -1 && quests[id].maxFear != -1)
			{
				if (player.strength >= quests[id].minStrength && player.strength <= quests[id].maxStrength)
				{
					if (player.fear >= quests[id].minFear && player.fear <= quests[id].maxFear)
					{
						CompleteQuest (id);
						return true;
					}
				}
			}
			else if (quests[id].minStrength != -1 && quests[id].maxStrength != -1)
			{
				if (player.strength >= quests[id].minStrength && player.strength <= quests[id].maxStrength)
				{
					CompleteQuest (id);
					return true;
				}
			}
			else if (quests[id].minFear != -1 && quests[id].maxFear != -1)
			{
				if (player.fear >= quests[id].minFear && player.fear <= quests[id].maxFear)
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

	public int FindIDByName (string name)
	{
		if (quests == null || quests.Count <= 0)
		{
			return -1;
		}

		foreach (Quest quest in quests)
		{
			if (quest.name == name)
			{
				return quest.id;
			}
		}

		return -1;
	}

	public Quest FindByName (string name)
	{
		if (quests == null || quests.Count <= 0)
		{
			return null;
		}

		foreach (Quest quest in quests)
		{
			if (quest.name == name)
			{
				return quest;
			}
		}

		return null;
	}
}
