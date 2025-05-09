﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
	static private Slingshot S;
	[Header("Set in Inspector")]
	public GameObject prefabProjectile;
	[Header("Set Dynamically")]
	
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public float velocityMult = 8f;
	[Header("Set Dynamically")]
	public bool aimingMode;
	private Rigidbody projectileRigidbody;
	
	static public Vector3 LAUNCH_POS { // b
	get {
		if (S == null ) return Vector3.zero;
			return S.launchPos;
		}
	}
	
	void Awake(){
		S = this;
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
	}
	
	void OnMouseEnter(){
		//print("Slingshot:OnMouseEnter()");
		launchPoint.SetActive(true);
	}
	
	void OnMouseExit(){
		//print("Slingshot:OnMouseExit()");
		launchPoint.SetActive(false);
	}
	
	void OnMouseDown(){
		aimingMode = true;
		projectile = Instantiate(prefabProjectile) as GameObject;
		// move it to the launch Pos
		projectile.transform.position = launchPos;
		projectile.GetComponent<Rigidbody>().isKinematic = true;
		projectileRigidbody = projectile.GetComponent<Rigidbody>();
		projectileRigidbody.isKinematic = true;
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!aimingMode) return;
		
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
		Vector3 mouseDelta = mousePos3D - launchPos;
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if (mouseDelta.magnitude > maxMagnitude){
		   mouseDelta.Normalize();
		   mouseDelta *= maxMagnitude;
		}
		
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;
		
		if (Input.GetMouseButtonUp(0)){
			aimingMode = false;
			projectileRigidbody.isKinematic = false;
			projectileRigidbody.velocity = -mouseDelta * velocityMult;
			FollowCam.POI = projectile;
			projectile = null;
		}
    }
}
