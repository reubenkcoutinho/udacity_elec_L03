﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerInput : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    private LineRenderer laser;
    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    public LayerMask laserMask;
    public float yNudgeAmount = 1f; // specifi to teleport aimer object height

	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();       
        laser = GetComponentInChildren<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        Debug.LogError(trackedObj.index);
        Debug.LogError("DEVICE");
        Debug.LogError(device.GetType().ToString());
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            laser.gameObject.SetActive(true);
            teleportAimerObject.gameObject.SetActive(true);

            laser.SetPosition(0, gameObject.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 15, laserMask))
            {
                teleportLocation = hit.point;
                laser.SetPosition(1, teleportLocation);
                //aimer position
                teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudgeAmount, teleportLocation.z);
            }
            else
            {
                teleportLocation  = new Vector3(transform.forward.x * 15 + transform.position.x, transform.forward.y * 15 + transform.position.y, transform.forward.z * 15 + transform.position.z);
                RaycastHit groundRay;
                if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17, laserMask))
                {
                    teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, groundRay.point.y, transform.forward.z * 15 + transform.position.z);
                    laser.SetPosition(1, teleportLocation);
                    //aimer position
                    teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudgeAmount, teleportLocation.z);
                }
                laser.SetPosition(1, transform.forward * 15 + transform.position);
                // aimer position
                teleportAimerObject.transform.position = teleportLocation + new Vector3(0, yNudgeAmount, 0);
            }
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            laser.gameObject.SetActive(false);
            teleportAimerObject.gameObject.SetActive(false);
            player.transform.position = teleportLocation;
        }
    }
}
