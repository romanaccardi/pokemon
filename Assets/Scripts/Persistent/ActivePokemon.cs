using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivePokemon : MonoBehaviour
{
    // Make this a persistent static class
    public static ActivePokemon instance;

    // Has a Pokemon object
    public Pokemon pokemon;

    // Various GameObjects reflecting UI elements relevant to the Pokemon
    public Image healthBar;
    public SpriteRenderer spriteRenderer;
    public Text nameText;

    // Move button texts
    public Text textButton0;
    public Text textButton1;
    public Text textButton2;
    public Text textButton3;

    public List<Text> buttonTexts;

    void Awake()
    {
        instance = this;

        buttonTexts.Add(textButton0);
        buttonTexts.Add(textButton1);
        buttonTexts.Add(textButton2);
        buttonTexts.Add(textButton3);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update all UI elements according to the current state of the Pokemon
    public void updateUIElements()
    {
        // HP first
        healthBar.fillAmount = (float)pokemon.statBlock.current_hp / pokemon.statBlock.base_hp;

        // ensure that the sprite is correct and update transform
        spriteRenderer.sprite = pokemon.sprite;
        transform.localScale = pokemon.spriteScale;

        // Update name
        nameText.text = pokemon.name;

        // Update buttons to say the correct moves
        for (int i = 0; i < buttonTexts.Count && i < pokemon.moves.Count; ++i)
        {
            buttonTexts[i].text = pokemon.moves[i].name;
        }
    }

    public void takeDamage(int damageAmount)
    {
        pokemon.takeDamage(damageAmount);
        updateUIElements();
    }

    public void switchActivePokemon(Pokemon newPokemon)
    {
        pokemon = newPokemon;
        pokemon.statBlock.resetStatsExceptHP();
        DialogueManager.instance.addToQueue("Go, " + newPokemon.name + "!");
        updateUIElements(); // TODO - wait to update UI elements until it's actually been switched
    }

    public void queueMoveAt(int moveNum)
    {
        if (GameController.instance.gameState == GameState.WAITING_FOR_INPUT)
        {
            GameController.instance.queuedPlayerAction = (PlayerInput)moveNum;
            GameController.instance.gameState = GameState.INPUT_RECEIVED;
        }
    }

    public void executeMoveAt(int moveNum)
    {
        Pokemon enemyPokemon = GameController.instance.getEnemyPokemon();

        if ( moveNum < pokemon.moves.Count )
        {
            Debug.Log("boom");
            pokemon.executeMoveAt(moveNum, GameController.instance.getEnemyPokemon());
        }

        this.updateUIElements();
    }

}
