using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPokemon : MonoBehaviour
{
    // Make this a persistent static class
    public static EnemyPokemon instance;

    // Has a Pokemon
    public Pokemon enemyPokemon;

    // Various GameObjects reflecting UI elements relevant to the Pokemon
    public Image healthBar;
    public SpriteRenderer spriteRenderer;
    public Text nameText;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void executeMove()
    {
        // TODO -- decide on a move
        enemyPokemon.executeRandomMove(GameController.instance.getActivePokemon());
        this.updateUIElements();
    }

    // Update all UI elements according to the current state of the Pokemon
    public void updateUIElements()
    {
        // HP first
        healthBar.fillAmount = (float)enemyPokemon.statBlock.current_hp / enemyPokemon.statBlock.base_hp;

        // Update name
        nameText.text = enemyPokemon.name;
    }

    public void loadEnemyPokemon(Pokemon poke)
    {
        this.enemyPokemon = poke;
        updateUIElements();
    }
}
