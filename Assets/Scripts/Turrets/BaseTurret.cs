using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 숙제 일요일까지 (1월 24일)
// 1. 터렛을 놓으면 아무데나 놓이는 게 아닌, 딱 중앙에 배치하게 끔 하기
// 2. 터렛이 해당 space에 맞닿아 있으면, 표시가 되게끔 하기
// 3. 터렛이 하나만 놓이게 하기
// 4. 금액관리 (터렛 가격 설정 및 터렛이 설치되면 금액 차감하기)
// 5. Gold가 부족하면 못 설치한다는 것을 표시하는 방법을 알아서 구상하기

// 화요일까지 (1월 28일)
// 1. 터렛 스페이스를 규칙적으로 다 배치하세요
// 2. 터렛에 있는 터렛이 다가오는 적을 공격하게 하세요. 터렛이 적을 향해 바라보게 하세요.
// 3. 적이 공격받고 Destroy되게 하세요

// 일요일까지 (2월 4일)
// 1. 터렛 공격 실현하기 (투사체가 나와야 된다) + 적 죽는 것까지
// 2. 터렛 설치전까지 attack range가 보이게 하기

// 화요일까지 (2월 6일)
// 1. 적 죽는 것 구현
// 2. unhighlight 제대로 구현하기
// 3. 다른 터렛 하나 만들기

// 다음주까지 (2월 13일)
// 1. 드래그 중에 turret이 적을 바라봅니다 (어떻게 구현하면 좋은지는 잘 고민해보기, install 전까지 inactive)
// 2. 설치된 turret에 새로운 turret 갖다대면 highlight 불가능하게 만들기 (TurretSpace의 installedTurret 변수 사용)
// 3. 설치된 turret을 클릭하면, 그 터렛 옆에 "UI"가 뜨게하기 (업그레이드 버튼, 판매 버튼) + 가능하면 Upgrade 실행도 되게끔 하기


// 일요일까지
// 1. 오류 나는 거 일단 해결
// 2. 팝업이 제대로 뜨게 해주기 (터렛 옆에) + 화면 바깥에 가지는 않게끔 잘 해보기
// 3. 업그레이드 버튼 누르면 업그레이드 되게끔 하기


// 화요일까지 (2월 20일)
// 1. UI 이외의 부분을 클릭하면 UI 사라지게 하기 (어떻게 하냐면 )
// RectTransformUtility.ScreenPointToLocalPointInRectangle(instantiatedTurretUI.GetComponent<RectTransform>(), screenPoint, canvasUI.worldCamera, out localPoint);
// ScreenPointToLocalPointInRectangle은 boolean을 내놓는다. true면 해당 screenpoint가 UI 안에 있다는 것. false면 UI 밖에 있다는 것
// 이것을 통해 UI를 껐다 킬 수 있게 하기
// 2. 위에 것을 아예 못하겠으면. 최소한 UI 끌 수 있는 x 버튼 만들기]
// 이것을 하려면 UI가 켜져있는지 안 켜져있는지 확인할 수 있는 방법을 준비해야합니다.

// 일요일까지 (3월 10일)
// 1. attackrange에 따라사 ray cast가 되게끔 하기 (attackRange에 거리 변수 하나 필요할 것)
// 2. 판매로직 구현하기 (판매하면, 업그레이드 비용 포함해서 돈을 늘려야 돼)
// 3. 터렛 업그레이드하면 저 새로 만든 3개 쏘는 걸로 바뀌게끔 해보기 

// 화요일까지 (4월 9일)
// 테마: 다음 수업 때 터렛이 오작동하고 있는 이유를 분석할 것인데,
// 밑단 작업
// 1. Turret이 바라바고 있는 지점을 Debug.Ray 하기 (터렛이 바라보는 방향을 알아야 함)
// 2. 각 터렛마다 isDebugMode를 넣기 (public)
//      (설명: isDebugMode 개별 터렛 분석용 변수.)
// 3. 터렛이 isDebugMode가 켜져있다면 Update문에 바라보고 있는 적들을 Debug.Log 나열하게 하세요 (Update)
//    Dictionary 정보를 가져오게 해라
// 4. 이외, 터렛이 오작동하는 이유를 분석할 수 있는 요소들을 Debug Log하는 밑단 작업을 완성하기
//    (조건문이 isDebugMode일때만 해당 내용들을 Debug.Log할 수 있게끔 작업)


