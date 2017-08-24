using UnityEngine;
using System.Collections.Generic;
using System;
using random = UnityEngine.Random;

public class BoardManager : MonoBehaviour 
{
	[Serializable]
	public class Dice
	{
		[Range(1,7)]
		private int cast;

		public Dice(int c)
		{
			cast = c;
		}

		public void Roll()
		{
			cast = random.Range (0,6);
		}

		public int getCast(){
			return cast;
		}

		public void setCast(int i){
			cast = i;
		}
	}

	public node[] start;
	public node[] turn;
	public GameObject selectedPawn;


	private RaycastHit hit = new RaycastHit();
	private string[] pawnLayer = { "yellow", "green", "red", "blue" };
	private const float dx = 1.125f, dz = 1.125f;
	private const float boardHeight = 7.0f;
	//private GameObject selected = null;
	private node[] nodes;
	//private node startRed,startBlue,startGreen,startYellow;
	//private node turnRed,turnBlue,turnGreen,turnYellow;
	private Dice dice;

	private Transform BoardHolder;
	private float moveTimer = 0.5f;
	private int moves = 0;
	private bool hitSomething = false;

	public void init(){
		dice = new Dice (0);
		nodes = new node[6 * 3 * 4 + 5 * 4 + 1];
		start = new node[4];
		turn = new node[4];
		//start outer path
		nodes[0] = new node(new Vector3(dx,boardHeight,6*dz));//yellow start node
		nodes[1] = new node(new Vector3(dx,boardHeight,5*dz));
		nodes [0].next = nodes [1];
		nodes[2] = new node(new Vector3(dx,boardHeight,4*dz));
		nodes [1].next = nodes [2];
		nodes[3] = new node(new Vector3(dx,boardHeight,3*dz));
		nodes [2].next = nodes [3];
		nodes[4] = new node(new Vector3(dx,boardHeight,2*dz));
		nodes [3].next = nodes [4];
		nodes[5] = new node(new Vector3(2*dx,boardHeight,dz));
		nodes [4].next = nodes [5];
		nodes[6] = new node(new Vector3(3*dx,boardHeight,dz));
		nodes [5].next = nodes [6];
		nodes[7] = new node(new Vector3(4*dx,boardHeight,dz));
		nodes [6].next = nodes [7];
		nodes[8] = new node(new Vector3(5*dx,boardHeight,dz));
		nodes [7].next = nodes [8];
		nodes[9] = new node(new Vector3(6*dx,boardHeight,dz));
		nodes [8].next = nodes [9];
		nodes[10] = new node(new Vector3(7*dx,boardHeight,dz));
		nodes [9].next = nodes [10];
		nodes[11] = new node(new Vector3(7*dx,boardHeight,0));//trun green node
		nodes [10].next = nodes [11];
		nodes[12] = new node(new Vector3(7*dx,boardHeight,-dz));
		nodes [11].next = nodes [12];
		nodes[13] = new node(new Vector3(6*dx,boardHeight,-dz));//green start node
		nodes [12].next = nodes [13];
		nodes[14] = new node(new Vector3(5*dx,boardHeight,-dz));
		nodes [13].next = nodes [14];
		nodes[15] = new node(new Vector3(4*dx,boardHeight,-dz));
		nodes [14].next = nodes [15];
		nodes[16] = new node(new Vector3(3*dx,boardHeight,-dz));
		nodes [15].next = nodes [16];
		nodes[17] = new node(new Vector3(2*dx,boardHeight,-dz));
		nodes [16].next = nodes [17];
		nodes[18] = new node(new Vector3(dx,boardHeight,-2*dz));
		nodes [17].next = nodes [18];
		nodes[19] = new node(new Vector3(dx,boardHeight,-3*dz));
		nodes [18].next = nodes [19];
		nodes[20] = new node(new Vector3(dx,boardHeight,-4*dz));
		nodes [19].next = nodes [20];
		nodes[21] = new node(new Vector3(dx,boardHeight,-5*dz));
		nodes [20].next = nodes [21];
		nodes[22] = new node(new Vector3(dx,boardHeight,-6*dz));
		nodes [21].next = nodes [22];
		nodes[23] = new node(new Vector3(dx,boardHeight,-7*dz));
		nodes [22].next = nodes [23];
		nodes[24] = new node(new Vector3(0,boardHeight,-7*dz));//turn red nde
		nodes [23].next = nodes [24];
		nodes[25] = new node(new Vector3(-dx,boardHeight,-7*dz));
		nodes [24].next = nodes [25];
		nodes[26] = new node(new Vector3(-dx,boardHeight,-6*dz));//start red node
		nodes [25].next = nodes [26];
		nodes[27] = new node(new Vector3(-dx,boardHeight,-5*dz));
		nodes [26].next = nodes [27];
		nodes[28] = new node(new Vector3(-dx,boardHeight,-4*dz));
		nodes [27].next = nodes [28];
		nodes[29] = new node(new Vector3(-dx,boardHeight,-3*dz));
		nodes [28].next = nodes [29];
		nodes[30] = new node(new Vector3(-dx,boardHeight,-2*dz));
		nodes [29].next = nodes [30];
		nodes[31] = new node(new Vector3(-2*dx,boardHeight,-dz));
		nodes [30].next = nodes [31];
		nodes[32] = new node(new Vector3(-3*dx,boardHeight,-dz));
		nodes [31].next = nodes [32];
		nodes[33] = new node(new Vector3(-4*dx,boardHeight,-dz));
		nodes [32].next = nodes [33];
		nodes[34] = new node(new Vector3(-5*dx,boardHeight,-dz));
		nodes [33].next = nodes [34];
		nodes[35] = new node(new Vector3(-6*dx,boardHeight,-dz));
		nodes [34].next = nodes [35];
		nodes[36] = new node(new Vector3(-7*dx,boardHeight,-dz));
		nodes [35].next = nodes [36];
		nodes[37] = new node(new Vector3(-7*dx,boardHeight,0));//trun blue node
		nodes [36].next = nodes [37];
		nodes[38] = new node(new Vector3(-7*dx,boardHeight,dz));
		nodes [37].next = nodes [38];
		nodes[39] = new node(new Vector3(-6*dx,boardHeight,dz));
		nodes [38].next = nodes [39];
		nodes[40] = new node(new Vector3(-5*dx,boardHeight,dz));
		nodes [39].next = nodes [40];
		nodes[41] = new node(new Vector3(-4*dx,boardHeight,dz));
		nodes [40].next = nodes [41];
		nodes[42] = new node(new Vector3(-3*dx,boardHeight,dz));
		nodes [41].next = nodes [42];
		nodes[43] = new node(new Vector3(-2*dx,boardHeight,dz));
		nodes [42].next = nodes [43];
		nodes[44] = new node(new Vector3(-dx,boardHeight,2*dz));
		nodes [43].next = nodes [44];
		nodes[45] = new node(new Vector3(-dx,boardHeight,3*dz));
		nodes [44].next = nodes [45];
		nodes[46] = new node(new Vector3(-dx,boardHeight,4*dz));
		nodes [45].next = nodes [46];
		nodes[47] = new node(new Vector3(-dx,boardHeight,5*dz));
		nodes [46].next = nodes [47];
		nodes[48] = new node(new Vector3(-dx,boardHeight,6*dz));
		nodes [47].next = nodes [48];
		nodes[49] = new node(new Vector3(-dx,boardHeight,7*dz));
		nodes [48].next = nodes [49];
		nodes[50] = new node(new Vector3(0,boardHeight,7*dz));//turn yellow node
		nodes [49].next = nodes [50];
		nodes[51] = new node(new Vector3(dx,boardHeight,7*dz));
		nodes [50].next = nodes [51];
		nodes [51].next = nodes [0];
		//yellow final row
		nodes[52] = new node(new Vector3(0,boardHeight,6*dz));
		nodes [50].altnext = nodes [52];
		nodes[53] = new node(new Vector3(0,boardHeight,5*dz));
		nodes [52].next = nodes [53];
		nodes[54] = new node(new Vector3(0,boardHeight,4*dz));
		nodes [53].next = nodes [54];
		nodes[55] = new node(new Vector3(0,boardHeight,3*dz));
		nodes [54].next = nodes [55];
		nodes[56] = new node(new Vector3(0,boardHeight,2*dz));
		nodes [55].next = nodes [56];

		//green final row
		nodes[57] = new node(new Vector3(6*dx,boardHeight,0));
		nodes [11].altnext = nodes [57];
		nodes[58] = new node(new Vector3(5*dx,boardHeight,0));
		nodes [57].next = nodes [58];
		nodes[59] = new node(new Vector3(4*dx,boardHeight,0));
		nodes [58].next = nodes [59];
		nodes[60] = new node(new Vector3(3*dx,boardHeight,0));
		nodes [59].next = nodes [60];
		nodes[61] = new node(new Vector3(2*dx,boardHeight,0));
		nodes [60].next = nodes [61];

		//red final row
		nodes[62] = new node(new Vector3(0,boardHeight,-6*dz));
		nodes [24].altnext = nodes [62];
		nodes[63] = new node(new Vector3(0,boardHeight,-5*dz));
		nodes [62].next = nodes [63];
		nodes[64] = new node(new Vector3(0,boardHeight,-4*dz));
		nodes [63].next = nodes [64];
		nodes[65] = new node(new Vector3(0,boardHeight,-3*dz));
		nodes [64].next = nodes [65];
		nodes[66] = new node(new Vector3(0,boardHeight,-2*dz));
		nodes [65].next = nodes [66];

		//blue final row
		nodes[67] = new node(new Vector3(-6*dx,boardHeight,0));
		nodes [37].altnext = nodes [67];
		nodes[68] = new node(new Vector3(-5*dx,boardHeight,0));
		nodes [67].next = nodes [68];
		nodes[69] = new node(new Vector3(-4*dx,boardHeight,0));
		nodes [68].next = nodes [69];
		nodes[70] = new node(new Vector3(-3*dx,boardHeight,0));
		nodes [69].next = nodes [70];
		nodes[71] = new node(new Vector3(-2*dx,boardHeight,0));
		nodes [70].next = nodes [71];

		//end node center
		nodes[72] = new node(new Vector3(0,boardHeight,0));
		nodes [72].next = null;
		nodes [72].altnext = null;
		nodes [56].next = nodes [72];
		nodes [61].next = nodes [72];
		nodes [66].next = nodes [72];
		nodes [71].next = nodes [72];

		start[0] = nodes [0];
		start[1] = nodes [13];
		start[2] = nodes [26];
		start[3] = nodes [39];

		turn [0] = nodes [50];
		turn [1] = nodes [11];
		turn [2] = nodes [24];
		turn [3] = nodes [37];

	}

