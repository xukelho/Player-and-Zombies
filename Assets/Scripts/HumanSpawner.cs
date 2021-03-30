using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    public Human PrefabHuman;

    public Human Human;

    /// <summary>
    /// Spawns a human.
    /// </summary>
    public void Spawn()
    {
        if (Human != null)
            return;

        Human = Instantiate(PrefabHuman);
        Human.transform.position = transform.position;
        Human.transform.rotation = transform.rotation;
    }
}