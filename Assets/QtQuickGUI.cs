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
    private static extern void SetTextureFromUnity(int objectId, IntPtr texture, int width, int height);
    [DllImport("QtUserInterfacePlugin")]
    private static extern void RemoveUIObject(int objectId);

    [DllImport("QtUserInterfacePlugin")]
    private static extern void RegisterTouchStartEvent(int objectId, float x, float y, int touchpoint);
    [DllImport("QtUserInterfacePlugin")]
    private static extern void RegisterTouchEndEvent(int objectId, float x, float y, int touchpoint);
    [DllImport("QtUserInterfacePlugin")]
    private static extern void RegisterTouchMoveEvent(int objectId, float x, float y, int touchpoint);

    public void RegisterTouchStartEvent(float x, float y, int touchpoint)
    {
        RegisterTouchStartEvent(GetInstanceID(), x, y, touchpoint);
    }

    public void RegisterTouchEndEvent(float x, float y, int touchpoint)
    {
        RegisterTouchEndEvent(GetInstanceID(), x, y, touchpoint);
    }

    public void RegisterTouchMoveEvent(float x, float y, int touchpoint)
    {
        RegisterTouchMoveEvent(GetInstanceID(), x, y, touchpoint);
    }

    // Use this for initialization
    IEnumerator Start () {
        CreateTextureAndPassToPlugin();
        yield return StartCoroutine("CallPluginAtEndOfFrames");
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
        SetTextureFromUnity(GetInstanceID(), tex.GetNativeTexturePtr(), tex.width, tex.height);
    }

    private IEnumerator CallPluginAtEndOfFrames()
    {
        while (true)
        {
            // Wait until all frame rendering is done
            yield return new WaitForEndOfFrame();

            // Issue a plugin event with instance ID identifier

            GL.IssuePluginEvent(GetRenderEventFunc(), GetInstanceID());
        }
    }

    void OnDestroy()
    {
        RemoveUIObject(GetInstanceID());
    }
}
