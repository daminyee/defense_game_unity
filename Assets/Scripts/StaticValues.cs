using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct Level
{
    public int maxNormalEnemy;
    public int maxFastEnemy;
    public int maxTankingEnemy;
    public int maxMiniBossEnemy;
    public int maxMultipleEnemy;
    public int maxShieldEnemy;

    public Level(int fastEnemyCount, int normalEnemyCount, int tankingEnemyCount, int miniBossEnemyCount, int multipleEnemyCount, int ShieldEnemyCount)
    {
        this.maxNormalEnemy = normalEnemyCount;
        this.maxFastEnemy = fastEnemyCount;
        this.maxTankingEnemy = tankingEnemyCount;
        this.maxMiniBossEnemy = miniBossEnemyCount;
        this.maxMultipleEnemy = multipleEnemyCount;
        this.maxShieldEnemy = ShieldEnemyCount;
    }

    public bool IsEqual(Level level)
    {
        return this.maxNormalEnemy == level.maxNormalEnemy &&
            this.maxFastEnemy == level.maxFastEnemy &&
            this.maxTankingEnemy == level.maxTankingEnemy;
    }
}

// 3월 31일 숙제
// 1. Level을 10까지 늘리기
// 2. 적을 두개 더 만들기 (알아서 창의적으로)
// 3. Level이 끝나면 게임 끝났다는 것을 UI로 나타내기 
    // 3-1. 게임이 끝났다는 것을 어떻게 인지할지 생각해보기
    // 3-2. HP가 0미만이면 게임 오버

// 4월 2일 숙제
// 1. 적 하나 더 만들기
// 2. 게임 스피드 전 구역 적용하기
public class StaticValues
{
    public float hp = 5;

    public int gold = 100;

    public bool isPaused;

    public float gameSpeed;
    public int currentGameSpeedIndex;
    public float[] gameSpeedOptions;

    public BaseTurret currentSelectedTurret;

    public TurretUI openedTurretUI;

    public BaseTurret draggedTurret = null;

    //public bool isShowingUI = false;

    public Level[] levels;

    public Vector2 centerPos;

    private StaticValues() {
        Initialize();
    }
    public static StaticValues instance;
    public static StaticValues GetInstance()
    {
        if(instance == null)
        {
            instance = new StaticValues();
        }
        return instance;
    }

    public void Initialize() 
    {
        this.currentGameSpeedIndex = 0;
        this.gameSpeedOptions = new float[]
        {
            1.0f, 1.5f, 2.0f, 3.0f
        };
        this.levels = new Level[10]
        {
            new Level(5, 0, 0, 0, 1, 1),
            new Level(10, 0, 0, 0, 1, 0),
            new Level(10, 5, 0, 0, 2, 0),
            new Level(10, 10, 0, 0, 3, 0),
            new Level(10, 10, 0, 1, 0, 1),
            new Level(10, 10, 5, 0, 3, 1),
            new Level(15, 10, 5, 0, 3, 0),
            new Level(15, 15, 5, 0, 2, 1),
            new Level(25, 20, 10, 0, 2, 0),
            new Level(30, 30, 15, 2, 3, 2)
        };

        this.hp = 5;
        this.gold = 100;

        this.draggedTurret = null;
    }

    public void ChangeSpeed() 
    {
        if(instance == null)
        {
            instance = new StaticValues();
        }
        currentGameSpeedIndex = (currentGameSpeedIndex + 1) % gameSpeedOptions.Length;
        
        Time.timeScale = gameSpeedOptions[currentGameSpeedIndex];
    }
}
