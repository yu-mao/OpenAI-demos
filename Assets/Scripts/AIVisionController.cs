using System;
using TMPro;
using UnityEngine;

public class AIVisionController : MonoBehaviour
{
    [Header("User Input")]
    [SerializeField] private Texture2D usersViewImage;
    
    [Header("AI Output")]
    [SerializeField] private AIClient openAIClient;
    [SerializeField] private TMP_InputField aiResponseTextFieldInUI;
    [SerializeField] private TMP_InputField aiResponseTextFieldInView;
    [SerializeField] private TTSController ttsController;
    
    public void AskAI()
    {
        ttsController.Stop();
        openAIClient.GetAIExplanationOnImage(usersViewImage);
    }

    private void Start()
    {
        AskAI();
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

    private void DisplayRealTimeAIResponse(string aiResponse)
    {
        aiResponseTextFieldInUI.text = aiResponse;
    }
    
    private void SpeakResponse(string aiResponse)
    {
        ttsController.Speak(aiResponse);
    }
}
