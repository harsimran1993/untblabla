using UnityEngine;
using System.Collections;

public class pawn : MonoBehaviour {

	public node currentNode{ set; get; }
	public Vector3 initial{ set; get; }
	public int type;
	public int distance{ set; get; }

	void Start(){
		initial = transform.position;
		distance = 0;
		//Debug.Log (type);
	}
	public void reset(){
		currentNode = null;
		distance = 0;
	}

}
