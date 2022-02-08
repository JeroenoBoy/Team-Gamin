using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public int maxHP = 100;
    public int maxArmor = 25;
    public int hp;
    public int armor;

    public Healthbar healthbar;
    public ArmorBar armorbar;

    void Start()
    {
        hp = maxHP;
        armor = maxArmor;
        healthbar.SetMaxHealth(maxHP);
        armorbar.SetMaxArmor(maxArmor);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(5);
        }

        if (armor <= 0)
        {
            armor = 0;
        }
        if (hp <= 0)
        {
            hp = 0;
            //ded
        }
    }

    public void TakeDamage(int damage)
    {
        if (armor != 0)
        {
            armor -= damage;
            armorbar.SetArmor(armor);
        }
        else
        {
            hp -= damage;
            healthbar.SetHealth(hp);
        }
    }
}
