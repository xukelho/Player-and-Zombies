using System.Linq;
using UnityEngine;

public class Human : Humanoid
{
    #region Fields

    public float RunSpeed = .1f;

    public AudioSource Source;

    private bool _isMoving = false;

    #endregion Fields

    #region Unity Methods

    private void Start()
    {
        PlayIdle();
    }

    private void Update()
    {
        if (!_isMoving)
            return;

        WalkToCheckpointInPath();
    }

    #endregion Unity Methods

    #region Methods

    public override void PlayIdle()
    {
        Animator.SetFloat("MoveSpeed", 0);
        _isMoving = false;
        Source.Stop();
    }

    public override void PlayMove()
    {
        StartCoroutine(PlayAnimationIdleToRun());
        _isMoving = true;
        Source.Play();
    }

    protected override void Die()
    {
        base.Die();

        PlayIdle();
        Destroy(gameObject);
        Destroy(this);
    }

    public override void CollidedWithObject(Collision collision)
    {
        Zombie zombie = collision.gameObject.GetComponent<Zombie>();

        if (zombie != null)
            TakeDamage(zombie.DamageToDeal);
    }

    public void PlayWin()
    {
        PlayIdle();
    }

    private void WalkToCheckpointInPath()
    {
        LookAtNextCheckpoint();

        Vector3 newposition = transform.position + transform.forward * RunSpeed;
        newposition.y = transform.position.y;
        transform.position = newposition;

        VerifyIfCheckpointHasBeenReached();
    }

    private void LookAtNextCheckpoint()
    {
        Vector3 checkpointPosition = HumanPathManager.Instance.Checkpoints.First().transform.position;
        Vector3 positionToLookAt = new Vector3(checkpointPosition.x, transform.position.y, checkpointPosition.z);
        transform.LookAt(positionToLookAt, Vector3.up);
    }

    private void VerifyIfCheckpointHasBeenReached()
    {
        float margin = .05f;
        Vector3 checkpointPosition = HumanPathManager.Instance.Checkpoints.First().transform.position;
        bool isXPositionWithinMargins = Mathf.Abs(transform.position.x - checkpointPosition.x) < margin;
        bool isZPositionWithinMargins = Mathf.Abs(transform.position.z - checkpointPosition.z) < margin;

        if (isXPositionWithinMargins && isZPositionWithinMargins)
        {
            HumanPathManager.Instance.Checkpoints.Remove(HumanPathManager.Instance.Checkpoints.First());

            if (HumanPathManager.Instance.Checkpoints.Count == 0)
            {
                PlayWin();
                Main.Instance.LevelFinished();
            }
        }
    }

    #endregion Methods
}