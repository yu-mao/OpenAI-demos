using TMPro;
using UnityEngine;

public class AITalkController : MonoBehaviour
{
    [Header("User Input")]
    [SerializeField] private TMP_InputField transcriptionTextField;
    [SerializeField] private DictationController dictationController;
    
    [Header("AI Output")]
    [SerializeField] private AIClient openAIClient;
    [SerializeField] private TMP_InputField aiResponseTextField;
    [SerializeField] private TTSController ttsController;
    
    public void ToggleDictation()
    {
        dictationController.Clear();
        dictationController.ToggleActivation();
        // stop Open AI's speaking if any
        ttsController.Stop();
    }
    
    private void OnEnable()
    {
        dictationController.OnTranscriptionUpdated += UpdateRealTimeTranscription;
        dictationController.OnFullTranscriptionCompleted += UpdateFullTranscription;
        
        openAIClient.OnRealTimeResponding += DisplayRealTimeAIResponse;
        openAIClient.OnResponded += SpeakResponse;
    }
    
    private void OnDisable()
    {
        dictationController.OnTranscriptionUpdated -= UpdateRealTimeTranscription;
        dictationController.OnFullTranscriptionCompleted -= UpdateFullTranscription;
        openAIClient.OnRealTimeResponding -= DisplayRealTimeAIResponse;
        openAIClient.OnResponded -= SpeakResponse;
    }
    
    private void UpdateRealTimeTranscription(string realTimeTranscription)
    {
        transcriptionTextField.text = realTimeTranscription;
    }
    
    private void UpdateFullTranscription(string fullTranscription)
    {
        aiResponseTextField.text = string.Empty;
        openAIClient.GetAIChatResponseWithContext(fullTranscription);
    }
    
    private void DisplayRealTimeAIResponse(string aiResponse)
    {
        aiResponseTextField.text = aiResponse;
    }
    
    private void SpeakResponse(string aiResponse)
    {
        ttsController.Speak(aiResponse);
    }
}
