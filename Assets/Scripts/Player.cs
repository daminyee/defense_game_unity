using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 2024.01.17 숙제
// 1. level마다 enemy가 새롭게 소환되는 전체 로직 구현 (쉬는 시간 포함)
// 2. enemy가 마지막 지점에 도달하면 hp 깎는 것도 구현 (+ enemy가 Destroy되는 것도 구현)

// 2024.01.21 숙제
// 1.base turret script 만들기 (turret을 자유롭게 구상하기) - 고려할 점: 업그레이드도 되어야 한다, turret 공격력, 등등등
// 2. turret 2개 prefab랑 각각을 위한 script만들기
// 다음수업때: drag 설명 다시하고, 어떻게 drag로 turret 설치할 수 있게 만드는지 설명

// 2024.03.03까지 숙제
// 1. turret UI 기본적인 것 다 했다. 근데 여러개가 열리는 것 방지하기 (숙제)
// 2. slow turret UI까지 다 연결하기
// 3. 실제 게임처럼 만들기. turret 설치 공간 추가, 적 죽일때마다 골드 얻기!

// 2024.03.05까지 숙제
// 1. turret 하나 더 만들기 (자유롭게)
// 2. turret 공간 설치 더 하기

public class Player : MonoBehaviour
{
    public GameObject centerPoint;

    public Camera mainCamera;
    public Canvas UI_Parent;

    public Text currentHpText;
    public Text currentGoldText;
    public Text timerText;

    public GameObject normalEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject tankingEnemyPrefab;

    public WayPoint[] path;

    public Level currentLevelStatus;

    private int currentLevelIndex = 0;

    private float timer = 0.0f;
    private float enemySpawnTimer = 0.0f;
    private float restTimer = 0.0f;

    private bool isRestTime = false;
    private bool isShowingUI = false;
    
    void Start()
    {
        currentLevelStatus = new Level(0,0,0);
        StaticValues.GetInstance().centerPos = centerPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentHpText.text = "HP : " + StaticValues.GetInstance().hp.ToString();
        currentGoldText.text = "Gold : " + StaticValues.GetInstance().gold.ToString();
        timerText.text = "Next wave : " + (30 - this.restTimer).ToString() + "s";

        // if StaticValues.GetInstance().draggedTurret != null){
        //     Debug.Log StaticValues.GetInstance().draggedTurret.name);
        // }
        
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
         StaticValues.GetInstance().gold = StaticValues.GetInstance().gold + 1;
            timer = 0.0f;
            // SpawnEnemy(normalEnemyPrefab.GetComponent<BaseEnemy>());
        }

        if(!isRestTime)
        {
            timerText.color = new Color(255, 255, 255, 0);
            enemySpawnTimer += Time.deltaTime;
            if(enemySpawnTimer > 5.0f)
            {
                if(currentLevelStatus.maxFastEnemy < StaticValues.GetInstance().levels[this.currentLevelIndex].maxFastEnemy)
                {
                    SpawnEnemy(fastEnemyPrefab);
                    currentLevelStatus.maxFastEnemy++;
                }
                if(currentLevelStatus.maxFastEnemy < StaticValues.GetInstance().levels[this.currentLevelIndex].maxNormalEnemy)
                {
                    SpawnEnemy(normalEnemyPrefab);
                    currentLevelStatus.maxNormalEnemy++;
                }
                if(currentLevelStatus.maxFastEnemy < StaticValues.GetInstance().levels[this.currentLevelIndex].maxTankingEnemy)
                {
                    SpawnEnemy(tankingEnemyPrefab);
                    currentLevelStatus.maxTankingEnemy++;
                }
                enemySpawnTimer = 0.0f;

                if(currentLevelStatus.IsEqual(StaticValues.GetInstance().levels[this.currentLevelIndex]))
                {
                    LoadNewLevel();
                }
            }
        }else 
        {
            restTimer += Time.deltaTime;
            timerText.color = new Color(255, 255, 255, 255);

            if(restTimer >= 30)
            {
                isRestTime = false;
            }
        }

        // every second
        // get elapsed time
        // if elapsed time > 1 second
        // do something
        // reset elapsed time

        // 1. get elapsed time
        // 2. if elapsed time > 1 second

        if(Input.GetMouseButtonDown(0))
        {

            // ⬇ 여기는 UI element가 클릭되었는지 확인하는 부분
            var pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            // 1) raycast all은 모든 "UI"에 대해서 raycast를 수행한다.
            EventSystem.current.RaycastAll(pointer, raycastResults); 
            TurretUI validTurretUI = null;
            bool isUIClicked = false;
            foreach(var result in raycastResults)
            {
                if(result.gameObject.GetComponent<TurretUI>())
                {
                    validTurretUI = result.gameObject.GetComponent<TurretUI>();
                    isUIClicked = true;
                }
            }
            // ⬆ 여기는 UI element가 클릭되었는지 확인하는 부분



            // ⬇ 여기는 UI element가 아닌 2D GameObject가 클릭되었는지 확인하는 부분
            // 2) For non-UI 2D GameObjects
            bool isTurretClicked = false;
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(rayPos, Vector2.zero);
            foreach(var h in hit)
            {
                if(h.collider.GetComponent<BaseTurret>() && !StaticValues.GetInstance().isShowingUI)
                {
                    Debug.Log("Base Turret Clicked");
                    h.collider.GetComponent<BaseTurret>().InstantiateUI();
                    h.collider.GetComponent<BaseTurret>().isShowingUI = true;
                    isTurretClicked = true;
                    StaticValues.GetInstance().isShowingUI = true;
                } 
            }
            // ⬆ 여기는 UI element가 아닌 2D GameObject가 클릭되었는지 확인하는 부분

            if(!isUIClicked && !isTurretClicked){
                // 열려있는 UI를 모두 닫는다.
                var uiElements = this.UI_Parent.GetComponentsInChildren<TurretUI>();
                foreach(var ui in uiElements)
                {
                    ui.DestroyUI();
                    StaticValues.GetInstance().isShowingUI = false;
                }

            }
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        // 1. get random position
        // 2. spawn enemy at random position
        var startingX = this.path[0].transform.position.x;
        var startingY = this.path[0].transform.position.y;
        var startingZ = this.path[0].transform.position.z;
        var startPoint = new Vector3(startingX, startingY, startingZ);
        var newEnemy = Instantiate(enemy, startPoint, Quaternion.identity);
        newEnemy.GetComponent<BaseEnemy>().Initialize(path, mainCamera, UI_Parent);
    }

    public void GotHit()
    {
     StaticValues.GetInstance().hp -= 1;
    }
    
    public void LoadNewLevel()
    {
        this.currentLevelIndex++;
        this.currentLevelStatus = new Level(0,0,0);
        isRestTime = true;
    }
}
