using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VideoPreviewInEditor : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;

    void Start()
    {
#if UNITY_EDITOR
        EditorApplication.update += UpdateEditor;
#endif
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
    }

#if UNITY_EDITOR
    void OnDisable()
    {
        EditorApplication.update -= UpdateEditor;
    }

    void UpdateEditor()
    {
        if (!Application.isPlaying && videoPlayer != null && renderTexture != null)
        {
            videoPlayer.targetTexture = renderTexture;
            videoPlayer.Play();
        }
    }
#endif
}
