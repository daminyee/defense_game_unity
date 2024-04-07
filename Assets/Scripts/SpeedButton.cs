using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    public GameObject speedText;
    public GameObject speedArrowText;
    // Start is called before the first frame update
    void Start()
    {
        speedText.GetComponent<Text>().text = "1.0";
        speedArrowText.GetComponent<Text>().text = ">";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        StaticValues.GetInstance().ChangeSpeed();

        float gameSpeed = StaticValues.GetInstance().gameSpeedOptions[StaticValues.GetInstance().currentGameSpeedIndex];
        //Debug.Log(gameSpeed);

        speedText.GetComponent<Text>().text = gameSpeed.ToString();

        // 올림
        var speedIntValue = Mathf.FloorToInt(gameSpeed);
        // 반복
        speedArrowText.GetComponent<Text>().text = new string('>', speedIntValue);

        // switch(gameSpeed)
        // {
        //     case 0.5f:
        //         speedArrowText.GetComponent<Text>().text = "<<";
        //         break;
        //     case 1.0f:
        //         speedArrowText.GetComponent<Text>().text = ">";
        //         break;
        //     case 2.0f:
        //         speedArrowText.GetComponent<Text>().text = ">>";
        //         break;
        // }
    } 
}
