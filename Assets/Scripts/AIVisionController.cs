using System;
using PassthroughCameraSamples;
using TMPro;
using UnityEngine;

public class AIVisionController : MonoBehaviour
{
    [Header("User Input")]
    [SerializeField] private WebCamTextureManager pcaManager;
    // [SerializeField] private Texture2D usersViewImage;
    
    [Header("AI Output")]
    [SerializeField] private AIClient openAIClient;
    [SerializeField] private TMP_InputField aiResponseTextFieldInUI;
    [SerializeField] private TMP_InputField aiResponseTextFieldInView;
    [SerializeField] private TTSController ttsController;
    
    public void AskAI()
    {
        ttsController.Stop();
        Texture2D usersViewImage = GetUsersViewImage();
        openAIClient.GetAIExplanationOnImage(usersViewImage);
    }

    private void OnEnable()
    {
        openAIClient.OnRealTimeResponding += DisplayRealTimeAIResponse;
        openAIClient.OnResponded += SpeakResponse;
    }

    private void OnDisable()
    {
        openAIClient.OnRealTimeResponding -= DisplayRealTimeAIResponse;
        openAIClient.OnResponded -= SpeakResponse;
    }

    private Texture2D GetUsersViewImage()
    {
        Texture2D usersViewImage = new Texture2D(pcaManager.WebCamTexture.width, pcaManager.WebCamTexture.height);
        
        Color32[] pixels = new Color32[usersViewImage.width * usersViewImage.height];
        pcaManager.WebCamTexture.GetPixels32(pixels);
        usersViewImage.SetPixels32(pixels);
        usersViewImage.Apply();

        return usersViewImage;
    }
    
    private void DisplayRealTimeAIResponse(string aiResponse)
    {
        aiResponseTextFieldInUI.text = aiResponse;
    }
    
    private void SpeakResponse(string aiResponse)
    {
        ttsController.Speak(aiResponse);
    }
}