	public void Roll(){
			dice.Roll ();
	}

	public void testRoll(int i){
		dice.setCast (i);
	}

	public int getRollCast(){
		return dice.getCast();
	}

	public void UpdateSelection(int myID){
		if (!Camera.main)
			return;

		hitSomething = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 50.0f, LayerMask.GetMask (pawnLayer[myID]));
	}

	public bool updateSelectedPawn(Player p){
		if (!hitSomething)
			return false;
		GameObject pw = hit.transform.gameObject;
		if (pw == null)
			return false;
		else if (GameObject.ReferenceEquals (p.pawns [0],pw) && (p.pawnActive[0] || getRollCast() == 5) && p.pawns[0].GetComponent<pawn> ().distance + getRollCast()<56){
			selectedPawn = pw;
			if(!p.pawnActive[0])
				p.pawnActive [0] = true;
			return true;
		}
		else if (GameObject.ReferenceEquals (p.pawns [1], pw) && (p.pawnActive[1] || getRollCast() == 5) && p.pawns[1].GetComponent<pawn> ().distance + getRollCast()<56) {
			selectedPawn = pw;
			if(!p.pawnActive[1])
				p.pawnActive [1] = true;
			return true;
		}
		else if (GameObject.ReferenceEquals (p.pawns [2], pw) && (p.pawnActive[2] || getRollCast() == 5) && p.pawns[2].GetComponent<pawn> ().distance + getRollCast()<56) {
			selectedPawn = pw;
			if(!p.pawnActive[2])
				p.pawnActive [2] = true;
			return true;
		}
		else if (GameObject.ReferenceEquals (p.pawns [3], pw) && (p.pawnActive[3] || getRollCast() == 5) && p.pawns[3].GetComponent<pawn> ().distance + getRollCast()<56) {
			selectedPawn = pw;
			if(!p.pawnActive[3])
				p.pawnActive [3] = true;
			return true;
		}
		return false;
	}

	public bool AISelect(Player p){
		if (getRollCast() == 5) {
			if (!p.pawnActive [0]) {
				selectedPawn = p.pawns [0];
				p.pawnActive [0] = true;
				return true;
			} else if (!p.pawnActive [1]) {
				selectedPawn = p.pawns [1];
				p.pawnActive [1] = true;
				return true;
			} else if (!p.pawnActive [2]) {
				selectedPawn = p.pawns [2];
				p.pawnActive [2] = true;
				return true;
			} else if (!p.pawnActive [3]) {
				selectedPawn = p.pawns [3];
				p.pawnActive [3] = true;
				return true;
			}
		}
		int selected = random.Range (0, 4);
		if (p.pawnActive [selected]) {
			selectedPawn = p.pawns [selected];
			if ((selectedPawn.GetComponent<pawn> ().distance + getRollCast())>55){
				selectedPawn = null;
				return false;
			}
			return true;
		}
		return false;
	}

	public void killPawns(Player[] player,int currentID)
	{
		for (int j = 0; j < player.Length; j++) 
		{
			if (j != currentID)
			{
				GameObject[] p = player [j].pawns;
				for (int i = 0; i < p.Length; i++) 
				{
					pawn pwn = p [i].GetComponent<pawn> ();
					if(pwn.distance < 56)
					if (pwn.currentNode == selectedPawn.GetComponent<pawn> ().currentNode) 
					{
						pwn.reset ();
						p[i].transform.position = pwn.initial;
						player [j].pawnActive [i] = false;
					}
				}
			}
		}
	}

	public void resetSelectPawn()
	{
		selectedPawn = null;
	}
	public void resetMoves(){
		moves = 0;
	}
	public void moveUpdate(int currentid){
		if (selectedPawn == null || selectedPawn.GetComponent<pawn> () == null) {
			moves = 10;
		}
		else if (selectedPawn.GetComponent<pawn> ().currentNode == null) {
			selectedPawn.GetComponent<pawn> ().currentNode = start[currentid];
			selectedPawn.transform.position = selectedPawn.GetComponent<pawn> ().currentNode.position;
			moves = 10;
		}
		else if (moveTimer < 0) {
			if (selectedPawn.GetComponent<pawn> ().currentNode == turn [selectedPawn.GetComponent<pawn> ().type]) {
				//Debug.Log (selectedPawn.GetComponent<pawn> ().currentNode.position);
				selectedPawn.transform.position = selectedPawn.GetComponent<pawn> ().currentNode.altnext.position;
				selectedPawn.GetComponent<pawn> ().currentNode = selectedPawn.GetComponent<pawn> ().currentNode.altnext;
			} else {
				//Debug.Log (selectedPawn.GetComponent<pawn> ().distance);
				selectedPawn.transform.position = selectedPawn.GetComponent<pawn> ().currentNode.next.position;
				selectedPawn.GetComponent<pawn> ().currentNode = selectedPawn.GetComponent<pawn> ().currentNode.next;
			}
			moveTimer = 0.5f;
			selectedPawn.GetComponent<pawn> ().distance++;
			moves++;
		}
		else {
			if (selectedPawn.GetComponent<pawn> ().currentNode == turn [selectedPawn.GetComponent<pawn> ().type]) {
				selectedPawn.transform.position = Vector3.Lerp (selectedPawn.GetComponent<pawn> ().currentNode.position, selectedPawn.GetComponent<pawn> ().currentNode.altnext.position, Time.deltaTime);
			} 
			else {
				selectedPawn.transform.position = Vector3.Lerp (selectedPawn.GetComponent<pawn> ().currentNode.position, selectedPawn.GetComponent<pawn> ().currentNode.next.position, Time.deltaTime);
			}
		}
		moveTimer -= Time.deltaTime;
	}

	public bool canMove(){
		return moves <= dice.getCast();
	}

}
