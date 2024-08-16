using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    private Rigidbody2D rb;
    public bool isHit;
    public bool isShoot;
    private bool isJumping;

    private float footstepInterval = 0.5f; // 발자국 소리 간격 (초)
    private float footstepTimer = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        // 이벤트 등록
        playerController.OnMoveEvent += MoveAnimation;
        playerController.OnJumpEvent += JumpAnimation;
        playerController.OnFallEvent += FallAnimation;
        playerController.OnDashEvent += DashAnimation;
        playerController.OnAttackEvent += AttackAnimation;
        playerController.OnWallSlideEvent += WallSlideAnimation;
        playerController.OnHitEvent += HitAnimation;
        playerController.OnDeathEvent += DeathAnimation;
        playerController.OnShootEvent += ShootAnimation;        
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        playerController.OnMoveEvent -= MoveAnimation;
        playerController.OnJumpEvent -= JumpAnimation;
        playerController.OnFallEvent -= FallAnimation;
        playerController.OnDashEvent -= DashAnimation;
        playerController.OnAttackEvent -= AttackAnimation;
        playerController.OnWallSlideEvent -= WallSlideAnimation;
        playerController.OnHitEvent -= HitAnimation;
        playerController.OnDeathEvent -= DeathAnimation;
        playerController.OnShootEvent -= ShootAnimation;
    }

    private void MoveAnimation(Vector2 direction)
    {
        bool isWalking = direction.magnitude > 0;
        animator.SetBool("isWalking", isWalking);
        
        if (isWalking)
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval)
            {
                PlayRandomFootstepSound();
                footstepTimer = 0f;
            }
        }
    }

    private void PlayRandomFootstepSound()
    {
        int index = Random.Range(0, 4);

        switch (index)
        {
            case 0:
                SoundManager.Instance.PlaySFX("FootstepsGrass1");
                break;
            case 1:
                SoundManager.Instance.PlaySFX("FootstepsGrass2");
                break;
            case 2:
                SoundManager.Instance.PlaySFX("FootstepsGrass3");
                break;
            case 3:
                SoundManager.Instance.PlaySFX("FootstepsGrass4");
                break;
        }
    }

    private void JumpAnimation()
    {
        animator.SetTrigger("Jump");
        animator.SetBool("isJumping", true);
        SoundManager.Instance.PlaySFX("Jump");
    }

    private void FallAnimation()
    {        
        //Debug.Log(rb.velocity.y);
        animator.SetBool("isFalling", true);
    }

    private void DashAnimation()
    {
        animator.SetTrigger("Dash");
        SoundManager.Instance.PlaySFX("Dash", 3f);
    }

    private void AttackAnimation(int attackStage)
    {
        animator.SetInteger("AttackCnt", attackStage);
        animator.SetTrigger("Attack");
        if (attackStage == 1)
        {
            Attack1SFX();
        }
        else if(attackStage == 2)
        {
            Attack2SFX();
        }
        else
        {
            Attack3SFX();
        }

    }

    private void WallSlideAnimation(bool isWallSliding)
    {
        animator.SetBool("IsWallSliding", isWallSliding);
        SoundManager.Instance.PlaySFX("Climb");
    }

    private void HitAnimation(PlayerController controller)
    {
        isHit = true;
        animator.SetBool("isHit", isHit);
        SoundManager.Instance.PlaySFX("Hit");
    }

    private void ShootAnimation()
    {
        animator.SetTrigger("Shoot");
        animator.SetBool("isShoot", true);        
    }

    private void DeathAnimation(PlayerController controller)
    {
        animator.SetBool("isDead", true);
    }

    public void PlayIdle()
    {
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Idle");
        
    }

    // 애니메이션 이벤트에서 호출될 메서드
    private void Attack1SFX()
    {
        SoundManager.Instance.PlaySFX("Attack1", 4f);
    }

    private void Attack2SFX()
    {
        SoundManager.Instance.PlaySFX("Attack2", 4f);
    }

    private void Attack3SFX()
    {
        SoundManager.Instance.PlaySFX("Attack3", 3f);
    }
}