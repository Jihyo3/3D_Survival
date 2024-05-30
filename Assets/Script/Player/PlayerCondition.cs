using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);  //기기마다 성능에 따라 시간이 달라질 수도 있기 때문에 Time.deltaTime 사용
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if(health.curValue == 0f)
        {
            Die();
        } 
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount); 
    }

    public void SpeedUp(float amount)
    {
        
    }

    public void SizeUp(float amount)
    {
        transform.localScale = new Vector3(amount, amount, amount);
    }

    public void SizeUpTime(float time)
    {
        StartCoroutine(ResetSize(time));
    }

    IEnumerator ResetSize(float time)
    {
        yield return new WaitForSeconds(time);

        transform.localScale = new Vector3(1, 1, 1);
    }

    public void Die()
    {
        Debug.Log("죽었다!");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
} 
