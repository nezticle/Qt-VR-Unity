using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class QtQuickGUI : MonoBehaviour {

    public int textureWidth = 512;
    public int textureHeight = 512;

    [DllImport("QtUserInterfacePlugin")]
    private static extern IntPtr GetRenderEventFunc();

    [DllImport("QtUserInterfacePlugin")]
    private static extern void SetTextureFromUnity(IntPtr texture, int width, int height);

    [DllImport("QtUserInterfacePlugin")]
    private static extern void SetTimeFromUnity(float t);

    [DllImport("QtUserInterfacePlugin")]
    private static extern void UpdateQtEventLoop();

    [DllImport("QtUserInterfacePlugin")]
    public static extern void RegisterTouchStartEvent(float x, float y, int touchpoint);
    [DllImport("QtUserInterfacePlugin")]
    public static extern void RegisterTouchEndEvent(float x, float y, int touchpoint);
    [DllImport("QtUserInterfacePlugin")]
    public static extern void RegisterTouchMoveEvent(float x, float y, int touchpoint);

    // Use this for initialization
    IEnumerator Start () {
        CreateTextureAndPassToPlugin();
        yield return StartCoroutine("CallPluginAtEndOfFrames");
	}
	
	// Update is called once per frame
	void Update () {
        UpdateQtEventLoop();
	}

    private void CreateTextureAndPassToPlugin()
    {
        // Create a texture
        Texture2D tex = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
        tex.filterMode = FilterMode.Trilinear;
        tex.Apply();

        // Set texture onto material
        GetComponent<Renderer>().material.mainTexture = tex;
        GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

        // Pass the texture pointer to the plugin
        SetTextureFromUnity(tex.GetNativeTexturePtr(), tex.width, tex.height);
    }

    private IEnumerator CallPluginAtEndOfFrames()
    {
        while (true)
        {
            // Wait until all frame rendering is done
            yield return new WaitForEndOfFrame();

            // Set time for the plugin
            SetTimeFromUnity(Time.timeSinceLevelLoad);

            // Issue a plugin event with arbitrary integer identifier.
            // The plugin can distinguish between different
            // things it needs to do based on this ID.
            // For our simple plugin, it does not matter which ID we pass here.
            GL.IssuePluginEvent(GetRenderEventFunc(), 1);
        }
    }
}
