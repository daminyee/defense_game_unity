using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossEnemy : BaseEnemy
{
    public GameObject dashEffectObject;

    public float dashCoolTime;
    private float dashCoolDown;

    void Start()
    {
        SetMap();
    }

    void Update()
    {
        this.MoveToNextWayPoint();
        if (dashCoolDown >= dashCoolTime)
        {
            Dash();
        }
        dashCoolDown += Time.deltaTime;
    }

    void Dash()
    {
        this.dashCoolDown = 0;
        StartCoroutine(PlayDashAnimation());
    }

    IEnumerator PlayDashAnimation()
    {

        var trailRenderer = dashEffectObject.GetComponent<TrailRenderer>();
        var trailRendererMaxTime = 5f;
        float accelerateTime = 0f;
        float delayTime = 0f;
        trailRenderer.emitting = true;
        trailRenderer.time = trailRendererMaxTime;
        while (delayTime < 1f)
        {
            delayTime += Time.deltaTime * 1.5f;
            yield return null; // while문 안에 yield return null을 넣어주는 이유는 while문이 돌면서 1프레임씩 기다려주기 위함
        }
        var maxAccelerateTime = 1f;
        var accelerateTimeDelta = Time.deltaTime * 1.5f;
        while (accelerateTime < maxAccelerateTime)
        {
            accelerateTime += accelerateTimeDelta;
            //dashEffectObject.SetActive(true);
            this.speed = this.originalSpeed + 1;
            trailRenderer.time = Mathf.Lerp(trailRendererMaxTime, 0, accelerateTime / maxAccelerateTime);
            yield return null; // while문 안에 yield return null을 넣어주는 이유는 while문이 돌면서 1프레임씩 기다려주기 위함
        }
        trailRenderer.emitting = false;
        trailRenderer.time = trailRendererMaxTime;
        this.speed = this.originalSpeed;
    }
}
