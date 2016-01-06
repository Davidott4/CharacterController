using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

	public float health;
	public float maxHealth = 100;
	public float mana;
	public float maxMana = 100;
	public float manaRegenRate = 1;
	public float stamina;
	public float maxStamina = 100;
	public float staminaRegenRate = 5;
	public float healthRegenRate = 1;
	public float rollStaminaCost =30;
	public float attackStaminaCost =30;
	public bool dead;
	public bool blocking;
	public bool sprinting;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		stamina = maxStamina;
		dead = false;
		blocking = false;
		sprinting = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (health <=0)
			Die();
		HandleRegeneration(Time.deltaTime);
	}

	void HandleRegeneration(float timeElapsed)
	{
		health +=(timeElapsed*healthRegenRate);
		if (health > maxHealth)
			health = maxHealth;
		mana +=(timeElapsed*manaRegenRate);
		if (mana > maxMana)
			mana = maxMana;

		if (!blocking)
		{
			stamina +=(timeElapsed*staminaRegenRate);
			if (stamina > maxStamina)
				stamina = maxStamina;
		}

		if (stamina <0)
			stamina =0;
		if (mana <0)
			mana =0;
	}

	void Die()
	{
		
	}
}
