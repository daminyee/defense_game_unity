using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Level
{
    public int maxNormalEnemy;
    public int maxFastEnemy;
    public int maxTankingEnemy;

    public Level(int fastEnemyCount, int normalEnemyCount, int tankingEnemyCount)
    {
        this.maxNormalEnemy = normalEnemyCount;
        this.maxFastEnemy = fastEnemyCount;
        this.maxTankingEnemy = tankingEnemyCount;
    }

    public bool IsEqual(Level level)
    {
        return this.maxNormalEnemy == level.maxNormalEnemy &&
            this.maxFastEnemy == level.maxFastEnemy &&
            this.maxTankingEnemy == level.maxTankingEnemy;
    }
}

public class StaticValues
{
    public float hp = 5;

    public int gold = 100;

    public BaseTurret currentSelectedTurret;

    public TurretUI openedTurretUI;

    public BaseTurret draggedTurret = null;

    //public bool isShowingUI = false;

    public Level[] levels = new Level[3]
    {
        new Level(5, 0, 0),
        new Level(10, 5, 0),
        new Level(10, 10, 5)
    };

    public Vector2 centerPos;

    private StaticValues() {}
    public static StaticValues instance;
    public static StaticValues GetInstance()
    {
        if(instance == null)
        {
            instance = new StaticValues();
        }
        return instance;
    }
}
