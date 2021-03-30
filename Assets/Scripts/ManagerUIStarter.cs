using UnityEngine;

public class ManagerUIStarter : MonoBehaviour
{
    public GameObject InstructionsPanel;

    public void StartPlaying()
    {
        ManagerLevel.PlayNextLevel();
    }

    public void HowToPlay()
    {
        InstructionsPanel.SetActive(!InstructionsPanel.activeSelf);
    }
}