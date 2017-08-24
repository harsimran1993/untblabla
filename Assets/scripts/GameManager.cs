using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;
	public Player[] player = new Player[4];
	public GameObject turnObj,timeObj,myTeamObj,MenuUI,GameUI,winUI,CountUI,diceContainer,diceObj;
	public float turnTime=40;

	private int currentID,myID,startRolls=0;
	private state gameState=state.Pause;
	private float timer,diceTimer=0;
	private Text turn,time,countUI,myTeam;
	private Vector3 currentFaceRotation;
	private BoardManager boardScript;

	private enum state{Pause,Start,Win,MyTurn,diceroll,selectPawn,Move,wait,nextturn};

	private string[] teams ={"YELLOW","GREEN","RED","BLUE"};
	private Color[] teamColor = { Color.yellow, Color.green, Color.red, Color.blue };
	private Vector3[] face;

	 void Awake()
        {
            //Check if instance already exists
            if (instance == null)
                
                //if not, set instance to this
                instance = this;
            
            //If instance already exists and it's not this:
            else if (instance != this)
                
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
            
            //Get a component reference to the attached BoardManager script
            boardScript = GetComponent<BoardManager>();
            
            //Call the InitGame function to initialize the first level 
            InitGame();
        }
	void InitGame()
	{
		//Call the SetupScene function of the BoardManager script, pass it current level number.
		boardScript.init();
	}

	public void selectPlayer(int id){
		/*player [0] = new Player ("playerA",0,id==0?true:false);
		player [1] = new Player ("playerB",0,id==1?true:false);
		player [2] = new Player ("playerC",0,id==2?true:false);
		player [3] = new Player ("playerD",0,id==3?true:false);*/
		myID = id;
		currentID = -1;
		//start game countdown
		MenuUI.SetActive(false);
		myTeam = myTeamObj.GetComponent<Text> ();
		myTeam.text = teams [myID];
		myTeam.color = teamColor [myID];
		timer = turnTime;
		setCameraTeam ();
		gameStart ();
	}
	// Use this for initialization
	void Start () {
		face = new[]{ new Vector3 (270,0,0), new Vector3 (0,0,90), new Vector3 (0,0,180) , new Vector3 (0,0,0) , new Vector3 (0,0,270) , new Vector3 (90,0,0)  };
		turn = turnObj.GetComponent<Text> ();
		time = timeObj.GetComponent<Text> ();
		countUI = CountUI.GetComponent<Text> ();
		setPause ();

	}
	
	// Update is called once per frame
	void Update () {
		if (gameState != state.Pause && gameState != state.Win && gameState!=state.Start && gameState != state.Move) {
			updateTimer ();
		}
		//Debug.Log (gameState.ToString());
		switch (gameState) {
		case state.Pause:
			break;
		case state.Start:
			diceTimer -= Time.deltaTime;
			countUI.text = ""+(int)diceTimer;
			if(diceTimer<0){
				GameUI.SetActive (true);
				diceContainer.SetActive (true);
				CountUI.SetActive (false);
				nextTurn ();
				time.text = "" + (int)timer;
				if (myID == 0) {
					myTurn ();
				} else {
					wait ();
				}
				}
			break;
		case state.MyTurn:
			setWin ();
			//its myturn roll dice
			if (Input.GetMouseButtonDown (0)) {
				boardScript.Roll ();
				/*if (currentID == myID)
					boardScript.testRoll (5);*/
				//Debug.Log (boardScript.getRollCast ());
				diceTimer = 4;
				currentFaceRotation = face [boardScript.getRollCast ()];
				diceRoll ();

			}
			//currentID = (currentID + 1) % 4;
			//gameState = state.selectPawn;
			break;
		case state.wait:
			boardScript.Roll ();
			//Debug.Log (boardScript.getRollCast ());
			diceTimer = 3;
			currentFaceRotation = face [boardScript.getRollCast ()];
			diceRoll ();
			//AI Or opponents turn logic here.
			break;
		case state.diceroll:
			if (diceTimer > 1) {
				diceTimer -= Time.deltaTime;
				diceObj.transform.Rotate (Vector3.right * 200 * Time.deltaTime);
				diceObj.transform.Rotate (Vector3.up * 500 * Time.deltaTime);
			} else if (Vector3.Distance (diceObj.transform.eulerAngles, currentFaceRotation) > 0.01f && diceTimer > 0) {
				diceTimer -= Time.deltaTime;
				diceObj.transform.eulerAngles = Vector3.Lerp (diceObj.transform.rotation.eulerAngles,currentFaceRotation,0.1f);
			} else {
				diceObj.transform.eulerAngles = currentFaceRotation;

				if (boardScript.getRollCast () == 5 || player[currentID].isActive())// player [currentID].pawnActive [0] || player [currentID].pawnActive [1] || player [currentID].pawnActive [2] || player [currentID].pawnActive [3])
					selectPawn ();
				else if(!(player[0].isActive() || player[1].isActive() || player[2].isActive() || player[3].isActive())){ //replace with a bool
					if (startRolls < 2) {
						startRolls++;
						currentID--;
						changeturn ();
					} else {
						startRolls = 0;
						changeturn ();
					}
				}
				else {
					changeturn ();
				}
			}
			break;
		case state.selectPawn:
			//if it was my turn then after diceroll then select one of pawn to move if possible.
			if (currentID == myID) {
				boardScript.UpdateSelection (currentID);

				if (Input.GetMouseButtonDown (0)) {
					if(boardScript.updateSelectedPawn (player[currentID]))
						move ();
				}
			} else {
				if (boardScript.AISelect (player [currentID]))
					move ();
			}
			break;
		case state.Move: 
			if (boardScript.canMove ()) {
				boardScript.moveUpdate (currentID);
			} else {
				boardScript.killPawns (player,currentID);
				changeturn ();
			}
			break;
		case state.nextturn:
			diceTimer -= Time.deltaTime;
			if (diceTimer < 0) {
				timer = turnTime;
				nextTurn ();
			}
			break;
		case state.Win:
			
			break;
		default:
			break;
		}
	}

	public void setPause(){
		gameState = state.Pause;
	}
	public void setWin(){
		winUI.SetActive (true);
		GameUI.SetActive (false);
		gameState = state.Win;
	}
	public void myTurn(){
		gameState = state.MyTurn;
	}
	public void move(){
		gameState = state.Move;
	}
	public void selectPawn(){
		gameState = state.selectPawn;
	}
	public void wait(){
		gameState = state.wait;
	}
	public void diceRoll(){
		gameState = state.diceroll;
	}
	public void gameStart(){
		diceTimer = 4;
		CountUI.SetActive (true);
		gameState = state.Start;
	}
	public void changeturn(){
		diceTimer = 1;
		boardScript.resetSelectPawn ();
		gameState = state.nextturn;
	}

	public void updateTeam(){
	turn.text = teams [currentID];
	turn.color = teamColor [currentID];
	}

	public void nextTurn(){
		if (currentID > -1 && player [currentID].isWin ()) {
			Debug.Log ("winner: "+currentID);
			CountUI.SetActive (true);
			countUI.fontSize =50;
			countUI.text = "Team: " + teams [currentID] + " Wins";
			setWin ();
		} else {
			currentID = (currentID + 1) % 4;
			updateTeam ();
			if (currentID == myID) {
				//diceContainer.SetActive (true);
				myTurn ();
			} else {
				//diceContainer.SetActive (false);
				wait ();
			}
			boardScript.resetMoves ();
		}
	}

	public void updateTimer(){
		timer -= Time.deltaTime;
		time.text = "" + (int)timer;
		if (timer < 0) {
			timer = turnTime;
			nextTurn ();
		}
	}

	//on start set camera to selected team
	public void setCameraTeam(){
		switch (myID) {
		case 0:
			Camera.main.transform.position = new Vector3 (0,28,22);
			Camera.main.transform.eulerAngles = new Vector3 (45,180,0);
			break;
		case 1:
			Camera.main.transform.position = new Vector3 (22,28,0);
			Camera.main.transform.eulerAngles = new Vector3 (45,270,0);
			break;
		case 2:
			Camera.main.transform.position = new Vector3 (0,28,-22);
			Camera.main.transform.eulerAngles = new Vector3 (45,0,0);
			break;
		case 3:
			Camera.main.transform.position = new Vector3 (-22,28,0);
			Camera.main.transform.eulerAngles = new Vector3 (45,90,0);
			break;

		}
	}

	public void returnbutton(){
		if (gameState == state.Win) {
			MenuUI.SetActive (true);
			foreach (Player p in player)
				p.reset ();
			setPause ();
		}
	}

}
