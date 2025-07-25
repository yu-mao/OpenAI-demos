using System;
using PassthroughCameraSamples;
using UnityEngine;

public class PCAControlller : MonoBehaviour
{
    [SerializeField] private WebCamTextureManager pcaManager;
    [SerializeField] private Renderer pcaDisplayRenderer;

    private void Update()
    {
        if (pcaManager.WebCamTexture != null)
        {
            pcaDisplayRenderer.material.mainTexture = pcaManager.WebCamTexture;
        }
    }
}