// 화요일까지 (4월 16일)
// 숙제: 숫자가 안 없어지는 문제 혼자서 해결해보기
//      + ) 다른 버그가 있는지 계속 테스트하긴
public abstract class BaseTurret : MonoBehaviour
{
    public TurretSpace turretSpace;
    public Vector3 spacePos;
    public GameObject bulletPrefab;
    public GameObject attackRange;
    public Canvas canvasUI;
    public Camera mainCamera;

    public GameObject turretUI_Prefab;
    public GameObject openedTurretUI { get; private set; }
    public bool isShowingUI = false;


    public BaseEnemy targetEnemy { get; private set; }
    public Dictionary<BaseEnemy, float> enemiesToAttack = new Dictionary<BaseEnemy, float>();

    public float attackPower;
    public float attackSpeed;
    public float attackCoolCount;

    public int price;
    public int upgradePrice;
    public int sellPrice;

    public bool isDebugMode;

    public bool isInValidSpace = false;
    public bool isHighlightingAnyTurretSpace = false;

    public bool isInstalledTurret = false;

    public bool isSelected = false;

    public Dictionary<TurretSpace, float> turretSpaceDistances = new Dictionary<TurretSpace, float>();

    public abstract void Attack();

    public float attackPowerDelta;
    public float attackSpeedDelta;
    public int upgradeLevel;
    public int upgradeCount = 0;
    //public bool isMaxUpgrade;

    public List<GameObject> upgradePrefabs; // 어떤 터렛은 여러번 prefab가 변경 가능하기 떄문


    public void UpgradeTurret()
    {
        this.attackPower += attackPowerDelta;
        this.attackSpeed += attackSpeedDelta;
        upgradeLevel += 1;

        if (upgradeLevel >= 3 && upgradeCount < upgradePrefabs.Count && upgradePrefabs[upgradeCount] != null)
        {
            GameObject newTurretGameObject = Instantiate(upgradePrefabs[upgradeCount], this.transform.position, Quaternion.identity);
            BaseTurret upgradeTurret = newTurretGameObject.GetComponent<BaseTurret>();
            CopyDataToNewTurret(upgradeTurret);
            attackRange = upgradeTurret.attackRange;
            upgradeTurret.MakeAttackRangeInvisible();

            StaticValues.GetInstance().openedTurretUI.DestroyUI();
            upgradeTurret.InstantiateUI();
            Destroy(this.gameObject);
        }
    }
    //public abstract void SellTurret();


    public void Initialize(bool isSelected, Camera mainCamera, Canvas canvasUI)
    // Start와 동일한 역할을 하는 함수
    {
        this.isSelected = isSelected;
        this.mainCamera = mainCamera;
        this.canvasUI = canvasUI;

        this.enemiesToAttack = new Dictionary<BaseEnemy, float>();
        this.turretSpaceDistances = new Dictionary<TurretSpace, float>();
        this.isSelected = true;
        this.sellPrice = price / 2;
    }



    public void CopyDataToNewTurret(BaseTurret newTurret)
    {
        turretSpace.installedTurret = newTurret;
        newTurret.turretSpace = this.turretSpace;
        newTurret.canvasUI = this.canvasUI;
        newTurret.mainCamera = this.mainCamera;
        newTurret.isInstalledTurret = true;

        newTurret.upgradePrefabs = this.upgradePrefabs;
        newTurret.upgradeLevel = 0;
        newTurret.upgradeCount = this.upgradeCount + 1;
        newTurret.turretSpaceDistances = this.turretSpaceDistances;
        newTurret.enemiesToAttack = this.enemiesToAttack;
        StaticValues.GetInstance().currentSelectedTurret = newTurret;
    }

