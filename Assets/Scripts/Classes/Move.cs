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

    public Move(string name)
    {
        this.name = name;
        callback = null;
        this.additionalMessage = "";
        this.accuracy = 1.0f;
    }

    public void execute(Pokemon attacker, Pokemon defender)
    {

        // First, check to see whether the move is damaging or not
        // If not, you can just execute the callback and return
        if( power == 0 )
        {
            if( callback != null )
            {
                callback.callback(attacker, defender, 0);
            }
            return;
        }

        // TODO - check for miss

        int injury;

        bool crit = Random.Range(0.0f, 1.0f) < 0.0625f;

        injury = Pokemon.calculateAdjustedInjury(attacker, defender, this, crit);

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
        else if (Pokemon.findTypeMultiplier(defender.types, this) < 0.9f)
        {
            DialogueManager.instance.addToQueue("It's not very effective...");
        }

        // return before the callback if the defender is unconscious
        // TODO this is a bit of a hack, sometimes the callback probably should be executed
        if (defender.statBlock.current_hp <= 0)
        {
            defender.conscious = false;
            return;
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
