using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//버그 스테미나가 안닳음 
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

[System.Serializable] //형태 알려줌 
public class Condition //: MonoBehaviour
{
    //HP SM HG 값 만듬 
    [HideInInspector]
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public float decayRate;
    public Image uiBar; 

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f); //0보다 안 낮아지게 0.0함 
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }

    
}
public class PlayerConditions : MonoBehaviour, IDamagable //클래스가 아닌 인터페이스라 다중상속 ㄱㄴ
{
    public Condition health; //HP
    public Condition hunger; //허기 
    public Condition stamina; //스테미나 

    public float noHungerHealthDecay;

    public UnityEvent onTakeDamage;

    void Start()
    {
        
        health.curValue = health.startValue;
        hunger.curValue = hunger.startValue;
        stamina.curValue = stamina.startValue;
    }

    void Update()
    {
        //계속 수치 떨어지도록 
        hunger.Subtract(hunger.decayRate * Time.deltaTime); 
        stamina.Add(stamina.regenRate * Time.deltaTime);
        
        if(hunger.curValue == 0.0f) //배고프면 피 달듯 
            health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue == 0.0f)
            Die(); //health 0이면 다이매서드 실행 

        health.uiBar.fillAmount = health.GetPercentage();
        hunger.uiBar.fillAmount = hunger.GetPercentage();
        stamina.uiBar.fillAmount = stamina.GetPercentage();
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount) 
    {
        hunger.Add(amount);
    }

    public bool UseStamina(float amount)
    {
        if(stamina.curValue - amount < 0)
            return false; 

        stamina.Subtract(amount);
        return true;
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었어요");
    }

    public void TakePhysicalDamage(int damageAmount) //인터페이스꺼 구현 
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}
