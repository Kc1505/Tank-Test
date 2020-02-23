using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

	public GameObject WheelFront;
	public GameObject WheelBack;
	public GameObject Gun;
	public GameObject Turret;
	public GameObject Chassis;
	public GameObject Wheels;
	public GameObject Track;
	public GameObject rot1;
	public GameObject rot2;
	public GameObject rotChild;
	public GameObject swapBoom;

	AudioSource gunMoveSound;

	public AudioClip gunSoundCLP;

	private float f;
	private float b;
	private float testGA;
	private float rotN;
	private Quaternion gunAngle;


	void Start()
	{

		rotN = 2;
		testGA = 0;
		gunAngle.eulerAngles = new Vector3(0, 0, 0);

		ConnectTracks();

		gunMoveSound = GetComponent<AudioSource>();
	}

	void FixedUpdate(){

		f = WheelFront.GetComponent<Rigidbody2D>().angularVelocity;
		b = WheelBack.GetComponent<Rigidbody2D>().angularVelocity;

		if (Input.GetKey("a"))
		{
			WheelFront.GetComponent<Rigidbody2D>().AddTorque(Mathf.Clamp(1 / (Mathf.Exp(0.005f * f - 5.4f)), 0, 100));
			WheelBack.GetComponent<Rigidbody2D>().AddTorque(Mathf.Clamp(1 / (Mathf.Exp(0.005f * b - 5.4f)), 0, 100));
		}

		if (Input.GetKey("d"))
		{
			WheelFront.GetComponent<Rigidbody2D>().AddTorque(-Mathf.Clamp(1 / (Mathf.Exp(-0.005f * f - 5.4f)), 0, 100));
			WheelBack.GetComponent<Rigidbody2D>().AddTorque(-Mathf.Clamp(1 / (Mathf.Exp(-0.005f * b - 5.4f)), 0, 100));
		}

		if (Input.GetKey("space"))
		{
			//WheelFront.GetComponent<Rigidbody2D>().AddTorque(-Mathf.Clamp(WheelFront.GetComponent<Rigidbody2D>().angularVelocity * 10000, -120, 120));
			//WheelBack.GetComponent<Rigidbody2D>().AddTorque(-Mathf.Clamp(WheelBack.GetComponent<Rigidbody2D>().angularVelocity * 10000, -120, 120));
			WheelFront.GetComponent<Rigidbody2D>().freezeRotation = true;
			WheelBack.GetComponent<Rigidbody2D>().freezeRotation = true;
		}
		else
		{
			WheelFront.GetComponent<Rigidbody2D>().freezeRotation = false;
			WheelBack.GetComponent<Rigidbody2D>().freezeRotation = false;
		}

		if (Input.GetKey("up") && ((Gun.GetComponent<WheelJoint2D>().jointAngle <= 16.5 && transform.lossyScale.x > 0) || (Gun.GetComponent<WheelJoint2D>().jointAngle >= -16.5 && transform.lossyScale.x < 0)))
		{
			gunMoveSound.pitch = 0.2f * Time.timeScale;
			gunMoveSound.PlayOneShot(gunSoundCLP, 0.03f);

			gunAngle.eulerAngles += new Vector3(0, 0, transform.lossyScale.x * 0.5f);
		}

		if (Input.GetKey("down") && ((Gun.GetComponent<WheelJoint2D>().jointAngle >= -3.5 && transform.lossyScale.x > 0) || (Gun.GetComponent<WheelJoint2D>().jointAngle <= 3.5 && transform.lossyScale.x < 0)))
		{
			gunMoveSound.pitch = 0.2f * Time.timeScale;
			gunMoveSound.PlayOneShot(gunSoundCLP, 0.05f);

			gunAngle.eulerAngles -= new Vector3(0, 0, transform.lossyScale.x * 0.5f);
		}

		if (Input.GetKey("right") && transform.localScale.x > 0)
		{
			//Destroy(Instantiate(swapBoom, Chassis.transform.position, Chassis.transform.rotation), 3f);

			transform.position = new Vector3(transform.position.x + (Chassis.transform.position.x - transform.position.x) * 2, transform.position.y, transform.position.z);
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			Rotate();

			ConnectTracks();

			gunAngle.eulerAngles = new Vector3(0,0,0);
		}

		if (Input.GetKey("left") && transform.localScale.x < 0)
		{
			transform.position = new Vector3(transform.position.x + (Chassis.transform.position.x - transform.position.x) * 2, transform.position.y, transform.position.z);
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

			Rotate();

			ConnectTracks();

			gunAngle.eulerAngles = new Vector3(0, 0, 0);
		}

		Gun.GetComponent<Transform>().Rotate(new Vector3(0,0,1),Chassis.GetComponent<Transform>().rotation.eulerAngles.z - Gun.GetComponent<Transform>().rotation.eulerAngles.z - gunAngle.eulerAngles.z);

		Chassis.GetComponent<AudioSource>().pitch = Time.timeScale*0.65f *Mathf.Clamp(Mathf.Abs(WheelFront.GetComponent<Rigidbody2D>().angularVelocity / 2000) + 0.5f, 0.5f, 0.8f);
		                     
	}

	void Rotate()
	{
		rotChild.transform.parent = rot2.transform;
		rot1.transform.position = Chassis.transform.position;
		rot1.transform.rotation = Quaternion.identity;
		rotChild.transform.parent = rot1.transform;
		Debug.Log("1: " + Chassis.transform.rotation.eulerAngles.z);
		rot1.transform.Rotate(new Vector3(0, 0, 1), -((Chassis.transform.rotation.eulerAngles.z)) * 2);
		Gun.GetComponent<Rigidbody2D>().angularVelocity = 0;
		Turret.GetComponent<Rigidbody2D>().angularVelocity = 0;
		Chassis.GetComponent<Rigidbody2D>().angularVelocity = 0;
	}

	void ConnectTracks()
	{
		int i;
		int total;
		for (total = 0; GameObject.Find("TrackSeg (" + total + ")"); total++)
			for (i = 0; i < total; i++)
			{
				GameObject.Find("TrackSeg (" + i + ")").GetComponent<HingeJoint2D>().connectedBody = GameObject.Find("TrackSeg (" + (i + 1) + ")").GetComponent<Rigidbody2D>();
			}
		GameObject.Find("TrackSeg (" + (total - 1) + ")").GetComponent<HingeJoint2D>().connectedBody = GameObject.Find("TrackSeg (" + 0 + ")").GetComponent<Rigidbody2D>();
	}

	private static float WrapAngle(float angle)
	{
		angle %= 360;
		if (angle > 180)
			return angle - 360;

		return angle;
	}

	private static float UnwrapAngle(float angle)
	{
		if (angle >= 0)
			return angle;

		angle = -angle % 360;

		return 360 - angle;
	}
}


