using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyProducer
{
    public int Cost;
    public int Level;
    public int GeneratedMoney;

    public MoneyProducer(int cost, int level, int generatedMoney)
    {
        Cost = cost;
        Level = level;
        GeneratedMoney = generatedMoney;
    }
}
