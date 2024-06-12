using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdditionalStaticValuesForGame2
{
    // public Dictionary<int, int> turretCountPerColumn; // column은 열 (row 는 행입니다)

    public Dictionary<Tuple<int, int>, bool> turretSpace; // turret이 있는지 없는지 확인하는 dictionary
    // tuple은 (n1, n2, ..) 이런식으로 사용할 수 있습니다. 여러개의 값을 하나로 묶어서 사용할 수 있습니다.
    // array랑 무슨 차이?
    // 1. array는 같은 데이터 타입만 넣을 수 있지만 tuple은 다른 데이터 타입도 넣을 수 있습니다.
    // 2. tuple은 array처럼 값을 추가할 수 없다

    public int maxTurretSpaceRowCount;
    public int maxTurretSpaceColumnCount;
    private AdditionalStaticValuesForGame2()
    {
        Initialize();
    }

    public bool IsTurretInstalled(int rowIndex, int columnIndex)
    {
        var key = new Tuple<int, int>(rowIndex, columnIndex);
        return turretSpace[key];
    }

    public void SetTurretInstallStatus(int rowIndex, int columnIndex, bool isInstalled)
    {
        var key = new Tuple<int, int>(rowIndex, columnIndex);
        turretSpace[key] = isInstalled;
    }

    public static AdditionalStaticValuesForGame2 instance;
    public static AdditionalStaticValuesForGame2 GetInstance()
    {
        if (instance == null)
        {
            instance = new AdditionalStaticValuesForGame2();
        }
        return instance;
    }

    // public void AddTurretCountToColumn(int columnIndex)
    // {
    //     turretCountPerColumn[columnIndex] += 1;
    // }
    // public void SubtractTurretCountFromColumn(int columnIndex)
    // {
    //     turretCountPerColumn[columnIndex] -= 1;
    // }
    public void Initialize()
    {
        turretSpace = new Dictionary<Tuple<int, int>, bool>();
    }
}
