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
    public List<Attribute> attributeList;

    public int lifeFindsAWayCounter = 0;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void initAttributes()
    {

        attributeList = new List<Attribute>();

        attributeList.Add(GameController.fire);
        attributeList.Add(GameController.water);
        attributeList.Add(GameController.electric);
        attributeList.Add(GameController.grass);
        attributeList.Add(GameController.ice);
        attributeList.Add(GameController.fighting);
        attributeList.Add(GameController.poison);
        attributeList.Add(GameController.ground);
        attributeList.Add(GameController.flying);
        attributeList.Add(GameController.psychic);
        attributeList.Add(GameController.bug);
        attributeList.Add(GameController.rock);
        attributeList.Add(GameController.ghost);
        attributeList.Add(GameController.dragon);
        attributeList.Add(GameController.dark);
        attributeList.Add(GameController.steel);
        attributeList.Add(GameController.fairy);
        attributeList.Add(GameController.normal);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void executeMove()
    {
        Move move = enemyPokemon.getRandomMove();
        Pokemon activePokemon = GameController.instance.getActivePokemon();
        // There are several moves that are not part of the normal order, but
        // might happen under certain conditions

        // 20% chance to just use a move based on current type
        if(Random.Range(0.0f, 1.0f) < 0.2f)
        {
            move = new Move("ADAPT TO SURVIVE");
            move.attribute = this.enemyPokemon.types[0];
            move.power = 80;
            move.special = true;
            executeMoveForReal(move);
            return;
        }

        // This move's too good
        if (Random.Range(0.0f, 1.0f) < 0.05f)
        {
            move = Movelist.earthquake;
            executeMoveForReal(move);
            return;
        }

        // If we're below 35% HP, can use life finds a way--but only 3 times max
        if (enemyPokemon.statBlock.getPercentHP() < 0.35 && lifeFindsAWayCounter < 3)
        {
            // 40% chance
            if(Random.Range(0.0f, 1.0f) < 0.4f)
            {
                ++this.lifeFindsAWayCounter;
                move = Movelist.lifeFindsAWay;
                executeMoveForReal(move);
                return;
            }
        }

        // If the enemy evade stat is above 0, 10% chance to reduce it
        if (activePokemon.statBlock.evasion_stage > 0)
        {
            // 20% chance
            if (Random.Range(0.0f, 1.0f) < 0.2f)
            {
                move = Movelist.closerThanTheyAppear;
                executeMoveForReal(move);
                return;
            }
        }

        // If there are any status conditions, 20% chance to heal them
        if(enemyPokemon.statuses.Count > 0 || enemyPokemon.confused)
        {
            // 20% chance
            if (Random.Range(0.0f, 1.0f) < 0.2f)
            {
                move = Movelist.cleverGirl;
                executeMoveForReal(move);
                return;
            }
        }

        // If we're slow, 20% chance to go faster
        if (enemyPokemon.statBlock.spe_stage < 3)
        {
            // 20% chance
            if (Random.Range(0.0f, 1.0f) < 0.2f)
            {
                move = Movelist.mustGoFaster;
                executeMoveForReal(move);
                return;
            }
        }

        executeMoveForReal(move);
    }

    public void executeMoveForReal(Move move)
    {
        enemyPokemon.executeMove(move, GameController.instance.getActivePokemon());

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

    public void chooseNewAttribute()
    {
        Attribute currentAttribute = this.enemyPokemon.types[0];

        Attribute newAttribute = attributeList[Random.Range(0, attributeList.Count - 1)];
        while(newAttribute == currentAttribute)
        {
            newAttribute = attributeList[Random.Range(0, attributeList.Count - 1)];
        }

        this.enemyPokemon.types.Clear();
        this.enemyPokemon.types.Add(newAttribute);

        DialogueManager.instance.addToQueue(enemyPokemon.name + " is now " + newAttribute.getName() + " type!");
    }
}
