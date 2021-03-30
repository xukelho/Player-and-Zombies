using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    #region Fields

    public static Main Instance;

    [Space(20)]
    public Camera MainCamera;

    [Space(20)]
    public Human PrefabHuman;

    public Human Player;
    public Transform PlayerSpawnerLocation;

    [Space(20)]
    public List<ZombieSpawner> ZombieSpawnerList;

    public enum HumanoidType
    {
        Human,
        Zombie
    }

    #endregion Fields

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ManagerCamera.Instance.FlyToHumanFinished += Instance_FlyToHumanFinished;
    }

    #endregion Unity Methods

    #region Methods

    public void SpawnHumanoid(HumanoidType type)
    {
        switch (type)
        {
            case HumanoidType.Human:
                SpawnHuman();
                break;

            case HumanoidType.Zombie:
                SpawnZombies();
                break;

            default:
                break;
        }
    }

    public void HumanPlayIdle()
    {
        Player?.PlayIdle();
    }

    public void HumanPlayRun()
    {
        Player?.PlayMove();
    }

    public void LevelFinished()
    {
        ManagerAudio.Instance.PlayVictory();
        ManagerCamera.Instance.FlyToHuman();
    }

    private void SpawnHuman()
    {
        if (Player != null)
            return;

        Player = Instantiate(PrefabHuman);
        Player.transform.position = PlayerSpawnerLocation.transform.position;
        Player.transform.rotation = PlayerSpawnerLocation.transform.rotation;
        Player.Died += PlayerDiedEventHandler;
    }

    private void SpawnZombies()
    {
        float waitTime = .1f;
        for (int i = 0; i < ZombieSpawnerList.Count; i++)
        {
            if (ZombieSpawnerList[i].gameObject.activeSelf)
                ZombieSpawnerList[i].Spawn(i * waitTime);
        }
    }

    #endregion Methods

    #region Events

    private void Instance_FlyToHumanFinished(object sender, System.EventArgs e)
    {
        ManagerLevel.PlayNextLevel();
    }

    private void PlayerDiedEventHandler(object sender, LiveBeing e)
    {
        Player = null;
    }

    #endregion Events
}