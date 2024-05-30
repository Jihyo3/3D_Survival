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
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);  //��⸶�� ���ɿ� ���� �ð��� �޶��� ���� �ֱ� ������ Time.deltaTime ���
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
        Debug.Log("�׾���!");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
} 
