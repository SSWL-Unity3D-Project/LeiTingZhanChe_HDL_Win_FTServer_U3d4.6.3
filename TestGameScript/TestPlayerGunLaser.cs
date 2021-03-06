﻿using UnityEngine;
using System.Collections;

public class TestPlayerGunLaser : MonoBehaviour
{
	public float scrollSpeed = 0.5f;
	public float pulseSpeed = 1.5f;
	public float noiseSize = 1.0f;
	
	public float maxWidth = 0.5f;
	public float minWidth = 0.2f;
	
	public GameObject pointer = null;
	
	private LineRenderer lRenderer;
//	private float aniTime = 0.0f;
	private float aniDir = 1.0f;
	
	//private var raycast : PerFrameRaycast;
	
	void Start()
	{
//		tr = transform;
		lRenderer = gameObject.GetComponent<LineRenderer>();
//		aniTime = 0.0f;
		
		// Change some animation values here and there
		StartCoroutine(ChoseNewAnimationTargetCoroutine());
	}
	
	IEnumerator ChoseNewAnimationTargetCoroutine()
	{
		while (true) {
			aniDir = aniDir * 0.9f + Random.Range (0.5f, 1.5f) * 0.1f;
			//yield return;
			minWidth = minWidth * 0.8f + Random.Range (0.1f, 1.0f) * 0.2f;
			yield return new WaitForSeconds (1.0f + Random.value * 2.0f - 1.0f);	
		}	
	}
	
//	private RaycastHit hitInfo;
//	private Transform tr;
	public LayerMask LaserHit;
	void Update()
	{
		//renderer.material.mainTextureOffset.x += Time.deltaTime * aniDir * scrollSpeed;
		Vector2 veOffset = renderer.material.mainTextureOffset;
		veOffset.x += Time.deltaTime * aniDir * scrollSpeed;
		renderer.material.mainTextureOffset = veOffset;
		renderer.material.SetTextureOffset("_NoiseTex", new Vector2(-Time.time * aniDir * scrollSpeed, 0.0f));
		
		float aniFactor = Mathf.PingPong (Time.time * pulseSpeed, 1.0f);
		aniFactor = Mathf.Max (minWidth, aniFactor) * maxWidth;
		lRenderer.SetWidth (aniFactor, aniFactor);
		
		// Cast a ray to find out the end point of the laser
//		RaycastHit hitInfo = GetHitInfo();
//		RaycastHit hitInfo;
//		if (Physics.Raycast(tr.position, tr.forward, out hitInfo, 500f, LaserHit.value)) {
////		if (hitInfo.transform) {
//			lRenderer.SetPosition (1, (hitInfo.distance * Vector3.forward));
//			veOffset = renderer.material.mainTextureScale;
//			veOffset.x = 0.1f * (hitInfo.distance);
//			//renderer.material.mainTextureScale.x = 0.1f * (hitInfo.distance);
//			renderer.material.mainTextureScale = veOffset;
//			renderer.material.SetTextureScale ("_NoiseTex", new Vector2(0.1f * hitInfo.distance * noiseSize, noiseSize));		
//			
//			// Use point and normal to align a nice & rough hit plane
//			if (pointer) {
//				pointer.renderer.enabled = true;
//				pointer.transform.position = hitInfo.point + (transform.position - hitInfo.point) * 0.01f;
//				pointer.transform.rotation = Quaternion.LookRotation (hitInfo.normal, transform.up);
////				pointer.transform.eulerAngles.x = 90.0f;
//				veOffset = pointer.transform.eulerAngles;
//				veOffset.x = 90.0f;
//				pointer.transform.eulerAngles = veOffset;
//			}
//		}
//		else {
//			if (pointer) {
//				pointer.renderer.enabled = false;
//			}
//			float maxDist = 200.0f;
//			lRenderer.SetPosition (1, (maxDist * Vector3.forward));
////			renderer.material.mainTextureScale.x = 0.1f * (maxDist);
//			veOffset = renderer.material.mainTextureScale;
//			veOffset.x = 0.1f * (maxDist);
//			renderer.material.mainTextureScale = veOffset;
//			renderer.material.SetTextureScale ("_NoiseTex", new Vector2(0.1f * (maxDist) * noiseSize, noiseSize));		
//		}
	}

//	RaycastHit GetHitInfo()
//	{
////		hitInfo = new RaycastHit();
//		Physics.Raycast(tr.position, tr.forward, out hitInfo, 500f, ~LaserHit.value);
//		return hitInfo;
//	}
}