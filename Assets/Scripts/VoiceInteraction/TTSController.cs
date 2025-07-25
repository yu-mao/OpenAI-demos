using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class TTSController : MonoBehaviour
{
    [SerializeField] private TTSSpeaker ttsSpeaker;

    public void Speak(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            ttsSpeaker.Speak(text);
        }
    }

    public void Stop()
    {
        ttsSpeaker.Stop();
    }
}