    void UpdateDistance(TurretSpace turretSpaceObject, float distance)
    {
        if (turretSpaceDistances.ContainsKey(turretSpaceObject))
        {
            turretSpaceDistances[turretSpaceObject] = distance;
        }
        else
        {
            turretSpaceDistances.Add(turretSpaceObject, distance);
        }
    }

    void DeleteDistance(TurretSpace turretSpaceObject)
    {
        if (turretSpaceDistances.ContainsKey(turretSpaceObject))
        {
            turretSpaceDistances.Remove(turretSpaceObject);
        }
    }


    void OnTriggerStay2D(Collider2D collider)
    {
        if (this.isInstalledTurret)
        {
            return;
        }

        if (collider.GetComponent<TurretSpace>() == null)
        {
            return;
        }
        var collidedTurretSpace = collider.GetComponent<TurretSpace>();
        var distanceToTurretSpace = Vector3.Distance(this.transform.position, collidedTurretSpace.transform.position);
        UpdateDistance(collidedTurretSpace, distanceToTurretSpace); // 왜 update distance를 하냐면, 
        // turretspace가 서로 가까이 있다면, 여러 turret space가 highlight 될 수 있기 때문에

        TurretSpace minDistanceTurretSpace = null;
        float minTurretSpaceDistance = float.MaxValue;
        foreach (var turretSpaceDistance in turretSpaceDistances)
        {
            if (turretSpaceDistance.Value < minTurretSpaceDistance)
            {
                minTurretSpaceDistance = turretSpaceDistance.Value;
                minDistanceTurretSpace = turretSpaceDistance.Key;
            }
        }
        spacePos = minDistanceTurretSpace.transform.position;
        turretSpace = minDistanceTurretSpace;
        foreach (var turretSpaceDistance in turretSpaceDistances)
        {
            if (turretSpaceDistance.Key == minDistanceTurretSpace && this.isSelected == true)
            {
                turretSpaceDistance.Key.HighlightSpace();
            }
            else
            {
                turretSpaceDistance.Key.UnhighlightSpace();
            }
        }
        //Debug.Log(spacePos);
        this.isInValidSpace = true;

    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (this.isInstalledTurret)
        {
            return;
        }
        turretSpace = collider.GetComponent<TurretSpace>();

        if (turretSpace == null)
        {
            return;
        }

        turretSpace.UnhighlightSpace();
        DeleteDistance(turretSpace);
        if (this.turretSpaceDistances.Count <= 0)
        {
            this.isInValidSpace = false;
        }
    }

    public bool IsTurrestInstallable()
    {
        return true;
    }

    public void InstallTurret()
    {
        AdditionalStaticValuesForGame2.GetInstance().SetTurret(turretSpace.rowIndex, turretSpace.columnIndex, this); ;
        this.transform.position = spacePos;
        StaticValues.GetInstance().gold -= this.price;
        MakeAttackRangeInvisible();

        foreach (var turretSpaceDistance in turretSpaceDistances)
        {
            turretSpaceDistance.Key.UnhighlightSpace();
        }
        this.isSelected = false;
        this.isInstalledTurret = true;
        this.turretSpaceDistances = null;

        turretSpace.installedTurret = this;
        turretSpace.isInstalled = true;
        turretSpace.TurnOffIsTrigger();
        turretSpace.ShowGetGold(this.price, false);
    }

    public void AddEnemy(BaseEnemy enemy)
    {
        if (this.targetEnemy == null)
        {
            this.SetTargetEnemy(enemy);
        }
        this.enemiesToAttack.Add(enemy, 0); //오류!
    }

    public void AttackEnemy()
    {
        if (StaticValues.GetInstance().isPaused) return;

        var range = this.attackRange.GetComponent<CircleCollider2D>().bounds.size;

        // get the turret direction
        var doAttack = false;
        // shoot debug ray
        var ray = new Ray(this.transform.position, this.transform.right);
        var hit = Physics2D.RaycastAll(ray.origin, ray.direction, range.x / 2); // 거리를 조절해야함
        foreach (var h in hit)
        {
            if (h.collider.GetComponent<BaseEnemy>() != null)
            {
                doAttack = true;
                break;
            }
        }
        if (!doAttack)
        {
            return;
        }
        Attack();
    }


