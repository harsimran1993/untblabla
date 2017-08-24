using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	public Transform target;
	public float xSpeed = 10.0f;
	public float ySpeed = 10.0f;
	public float lowY = 3;

	private float x = 0.0f,lastx=0.0f; 
	private float y = 0.0f,lasty=0.0f;
	//private float z = 0.0f,lastz=0.0f; 
	private Touch touch;

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			x += Input.GetAxis ("Mouse X") * xSpeed * Time.deltaTime;
			y -= Input.GetAxis ("Mouse Y") * ySpeed * Time.deltaTime;
			//z = 0.0f; 
			//Reorient();
		} else if (Input.GetMouseButton (0)) {
			lastx = x;
			lasty = y;
			//lastz = z;
			x += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
			//z = 0.0f;
			Reorient();
		}
		if (Input.GetMouseButtonUp (0)) {

			/*if (transform.position.y < lowY) {
				transform.Translate (0, lowY, -(transform.position.z*0.03f));
				transform.LookAt (target);
			}*/

		}

	}

	void Reorient()
	{
		x -= lastx;
		y -= lasty;
		if ((transform.rotation.eulerAngles.x > lowY || y > 0) && (transform.rotation.eulerAngles.x < 65 || y < 0))
			transform.RotateAround (target.position, transform.right, y);
		//if(x > 2 || x < -2)
			transform.RotateAround (target.position,Vector3.up,x);
		transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,0f);
		transform.LookAt (target);
		x = 0;
		y = 0;
	}
}