using UnityEngine;

public class Zombie : Humanoid
{
    #region Fields

    public float DistanceToDetectHuman = 3;
    public float DistanceToAttackHuman = 1;

    public float WalkSpeed = 1;

    public int DamageToDeal;

    private bool _playerDetected = false;
    private bool _runningToPlayer = false;
    private bool _attackPlayed = false;

    #endregion Fields

    #region Unity

    private void Update()
    {
        if (Main.Instance.Player != null
            && Vector3.Distance(Main.Instance.Player.transform.position, transform.position) < DistanceToDetectHuman
            && !_playerDetected)
        {
            _playerDetected = true;
            GoAfterPlayer();
        }

        if (_runningToPlayer)
        {
            MoveTowardsPlayer();

            if (!_attackPlayed && Vector3.Distance(Main.Instance.Player.transform.position, transform.position) < DistanceToAttackHuman)
            {
                PlayAttack();
            }
        }
    }

    #endregion Unity

    #region Methods

    public override void PlayIdle()
    {
        Animator.SetFloat("MoveSpeed", 0);
    }

    public override void PlayMove()
    {
        StartCoroutine(PlayAnimationIdleToRun());
    }

    protected override void Die()
    {
        base.Die();

        _playerDetected = false;
    }

    private void PlayAttack()
    {
        Animator.SetTrigger("Attack");
        _attackPlayed = true;
    }

    private void GoAfterPlayer()
    {
        Main.Instance.Player.Died += PlayerDiedEventHandler;
        _runningToPlayer = true;
        PlayMove();
    }

    private void PlayerDiedEventHandler(object sender, LiveBeing e)
    {
        _playerDetected = false;
        _runningToPlayer = false;
        _attackPlayed = false;
        PlayIdle();

        e.Died -= PlayerDiedEventHandler;
    }

    private void MoveTowardsPlayer()
    {
        Humanoid player = Main.Instance.Player;
        Vector3 lookAtPlayer = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        transform.LookAt(lookAtPlayer);
        transform.position += transform.forward * (WalkSpeed / 100);
    }

    #endregion Methods
}