using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private string username;
	private int playerID;
	private bool human;
	public GameObject[] pawns = new GameObject[4];
	public bool[] pawnActive;
	public void setPlayer(string name,int id,bool ishuman){
		username = name;
		playerID = id;
		human = ishuman;
	}

	public string getName(){
		return username;
	}

	public int getID(){
		return playerID;
	}

	public bool isHuman(){
		return human;
	}

	public bool isActive(){
		return pawnActive [0] || pawnActive [1] || pawnActive [2] || pawnActive [3];
	}

	public bool isWin(){
		return pawns [0].GetComponent<pawn>().distance > 55 && pawns [1].GetComponent<pawn>().distance > 55 && pawns [2].GetComponent<pawn>().distance > 55 && pawns [3].GetComponent<pawn>().distance > 55;
	}
	// Use this for initialization
	void Start () {
		pawnActive = new bool[4];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void reset(){
		for (int i = 0; i < pawns.Length; i++) {
			pawn pw = pawns[i].GetComponent<pawn> ();
			pawns [i].transform.position = pw.initial;
			pw.reset ();
			name = null;
			playerID = 0;
			human = false;
			for (int j = 0; j < pawnActive.Length; j++){
				pawnActive[j] = false;
			}
		}
	}

}
