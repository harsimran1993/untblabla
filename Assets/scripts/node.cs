using UnityEngine;
using System.Collections;

public class node {
	public Vector3 position{ set; get; }
	public node next{ set; get; }
	public node altnext{set;get;}
	public bool filled{ set; get; }

	public node(Vector3 pos){
		position = pos;
		next = null;
		altnext = null;
		filled = false;
	}
}
