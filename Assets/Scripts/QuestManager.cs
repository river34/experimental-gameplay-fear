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
	public float fear;

	public bool isFinished;
	public float startTime;
	public float finishTime;
	public List<GameObject> objects;

	public Quest (int _id, string _name, string _text, string _tag, int _num, bool _isOpen, int _prerequisite, int _next, float _fear)
	{
		id = _id;
		name = _name;
		text = _text;
		tag = _tag;
		num = _num;
		isOpen = _isOpen;
		prerequisite = _prerequisite;
		next = _next;
		fear = _fear;

		isFinished = false;
		startTime = Time.deltaTime;
		objects = new List<GameObject> ();
	}

	public Quest (int _id, string _name, string _text, string _tag, int _num, bool _isOpen, int _prerequisite, int _next)
	{
		id = _id;
		name = _name;
		text = _text;
		tag = _tag;
		num = _num;
		isOpen = _isOpen;
		prerequisite = _prerequisite;
		next = _next;

		fear = -1;
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
		quest = new Quest (quests.Count, "Strength", "Ima needs physical strength to explore the forest.\r\nYellow Stars can help to increase Ima's strength.  Now, find a Yellow Star.",
		"Strength", 1, true, quests.Count-1, quests.Count+1);
		quests.Add (quest);

		// quest 1 : find the wisdom tree
		quest = new Quest (quests.Count, "Wisdom", "Nice!  But physical strength is not the only thing that is needed.\r\nIf a Kumu has questions, he or she will seek help from the Wisdom Tree.",
		"WisdomTree", 1, false, quests.Count-1, quests.Count+1);
		quests.Add (quest);

		// quest 2 : collect red star
		quest = new Quest (quests.Count, "Courage", "Wisdom Tree says, a true Kumu needs courage to survive the fear forest.\r\nCourage can be gained from Red Stars.  Now, find a Red Star.",
		"Courage", 1, false, quests.Count-1, quests.Count+1);
		quests.Add (quest);

		// quest 3 : ghosts
		quest = new Quest (quests.Count, "Ghosts", "Wisdom Tree also says, in the fear forest there live Ghosts.  If Ima gets too close, \r\nthey will take away Ima's physical strength.  Space / Right Click to run away from Ghosts.",
		"Monster", 5, false, quests.Count-1, quests.Count+1);
		quests.Add (quest);

		// quest 4 : stay with ghosts
		quest = new Quest (quests.Count, "Stay with Ghosts", "Ghosts are everywhere.  There is no way to get rid of them.",
		"Monster", 10, false, quests.Count-1, quests.Count+1, 500);
		quests.Add (quest);

		// quest 5 : ask for the secret of ghosts
		quest = new Quest (quests.Count, "Secret of Ghosts", "There is something wield.  Maybe Wisdom Tree knows about it.",
		"WisdomTree", 1, false, quests.Count-1, quests.Count+1);
		quests.Add (quest);

		// quest 6 : find the secret of ghosts
		quest = new Quest (quests.Count, "Find Secret", "Wisdom Tree: A true Kumu will not fear Ghosts. They are just a reflection of your own fear. \r\nLet them stay with you, and find your courage from fear.",
		"Monster", 20, false, quests.Count-1, quests.Count+1, 0);
		quests.Add (quest);

		// quest 7 : find spirit tree
		quest = new Quest (quests.Count, "Sprite", "Is that the secret of courage? Maybe Spirit Tree knows.",
		"SpiritTree", 1, false, quests.Count-1, -1);
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
		// Debug.Log (id + " : " + quests[id].finishTime);

		GameController.instance.level += 1;

		if (quests[id].next != -1)
		{
			if (quests[id].next < quests.Count)
			{
				quest = quests[quests[id].next];
				quest.isOpen = true;
				questName.text = quest.name;
				questText.text = quest.text;
				// Debug.Log (quest.id + " : start");
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

		if (id == 0)
		{
			map.SetNoStrength (false);
		}
		else if (id == 1)
		{
			map.SetNoWisdomTree (false);
			map.SetNoCourage (false);
		}
		else if (id == 3)
		{
			map.SetNoCourage (true);
			map.SetNoMonster (false);
		}
		else if (id == 6)
		{
			map.SetNoSpiritTree (false);
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

		// Debug.Log (id + " : " + quests[id].objects.Count + ", " + quests[id].num + ", (" + quests[id].fear + ")");
		if (quests[id].objects.Count >= quests[id].num)
		{
			if (quests[id].fear != -1)
			{
				if (player.fear <= quests[id].fear)
				{
					// Debug.Log (id + " : complete");
					CompleteQuest (id);
					return true;
				}
			}
			else
			{
				// Debug.Log (id + " : complete");
				CompleteQuest (id);
				return true;
			}
		}

		return false;
	}
}
