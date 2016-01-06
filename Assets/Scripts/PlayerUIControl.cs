using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class PlayerUIControl : MonoBehaviour {

	Stats plStats;

	public Slider healthBar;
	public Slider staminaBar;
	public Slider manaBar;

	// Use this for initialization
	void Start () {
		plStats = GetComponent<Stats>();
	
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.value = plStats.health/plStats.maxHealth;
		staminaBar.value = plStats.stamina/plStats.maxStamina;
		manaBar.value = plStats.mana/plStats.maxMana;
	}
}
