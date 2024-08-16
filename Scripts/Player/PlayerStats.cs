[System.Serializable]
public class PlayerStats
{
    public float playerMaxHP = 6.0f;
    public float playerCurHP;
    public float playerSpeed = 5f;
    public float playerKnockbackDist = 0.5f;
    public AttackSO[] playerAttackSO;

    public float playerMeleeAtkDamage;
    public float playerBowAtkDamage;
    public float playerAtkSpeed;
    public float numberOfClicks = 0f;
    public float maxComboDelay = 0.9f;
    public float lastClickedTime = 0f;
}