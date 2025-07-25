using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using UnityEngine;

public class AIClient : MonoBehaviour
{
    public event Action<string> OnRealTimeResponding;
    public event Action<string> OnResponded;
    
    [SerializeField] private OpenAIConfiguration openAIConfiguration;
    [SerializeField] private bool enableOpenAIDebug = true;
    [SerializeField] private int maxNumOfWordsInReply = 40;
    [SerializeField] private string priorKnowledge =
        "Flowtropolis is a VR SaaS platform for enterprises, used for onboarding, upskilling, sales, marketing, " +
        "and R&D, with a user-friendly web portal. The startup behind Flowtropolis is based in Stockholm, Sweden";

    private OpenAIClient aiClient;
    private readonly Conversation conversation = new Conversation();
    private readonly List<Tool> assistantTools = new List<Tool>();
    private StringBuilder aiResponse = new StringBuilder();
    
    public void GetAIChatResponseWithContext(string currentUserInputText)
    {
        aiResponse.Clear();

        conversation.AppendMessage(new Message(Role.User, currentUserInputText));
        var request = new ChatRequest(conversation.Messages, tools: assistantTools, model: Model.GPT4o);
        AsyncGetAIResponse(request);    
    }

    public void GetAIExplanationOnImage(Texture2D imageToAnalyze)
    {
        aiResponse.Clear();

        conversation.AppendMessage(new Message(Role.User, new List<Content>
        {
            "Can you explain what's in this image using max 20 words?",
            imageToAnalyze
        }));
        
        var request = new ChatRequest(conversation.Messages, tools: assistantTools, model: Model.GPT4o);
        AsyncGetAIResponse(request);
        
    }
    
    private void Awake()
    {
        aiClient = new OpenAIClient(openAIConfiguration) { EnableDebug = enableOpenAIDebug };
        conversation.AppendMessage(new Message(Role.System, "You are a helpful assistant."));
        conversation.AppendMessage(new Message(Role.System, "Use max " + maxNumOfWordsInReply.ToString() +
                                   " words in your reply please"));
        conversation.AppendMessage(new Message(Role.System, priorKnowledge));
    }

    private async void AsyncGetAIResponse(ChatRequest request)
    {
        try
        {
            // using delta response to get real-time Open AI responses
            // use CHAT endpoint as it's faster, simpler, and more flexible compared to ASSISTANT endpoint; 
            // but CHAT endpoint has No built-in memory or persistence; you'll need to manage context.
            var response = await aiClient.ChatEndpoint.StreamCompletionAsync(request, resultHandler: deltaResponse =>
            {
                if (deltaResponse?.FirstChoice?.Delta == null) return;
                aiResponse.Append(deltaResponse.FirstChoice.Delta.ToString());
                // display AI real-time response in UI
                OnRealTimeResponding?.Invoke(aiResponse.ToString());
            }, cancellationToken: destroyCancellationToken);
            
            OnResponded?.Invoke(aiResponse.ToString());
            conversation.AppendMessage(response.FirstChoice.Message);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case TaskCanceledException :
                case OperationCanceledException :
                    break;
                default:
                    Debug.LogError(e);
                    break;
            }
        }
    }

    private async void AsyncGetPCAExplanation()
    {
        
    }
}
