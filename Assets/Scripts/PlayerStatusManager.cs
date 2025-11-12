using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager Instance;

    [Header("From Indoor")]
    public int batteryCount = 3; // 실내에서 전달받을 값(예시)
    [HideInInspector] public int health;
    [HideInInspector] public int currentQuizIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitHealth();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitHealth()
    {
        // 예: 배터리 1개당 10 체력 (원하면 조정)
        health = Mathf.Max(1, batteryCount) * 10;
    }

    public void SetBatteryCount(int count)
    {
        batteryCount = count;
        InitHealth();
    }

    public void LoseHealth(int amount)
    {
        health -= amount;
        if (health < 0) health = 0;
    }
}
