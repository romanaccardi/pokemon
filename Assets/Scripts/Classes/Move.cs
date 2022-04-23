using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{

    public Attribute attribute;
    public int power;
    public string name;
    public MoveCallback callback;
    public bool special;
    public string additionalMessage;
    public float accuracy;
    public bool executeCallbackEvenIfEnemyFainted;

    public Move(string name)
    {
        this.name = name;
        callback = null;
        this.additionalMessage = "";
        this.accuracy = 1.0f;
        this.executeCallbackEvenIfEnemyFainted = false;
    }

    public void execute(Pokemon attacker, Pokemon defender)
    {

        // First, check to see whether the move is damaging or not
        // If not, you can just execute the callback and return
        if( power == 0 )
        {
            if(Random.Range(0.0f, 1.0f) > this.accuracy)
            {
                DialogueManager.instance.addToQueue("But, it failed!");
                return;
            }
            if( callback != null )
            {
                callback.callback(attacker, defender, 0);
            }
            return;
        }

        bool miss = Random.Range(0.0f, 1.0f) > this.accuracy * attacker.statBlock.getChanceToHit();
        if(miss)
        {
            DialogueManager.instance.addToQueue(attacker.name + "'s attack missed!");
            return;
        }

        bool evade = Random.Range(0.0f, 1.0f) < defender.statBlock.getChanceToEvade();
        if(evade)
        {
            DialogueManager.instance.addToQueue(defender.name + " evaded the attack!");
            return;
        }

        bool crit = Random.Range(0.0f, 1.0f) < 0.0625f;

        int injury = Pokemon.calculateAdjustedInjury(attacker, defender, this, crit);

        // TODO - animation

        // Deal damage to the defending Pokemon
        defender.takeDamage(injury);

        // update the UI elements to reflect new damage
        ActivePokemon.instance.updateUIElements();
        EnemyPokemon.instance.updateUIElements();

        if (crit)
        {
            DialogueManager.instance.addToQueue("Critical hit!");
        }

        if (Pokemon.findTypeMultiplier(defender.types, this) > 1.1f)
        {
            DialogueManager.instance.addToQueue("It's super effective!");
        }
        else if (Pokemon.findTypeMultiplier(defender.types, this) == 0.0f)
        {
            DialogueManager.instance.addToQueue("The move had no effect.");
        }
        else if (Pokemon.findTypeMultiplier(defender.types, this) < 0.9f)
        {
            DialogueManager.instance.addToQueue("It's not very effective...");
        }

        // return before the callback if the defender is unconscious
        if (defender.statBlock.current_hp <= 0)
        {
            defender.conscious = false;
            if(!this.executeCallbackEvenIfEnemyFainted)
            {
                return;
            }
        }

        if ( this.additionalMessage != "" )
        {
            DialogueManager.instance.addToQueue(this.additionalMessage);
        }

        // execute callback if there is one
        if ( callback != null )
        {
            callback.callback(attacker, defender, injury);
        }
        
    }

}
