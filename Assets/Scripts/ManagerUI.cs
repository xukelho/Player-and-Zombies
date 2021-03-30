using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    public void SpawnHuman() => Main.Instance.SpawnHumanoid(Main.HumanoidType.Human);

    public void SpawnZombie() => Main.Instance.SpawnHumanoid(Main.HumanoidType.Zombie);

    public void PlayIdle() => Main.Instance.HumanPlayIdle();

    public void PlayRun() => Main.Instance.HumanPlayRun();
}