    protected void WatchEnemy()
    {
        if (this.enemiesToAttack.Count == 0 || StaticValues.GetInstance().isPaused)
        {
            return;
        }
        if (targetEnemy != null)
        {
            if (targetEnemy.hasPassed == true)
            {
                DeleteEnemy(targetEnemy);
                SetTargetEnemy(null);
            }
        }
        foreach (var enemyToAttack in enemiesToAttack)
        {
            if (targetEnemy == null)
            {
                this.targetEnemy = enemyToAttack.Key;
            }
            if (enemyToAttack.Key.targetPriority > this.targetEnemy.targetPriority)
            {
                this.targetEnemy = enemyToAttack.Key;
            }
        }

        if (targetEnemy == null) return;
        var xLength = this.targetEnemy.transform.position.x - this.transform.position.x;
        var yLength = this.targetEnemy.transform.position.y - this.transform.position.y;
        var angle = Math.Atan2(yLength, xLength);
        var toRotation = Quaternion.Euler(0, 0, (float)(angle * 180 / Math.PI));
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, toRotation, 0.07f);
    }

    private void SetTargetEnemy(BaseEnemy enemy)
    {
        this.targetEnemy = enemy;
    }
    // enenmy 죽었거나, 영역을 벗어났을 때
    public void DeleteEnemy(BaseEnemy enemy)
    {
        if (this.enemiesToAttack.Count == 0)
        {
            this.SetTargetEnemy(null);
            return;
        }
        var deletedEnemy = this.enemiesToAttack.Remove(enemy);
        this.SetTargetEnemy(null);
    }

    void MakeAttackRangeVisible()
    {
        attackRange.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }
    public void MakeAttackRangeInvisible()
    {
        attackRange.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
    }


    public enum Direction
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        None
    }

    public void InstantiateUI()
    {
        if (isShowingUI) return;

        MakeAttackRangeVisible();

        var centerPos = StaticValues.GetInstance().centerPos;
        Vector3 worldPoint = this.transform.position;

        Direction direction = Direction.None;

        if (centerPos.x >= this.transform.position.x && centerPos.y > this.transform.position.y)
        {
            // turret 위치는 왼쪽 아래
            direction = Direction.BottomLeft;
        }
        else if (centerPos.x >= this.transform.position.x && centerPos.y <= this.transform.position.y)
        {
            // turret 위치는 왼쪽 위
            direction = Direction.TopLeft;
        }
        else if (centerPos.x < this.transform.position.x && centerPos.y > this.transform.position.y)
        {
            // turret 위치는 오른쪽 아래
            direction = Direction.BottomRight;
        }
        else if (centerPos.x < this.transform.position.x && centerPos.y <= this.transform.position.y)
        {
            // turret 위치는 오른쪽 위
            direction = Direction.TopRight;
        }

        switch (direction)
        {
            case Direction.TopLeft:
                worldPoint.x += turretSpace.spriteBounds.x / 2;
                worldPoint.y += turretSpace.spriteBounds.y / 2;
                break;
            case Direction.TopRight:
                worldPoint.x -= turretSpace.spriteBounds.x / 2;
                worldPoint.y += turretSpace.spriteBounds.y / 2;
                break;
            case Direction.BottomLeft:
                worldPoint.x += turretSpace.spriteBounds.x / 2;
                worldPoint.y -= turretSpace.spriteBounds.y / 2;
                break;
            case Direction.BottomRight:
                worldPoint.x -= turretSpace.spriteBounds.x / 2;
                worldPoint.y -= turretSpace.spriteBounds.y / 2;
                break;
        }
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldPoint);

        var instantiatedTurretUI = Instantiate(turretUI_Prefab, screenPoint, Quaternion.identity);
        this.openedTurretUI = instantiatedTurretUI;
        StaticValues.GetInstance().openedTurretUI = instantiatedTurretUI.GetComponent<TurretUI>();
        instantiatedTurretUI.transform.SetParent(canvasUI.transform, false);

        var turretUI_Class = instantiatedTurretUI.GetComponent<TurretUI>();
        turretUI_Class.turret = this;

        // Adjust its position using the RectTransform. Convert screenPoint to local point in canvas
        RectTransform rectTransform = instantiatedTurretUI.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasUI.GetComponent<RectTransform>(), screenPoint, canvasUI.worldCamera, out localPoint);

        switch (direction)
        {
            case Direction.TopLeft:
                localPoint.x += rectTransform.rect.width / 2;
                localPoint.y -= rectTransform.rect.height / 2;
                break;
            case Direction.TopRight:
                localPoint.x -= rectTransform.rect.width / 2;
                localPoint.y -= rectTransform.rect.height / 2;
                break;
            case Direction.BottomLeft:
                localPoint.x += rectTransform.rect.width / 2;
                localPoint.y += rectTransform.rect.height / 2;
                break;
            case Direction.BottomRight:
                localPoint.x -= rectTransform.rect.width / 2;
                localPoint.y += rectTransform.rect.height / 2;
                break;
        }

        rectTransform.anchoredPosition = localPoint;
    }

    public void SellTurret()
    {
        AdditionalStaticValuesForGame2.GetInstance().SetTurret(turretSpace.rowIndex, turretSpace.columnIndex, this);
        StaticValues.GetInstance().gold += this.sellPrice;
        this.turretSpace.ShowGetGold(this.upgradePrice, true);
        turretSpace.installedTurret = null;
        turretSpace.isInstalled = false;
        turretSpace.TurnOnIsTrigger();
        Destroy(this.gameObject);
    }

    public void DebugTarget()
    {
        if (isDebugMode)
        {
            // if (targetEnemy != null)
            // {
            //     Debug.Log(targetEnemy + "is current Target");
            // }
            Debug.Log("Enemies in queue: " + enemiesToAttack.Count);

            // foreach (BaseEnemy enemyToAttack in enemiesToAttack)
            // {
            //     Debug.Log(enemyToAttack + "is in queue");
            // }
        }
    }

    public void DebugRay()
    {
        if (isDebugMode)
        {
            attackCoolCount += Time.deltaTime * attackSpeed;

            var range = this.attackRange.GetComponent<CircleCollider2D>().bounds.size;

            var ray = new Ray(this.transform.position, this.transform.right);
            var hit = Physics2D.RaycastAll(ray.origin, ray.direction, range.x / 2);
            // // show ray
            Debug.DrawRay(ray.origin, ray.direction * range.x / 2, Color.red);
        }
    }

    // public void ChangeSpeed(float gameSpeed)
    // {

    // }
    // void OnMouseDown() // collider가 있으면 클릭가능
    // {
    //     if(this.isInstalledTurret){
    //         this.InstantiateUI();
    //     }
    // }

    // public void DeleteUI(Vector3 mousePos)
    // {
    //     Debug.Log(mousePos);
    //     // Vector3 screenPoint = mainCamera.
    //     // Debug.Log(screenPoint);
    //     Vector2 localPoint;

    //     Debug.Log(RectTransformUtility.ScreenPointToLocalPointInRectangle(savedTurretUI.GetComponent<RectTransform>(), mousePos, canvasUI.worldCamera, out localPoint));
    //     if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(savedTurretUI.GetComponent<RectTransform>(), mousePos, canvasUI.worldCamera, out localPoint))
    //     {
    //         Debug.Log("삭제됨");
    //         Destroy(savedTurretUI); //instantiatedTurretUI를 가져와서 삭제해야함---------
    //         isShowingUI = false;
    //     }
    // }


    // private Vector3 WorldToScreenPoint(Vector2 screenPoint)
    // {
    //     return mainCamera.WorldToScreenPoint(screenPoint);
    // }
}
