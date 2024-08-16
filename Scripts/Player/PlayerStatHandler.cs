using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatHandler : MonoBehaviour, IDamagable
{
    private PlayerController playerController;
    public PlayerStats playerstats;

    [Header("Health UI")]
    public List<Image> heartImages; // 하트 이미지 리스트
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            //Debug.LogError("PlayerController is not assigned in PlayerStatHandler.");
            return;
        }

        playerstats = playerController.playerStats;
        if (playerstats == null)
        {
            //Debug.LogError("PlayerStats is not assigned in PlayerStatHandler.");
            return;
        }

        playerstats.playerCurHP = playerstats.playerMaxHP;
        InitializeHealthUI();
        //UpdateHealthUI(playerstats.playerCurHP); // 초기 체력 UI 업데이트
    }

    public void InitializeHealthUI()
    {
        // 기존 리스트를 초기화하고 하트 이미지를 다시 찾습니다.
        FindHeartImages();

        // 하트 이미지를 초기화합니다.
        ResetHealth(); // 체력과 UI를 최대 상태로 초기화
    }
    public void FindHeartImages()
    {
        // 기존 리스트를 완전히 초기화하여 중복이나 잘못된 참조 제거
        heartImages.Clear();

        // HeartImage 태그가 붙은 모든 오브젝트를 찾습니다.
        GameObject[] heartImageObjects = GameObject.FindGameObjectsWithTag("HeartImage");

        // 찾은 오브젝트들을 리스트에 추가
        foreach (var heartObj in heartImageObjects)
        {
            Image heartImageComponent = heartObj.GetComponent<Image>();
            if (heartImageComponent != null)
            {
                heartImages.Add(heartImageComponent);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        playerstats.playerCurHP -= damage;
        UpdateHealthUI(playerstats.playerCurHP); // 체력 변화 이벤트 발생

        if (playerstats.playerCurHP <= 0)
        {
            playerController.OnDeath();
        }
    }

    public void UpdateHealthUI(float currentHP)
    {
        if (heartImages == null || heartImages.Count == 0)
        {
            //Debug.LogWarning("Heart Images are missing!");
            return;
        }

        float healthPerHeart = playerstats.playerMaxHP / (heartImages.Count * 2); // 각 하트를 2개의 반칸으로 나눔
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (currentHP >= (i + 1) * healthPerHeart * 2)
            {
                heartImages[i].sprite = fullHeart;
            }
            else if (currentHP >= (i * healthPerHeart * 2 + healthPerHeart))
            {
                heartImages[i].sprite = halfHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }

    public void ResetHealth()
    {
        playerstats.playerCurHP = playerstats.playerMaxHP;
        UpdateHealthUI(playerstats.playerCurHP); // 초기 체력 UI 업데이트
    }
    void OnEnable()
    {
        InitializeHealthUI(); // 씬이 로드되거나 활성화될 때마다 UI 초기화
    }
}