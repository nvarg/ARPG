﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    Transform player;
    public int currentHP;
    public Stat maxHP;
    public Stat damage;
    public Stat armor;
    Animator anim;
    Animator playerAnim;
    

    // Temporary test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
           Heal(10);
        }
    }

    public virtual void Start()
    {
        player = Player.instance.transform;
        playerAnim = player.GetComponent<Animator>();
        anim = GetComponent<Animator>();
        // Spawn with Max HP
        currentHP = maxHP.GetStat();
    }

    // Calculate and deal damage
    public void TakeDamage(int rawDamage)
    {
        // Reduce damage with armor
        int finalDamage = rawDamage;
        finalDamage -= armor.GetStat();
        finalDamage = Mathf.Clamp(finalDamage, 0, int.MaxValue);

        // Take damage
        currentHP -= finalDamage;

        // Die when health reaches 0
        if (currentHP <= 0)
        {
           // Might need to make this overwriteable to give enemies deaths too
           StartCoroutine(Dead());
         
        }
    }

    // Player death
    public IEnumerator Dead()
    {

        // If player dies
        if (transform.tag == "Player")
        {

            // Play death animation
            anim.SetBool("dead", true);

            // TODO: Game Over message


            // Wait
            yield return new WaitForSeconds(2f);

            // Restart level Prompt
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // if anything else dies
        else
        {

            // play death animation
           anim.Play("Die");
  
            // destroy it
           yield return new WaitForSeconds(2f);
           Destroy(gameObject);

            // player stops attacking
            playerAnim.SetBool("RClick", false);


        }

    }

    // Heal the character.
    public void Heal(int rawHeal)
    {
        currentHP += rawHeal;
        

        // Prevent overheal
        currentHP = Mathf.Clamp(currentHP, 0, maxHP.GetStat());
    }



}