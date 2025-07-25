using System;
using System.Text;
using Meta.WitAi.Dictation;
using UnityEngine;

public class DictationController : MonoBehaviour
{
    public event Action<string> OnTranscriptionUpdated;
    public event Action<string> OnFullTranscriptionCompleted;
    public bool IsTranscriptionHistoryKept;
    
    [SerializeField] private DictationService dictation;
    [SerializeField] private string separator = ". ";
    
    private StringBuilder transcriptionHistory; // transcription result history. Using StringBuilder for better performance
    private string activeText; // real-time partial transcription

    public void ToggleActivation()
    {
        if (dictation.MicActive)
        {
            dictation.Deactivate();
        }
        else
        {
            dictation.Activate();
        }
    }
    
    public void Clear()
    {
        transcriptionHistory.Clear();
        OnTranscriptionUpdated?.Invoke(string.Empty);
    }

    private void Awake()
    {
        transcriptionHistory = new StringBuilder();
    }

    private void OnEnable()
    {
        // _activeText = string.Empty;
        dictation.DictationEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
        dictation.DictationEvents.OnFullTranscription.AddListener(OnFullTranscription);
        dictation.DictationEvents.OnAborting.AddListener(OnCancelled);
    }

    private void OnDisable()
    {
        activeText = string.Empty;
        dictation.DictationEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
        dictation.DictationEvents.OnFullTranscription.RemoveListener(OnFullTranscription);
        dictation.DictationEvents.OnAborting.RemoveListener(OnCancelled);
    }

    /// <summary>
    /// Transcribe intermediate results while speaking
    /// </summary>
    /// <param name="text">real-time transcription</param>
    private void OnPartialTranscription(string text)
    {
        activeText = text;
        UpdateTranscription(false);
    }
    
    /// <summary>
    /// Transcribe final result after user stops speaking
    /// </summary>
    /// <param name="text">final transcription after a clear speech pause</param>
    private void OnFullTranscription(string text)
    {
        activeText = string.Empty;

        if (IsTranscriptionHistoryKept)
        {
            if (text.Length > 0)
            {
                transcriptionHistory.Append(separator);
            }

            transcriptionHistory.Append(text);
        }
        else
        {
            transcriptionHistory.Clear();
            transcriptionHistory.Append(text);
        }
        
        UpdateTranscription(true);
    }
    
    private void OnCancelled()
    {
        activeText = string.Empty;
        UpdateTranscription(false);
    }

    /// <summary>
    /// Update real-time transcription results in UI Text Mesh Pro - Input Field
    /// </summary>
    private void UpdateTranscription(bool isFullTranscription)
    {
        var transcription = new StringBuilder();
        transcription.Append(transcriptionHistory);
        if (!string.IsNullOrEmpty(activeText))
        {
            if (transcription.Length > 0)
            {
                transcription.Append(separator);
            }
            transcription.Append(activeText);
        }
        OnTranscriptionUpdated?.Invoke(transcription.ToString());
        
        if (isFullTranscription)
        {
            OnFullTranscriptionCompleted?.Invoke(transcription.ToString());
        }
    }
}
