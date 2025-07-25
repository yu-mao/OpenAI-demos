using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject aiTalk;
    [SerializeField] private GameObject aiVision;
    [SerializeField] private GameObject aiAction;
    public void EnableAiChat(bool isEnabled)
    {
        if (isEnabled)
        {
            aiTalk.SetActive(true);
            aiVision.SetActive(false);
            aiAction.SetActive(false);
        }
    }

    public void EnableAiVision(bool isEnabled)
    {
        if (isEnabled)
        {
            aiTalk.SetActive(false);
            aiVision.SetActive(true);
            aiAction.SetActive(false);
        }
    }

    public void EnableAiAction(bool isEnabled)
    {
        if (isEnabled)
        {
            aiTalk .SetActive(false);
            aiVision.SetActive(false);
            aiAction.SetActive(true);
        }
    }
}
