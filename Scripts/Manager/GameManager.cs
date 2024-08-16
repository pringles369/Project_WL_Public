using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    public string playerName = "Player"; // 씬에서 플레이어 오브젝트의 이름
    public GameObject playerPrefab; // 플레이어 프리팹 참조
    public GameObject gameOverUIPrefab; // 게임 오버 UI 프리팹
    private GameObject player; // 현재 활성화된 플레이어 인스턴스
    private GameObject gameOverUI; // 현재 활성화된 게임 오버 UI 인스턴스
    private bool isSceneInitialized = false; // 씬 초기화가 되었는지 확인하는 플래그

    private PlayerStatHandler playerStatHandler;

    protected override void Awake()
    {
        base.Awake();
        //Debug.Log("GameManager Awake");

        // 게임 오버 UI 프리팹을 인스턴스화하고 비활성화
        if (gameOverUIPrefab != null && gameOverUI == null)
        {
            gameOverUI = Instantiate(gameOverUIPrefab);
            gameOverUI.SetActive(false);
            DontDestroyOnLoad(gameOverUI);
        }
    }

    private void Start()
    {
        //Debug.Log("GameManager Start");
        FindAndAssignPlayer();
    }

    public void FindAndAssignPlayer()
    {
        // 이미 플레이어 오브젝트가 존재하는지 확인
        player = GameObject.FindWithTag(playerName);

        if (player == null && playerPrefab != null)
        {
            //Debug.LogWarning("Player object not found, creating new one from prefab.");
            player = Instantiate(playerPrefab);
            player.name = playerName; // 플레이어 이름 설정
            DontDestroyOnLoad(player);
        }
        else if (player != null)
        {
            //Debug.Log("Player already assigned: " + player.name);
        }
        else
        {
            //Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없으며, 프리팹도 설정되지 않았습니다.");
            return;
        }

        playerStatHandler = player.GetComponent<PlayerStatHandler>();
        if (playerStatHandler != null)
        {
            playerStatHandler.FindHeartImages();  // 하트 이미지를 다시 참조
            playerStatHandler.InitializeHealthUI();  // 씬 초기화 시 체력 UI를 초기화
        }
    }

    // 씬 초기화 메서드
    public void InitializeCurrentScene()
    {
        //Debug.Log("InitializeCurrentScene 호출됨");
        if (!isSceneInitialized)
        {
            isSceneInitialized = true; // 플래그를 먼저 설정
            StartCoroutine(InitializeCurrentSceneRoutine());
        }
    }

    private IEnumerator InitializeCurrentSceneRoutine()
    {
        yield return new WaitForEndOfFrame(); // 씬 로드 후 한 프레임 대기
        //Debug.Log("InitializeCurrentSceneRoutine 시작");

        MapSettings mapSettings = FindObjectOfType<MapSettings>();
        if (mapSettings != null)
        {
            //Debug.Log("MapSettings를 성공적으로 찾았습니다: " + mapSettings.gameObject.name);
            InitializeMapSettings(mapSettings);
        }
        else
        {
            //Debug.LogWarning("현재 씬에서 MapSettings를 찾을 수 없습니다.");
        }
    }

    private void InitializeMapSettings(MapSettings mapSettings)
    {
        // 플레이어를 StartPoint로 이동
        if (mapSettings.StartPoint != null && player != null)
        {
            //Debug.Log("플레이어를 StartPoint로 이동합니다: " + mapSettings.StartPoint.position);
            player.transform.position = mapSettings.StartPoint.position;

            // MapDoorManager 초기화
            MapDoorManager doorManager = mapSettings.DoorPrefab.GetComponent<MapDoorManager>();
            if (doorManager != null)
            {
                doorManager.Initialize(mapSettings);
            }
            else
            {
                //Debug.LogWarning("MapDoorManager를 찾을 수 없습니다.");
            }
        }
        else
        {
            //Debug.LogWarning("StartPoint가 설정되지 않았거나 Player가 null입니다.");
        }
    }

    public void GameOver()
    {
        if (player != null)
        {
            player.GetComponent<PlayerStatHandler>().enabled = false;
        }
        Time.timeScale = 0;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameRoutine());
    }

    private IEnumerator RestartGameRoutine()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // 기존 플레이어 오브젝트 제거
        if (player != null)
        {
            Destroy(player);
            player = null;
        }

        // 하트 UI 오브젝트도 초기화
        if (playerStatHandler != null)
        {
            playerStatHandler.heartImages.Clear();  // 리스트 초기화
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isSceneInitialized = false; // 게임 재시작 시 초기화 플래그를 초기화
        yield return null;

        FindAndAssignPlayer();

        if (playerStatHandler != null)
        {
            playerStatHandler.InitializeHealthUI();  // 체력 UI를 다시 초기화
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("씬 로드 완료 후 초기화 실행: " + scene.name);

        // 기존의 플레이어 오브젝트가 존재하는지 확인하고 초기화
        if (player == null)
        {
            FindAndAssignPlayer();
        }

        isSceneInitialized = false; // 씬 로드 시 초기화 플래그를 초기화
        InitializeCurrentScene(); // 씬이 로드된 후 다시 초기화 루틴을 실행

        if (playerStatHandler != null)
        {
            playerStatHandler.FindHeartImages();
            playerStatHandler.UpdateHealthUI(playerStatHandler.playerstats.playerCurHP);
        }
    }
}