/* Code graveyard
	-Settingle angle limits to move the turret (Causes the tank to move when rotating)
		JointAngleLimits2D test = Gun.GetComponent<HingeJoint2D>().limits;
		test.max -= 0.5f;
		test.min -= 0.5f;
		Gun.GetComponent<HingeJoint2D>().limits = test;
	
	-using force to make sure the turret doesnt clip (Given up for now)
		private int gunCol;
		&& Gun.GetComponent<HingeJoint2D>().reactionTorque < 100 && Gun.GetComponent<HingeJoint2D>().reactionTorque > -100
		gunAngle.eulerAngles = new Vector3(0, 0, Chassis.GetComponent<Transform>().rotation.eulerAngles.z - Gun.GetComponent<Transform>().rotation.eulerAngles.z)
	
	-Force Method of Moving Turret
		Gun.GetComponent<Rigidbody2D>().AddTorque(-15 * CubeRoot(Mathf.DeltaAngle(Chassis.GetComponent<Transform>().rotation.eulerAngles.z - testGA, Gun.GetComponent<Transform>().rotation.eulerAngles.z)));
		testGA -= 0.5f;
		-Cube Root Equation
			float CubeRoot(float d){ if(d < 0.0f){return -Mathf.Pow(-d, 1f / 3f);} else{return Mathf.Pow(d, 1f / 3f);}}
*/
