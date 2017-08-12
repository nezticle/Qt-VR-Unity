using UnityEngine;
using System.Runtime.InteropServices;

public class QtUserInterface : MonoBehaviour {

    [DllImport("QtUserInterfacePlugin")]
    private static extern void UpdateQtEventLoop();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateQtEventLoop();
	}
}
