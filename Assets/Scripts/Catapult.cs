using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    #region Fields

    public Projectile CurrentBarrel;
    public float ProjectileCooldown = 3;

    [Space(20)]
    public Projectile BarrelPrefab;

    public Transform BarrelSpawnLocation;

    public LayerMask ColliderLayerMask;

    public List<Projectile> UsedBarrels = new List<Projectile>();

    private bool _isBarrelReadyForThrowing = true;

    #endregion Fields

    #region Unity

    private void Start()
    {
        InstatiateBarrel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && _isBarrelReadyForThrowing)
        {
            Ray ray = ManagerCamera.Instance.CameraComponent.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ColliderLayerMask))
            {
                CurrentBarrel.Fire(hit.point);
                _isBarrelReadyForThrowing = false;
                StartCoroutine(StartProjectileCooldown());
            }
        }
    }

    #endregion Unity

    #region Methods

    private IEnumerator StartProjectileCooldown()
    {
        yield return new WaitForSeconds(ProjectileCooldown);

        CurrentBarrel = GetUsedBarrel();

        if (CurrentBarrel == null)
            InstatiateBarrel();
        else
        {
            ResetBarrel();
            CurrentBarrel.gameObject.SetActive(true);
        }

        _isBarrelReadyForThrowing = true;
    }

    private void InstatiateBarrel()
    {
        CurrentBarrel = Instantiate(BarrelPrefab);
        CurrentBarrel.gameObject.name = $"Barrel{UsedBarrels.Count}";
        ResetBarrel();
        CurrentBarrel.Exploded += BarrelExplodedEventHandler;

        UsedBarrels.Add(CurrentBarrel);
    }

    private Projectile GetUsedBarrel()
    {
        return UsedBarrels.FirstOrDefault(barrel => !barrel.gameObject.activeSelf);
    }

    private void ResetBarrel()
    {
        CurrentBarrel.transform.position = BarrelSpawnLocation.position;
        CurrentBarrel.transform.rotation = BarrelSpawnLocation.rotation;
    }

    #endregion Methods

    #region Events

    private void BarrelExplodedEventHandler(object sender, Projectile barrel)
    {
        barrel.gameObject.SetActive(false);
        ResetBarrel();
    }

    #endregion Events
}