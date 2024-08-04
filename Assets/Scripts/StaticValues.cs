using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyType { Normal, Fast, Tanking, MiniBoss, Multiple, Shield }

public struct Level
{
    public Dictionary<EnemyType, int> maxEnemyCountDict;

    // constructor (construct 뜻? 건설하다)
    public Level(int fastEnemyCount, int normalEnemyCount, int tankingEnemyCount, int miniBossEnemyCount, int multipleEnemyCount, int ShieldEnemyCount)
    {
        this.maxEnemyCountDict = new Dictionary<EnemyType, int>();
        this.maxEnemyCountDict.Add(EnemyType.Normal, normalEnemyCount);
        this.maxEnemyCountDict.Add(EnemyType.Fast, fastEnemyCount);
        this.maxEnemyCountDict.Add(EnemyType.Tanking, tankingEnemyCount);
        this.maxEnemyCountDict.Add(EnemyType.MiniBoss, miniBossEnemyCount);
        this.maxEnemyCountDict.Add(EnemyType.Multiple, multipleEnemyCount);
        this.maxEnemyCountDict.Add(EnemyType.Shield, ShieldEnemyCount);
        if (this.maxEnemyCountDict.Count != Enum.GetNames(typeof(EnemyType)).Length)
        {
            Debug.LogError("Level Constructor Error: EnemyType Count is not 6");
        }
    }

    public Level(int minEnemyCountPerType, int maxEnemyCountPerType)
    {
        this.maxEnemyCountDict = new Dictionary<EnemyType, int>();
        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            var randomEnemyCount = UnityEngine.Random.Range(minEnemyCountPerType, maxEnemyCountPerType);
            this.maxEnemyCountDict.Add(enemyType, randomEnemyCount);
        }
    }

    public Level(int totalEnemyCount)
    {
        // divide the total enemy count by 6
        int enemyCountPerType = totalEnemyCount / Enum.GetNames(typeof(EnemyType)).Length;
        this.maxEnemyCountDict = new Dictionary<EnemyType, int>();
        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            this.maxEnemyCountDict.Add(enemyType, enemyCountPerType);
        }
    }

    public int GetEnemyCount(EnemyType enemyType)
    {
        return maxEnemyCountDict[enemyType];
    }

    public bool IsEqual(Level level)
    {
        var currentMaxEnemyCount = this.maxEnemyCountDict; // Copy to a local variable

        return currentMaxEnemyCount.Keys.Count == level.maxEnemyCountDict.Keys.Count &&
               currentMaxEnemyCount.Keys.All(key => level.maxEnemyCountDict.ContainsKey(key) && object.Equals(level.maxEnemyCountDict[key], currentMaxEnemyCount[key]));
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
    public float hp = 500;

    public int gold = 100;

    public bool isPaused;

    public float gameSpeed;
    public int currentGameSpeedIndex;
    public float[] gameSpeedOptions;

    public int livingEnemyCount = 0;

    public BaseTurret currentSelectedTurret;

    public TurretUI openedTurretUI;

    public BaseTurret draggedTurret = null;

    //public bool isShowingUI = false;

    public Level[] levels;

    public Vector2 centerPos;

    private StaticValues()
    {
        Initialize();
    }
    public static StaticValues instance;
    public static StaticValues GetInstance()
    {
        if (instance == null)
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
        this.livingEnemyCount = 0;
        this.levels = new Level[10]
        {
            new Level(0, 0, 0, 0, 2, 0),
            new Level(0, 0, 0, 0, 1, 0),
            new Level(0, 0, 0, 0, 2, 0),
            new Level(0, 0, 0, 0, 3, 0),
            new Level(0, 0, 0, 0, 0, 0),
            new Level(0, 0, 0, 0, 3, 0),
            new Level(0, 0, 0, 0, 3, 0),
            new Level(0, 0, 0, 0, 2, 0),
            new Level(0, 0, 0, 0, 2, 0),
            new Level(0, 0, 0, 0, 3, 0)
        };

        this.hp = 500;
        this.gold = 100;

        this.draggedTurret = null;
    }

    public void ChangeSpeed()
    {
        if (instance == null)
        {
            instance = new StaticValues();
        }
        currentGameSpeedIndex = (currentGameSpeedIndex + 1) % gameSpeedOptions.Length;

        Time.timeScale = gameSpeedOptions[currentGameSpeedIndex];
    }
}
