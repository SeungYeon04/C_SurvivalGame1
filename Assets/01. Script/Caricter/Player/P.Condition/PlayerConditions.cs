using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//���� ���׹̳��� �ȴ��� 
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

[System.Serializable] //���� �˷��� 
public class Condition //: MonoBehaviour
{
    //HP SM HG �� ���� 
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
        curValue = Mathf.Max(curValue - amount, 0.0f); //0���� �� �������� 0.0�� 
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }

    
}
public class PlayerConditions : MonoBehaviour, IDamagable //Ŭ������ �ƴ� �������̽��� ���߻�� ����
{
    public Condition health; //HP
    public Condition hunger; //��� 
    public Condition stamina; //���׹̳� 

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
        //��� ��ġ ���������� 
        hunger.Subtract(hunger.decayRate * Time.deltaTime); 
        stamina.Add(stamina.regenRate * Time.deltaTime);
        
        if(hunger.curValue == 0.0f) //������� �� �޵� 
            health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue == 0.0f)
            Die(); //health 0�̸� ���̸ż��� ���� 

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
        Debug.Log("�÷��̾ �׾����");
    }

    public void TakePhysicalDamage(int damageAmount) //�������̽��� ���� 
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}
