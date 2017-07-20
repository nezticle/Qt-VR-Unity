using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public GameObject pointer;

    private GameObject pointerRight;
    private GameObject pointerLeft;
    private GameObject targetRight;

    public Transform leftHand;
    public Transform rightHand;

	// Use this for initialization
	void Start () {
        pointerRight = Instantiate(pointer, rightHand);
        pointerRight.SetActive(false);
        pointerRight.name = "rightPointer";
        pointerLeft = Instantiate(pointer, leftHand);
        pointerLeft.SetActive(false);
        pointerLeft.name = "leftPointer";
        targetRight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        targetRight.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        targetRight.GetComponent<Collider>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        // Recenter View
        if (OVRInput.GetUp(OVRInput.Button.Two))
        {
            OVRManager.display.RecenterPose();
        }

        // Primary
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            pointerRight.SetActive(true);

            pointerRight.transform.localPosition = new Vector3(0f, -0.02f, 0f);

            RaycastHit hit;
            if (Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hit, 100))
            {
                //Debug.Log("hit!");
                Vector3 scale = new Vector3(1f, 1f, 1f);
                scale.z = hit.distance;
                pointerRight.transform.localScale = scale;

                targetRight.transform.position = hit.point;
                targetRight.SetActive(true);

                GameObject hitObject = hit.collider.gameObject;
                QtQuickGUI guiObject = hitObject.GetComponent<QtQuickGUI>();
                if (guiObject)
                {
                    //Debug.Log(hit.textureCoord.x + ", " + hit.textureCoord.y);

                    if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                    {
                        // push
                        Debug.Log("push");
                        QtQuickGUI.RegisterTouchStartEvent(-(hit.textureCoord.x - 1.0f), hit.textureCoord.y, 0);
                    } else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
                    {
                        Debug.Log("release");
                        // release
                        QtQuickGUI.RegisterTouchEndEvent(-(hit.textureCoord.x - 1.0f), hit.textureCoord.y, 0);
                    } else
                    {
                        // move
                        QtQuickGUI.RegisterTouchMoveEvent(-(hit.textureCoord.x - 1.0f), hit.textureCoord.y, 0);
                    }
                }
            }

        } else
        {
            pointerRight.SetActive(false);
            targetRight.SetActive(false);
        }

        // Secondary
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {

        } else
        {
            
        }

	}
}
