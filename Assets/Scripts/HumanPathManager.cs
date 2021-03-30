using System.Collections.Generic;
using UnityEngine;

public class HumanPathManager : MonoBehaviour
{
    public static HumanPathManager Instance;

    public List<GameObject> Checkpoints = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }
}