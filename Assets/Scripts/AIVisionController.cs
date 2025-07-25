using System.Collections;
using Oculus.Interaction.Input;
using PassthroughCameraSamples;
using TMPro;
using UnityEngine;
using Utilities.Extensions;

public class AIVisionController : MonoBehaviour
{
    [Header("User Input")]
    [SerializeField] private WebCamTextureManager pcaManager;

    [SerializeField] private Hand handWithPinchToTakePhoto;
    
    [Header("AI Output")]
    [SerializeField] private AIClient openAIClient;
    [SerializeField] private TMP_InputField aiResponseTextFieldInUI;
    [SerializeField] private TextMeshProUGUI aiResponseTextFieldInView;
    [SerializeField] private TTSController ttsController;

    private bool aiCanSeeScene = false;
    
    public void AskAI()
    {
        ttsController.Stop();
        Texture2D usersViewImage = GetUsersViewImage();
        openAIClient.GetAIExplanationOnImage(usersViewImage);
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
        aiResponseTextFieldInView.text = aiResponse;
    }
    
    private void SpeakResponse(string aiResponse)
    {
        ttsController.Speak(aiResponse);
        aiCanSeeScene = true;
    }
    
    
    private void OnEnable()
    {
        openAIClient.OnRealTimeResponding += DisplayRealTimeAIResponse;
        openAIClient.OnResponded += SpeakResponse;
        aiCanSeeScene = true;
        aiResponseTextFieldInView.SetActive(true);
    }

    private void OnDisable()
    {
        openAIClient.OnRealTimeResponding -= DisplayRealTimeAIResponse;
        openAIClient.OnResponded -= SpeakResponse;
        aiCanSeeScene = false;
        aiResponseTextFieldInView.SetActive(false);

    }

    private void Update()
    {
        if (aiCanSeeScene && handWithPinchToTakePhoto.GetFingerIsPinching(HandFinger.Index))
        {
            AskAI();
            aiCanSeeScene = false;
            aiResponseTextFieldInUI.text = "... ...";
            aiResponseTextFieldInView.text = "... ...";
        }
    }
}
