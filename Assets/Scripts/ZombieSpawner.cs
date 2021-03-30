using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public float ZombieRespawnTimer = 5;

    public Zombie PrefabZombie;

    private Zombie _zombie;

    /// <summary>
    /// Spawns a zombie.
    /// </summary>
    /// <param name="wait"> Time to wait in seconds before spawning a zombie. </param>
    public void Spawn(float wait = .2f)
    {
        if (_zombie == null)
            StartCoroutine(SpawnZombieWithDelay(wait));
    }

    private IEnumerator SpawnZombieWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_zombie == null)
        {
            _zombie = Instantiate(PrefabZombie);
            _zombie.transform.position = transform.position;
            _zombie.transform.rotation = transform.rotation;

            _zombie.Died += ZombieDiedEventHandler;
        }
        else
            _zombie.gameObject.SetActive(true);
    }

    private void ZombieDiedEventHandler(object sender, LiveBeing e)
    {
        _zombie.gameObject.SetActive(false);
        _zombie.transform.position = transform.position;
        _zombie.transform.rotation = transform.rotation;
        _zombie.Health = PrefabZombie.Health;

        StartCoroutine(SpawnZombieWithDelay(ZombieRespawnTimer));
    }
}