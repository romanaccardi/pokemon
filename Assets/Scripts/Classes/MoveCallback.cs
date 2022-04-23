using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats
{
    HP,
    ATTACK,
    DEFENSE,
    SP_ATTACK,
    SP_DEFENSE,
    SPEED,
    NONE
}

public class MoveCallback
{
    //// Chance to inflict status effects on opponent
    //public Status possibleStatus;
    //public float chanceToInflictStatusEffect;

    //// Chance to inflict abnormal status effects on opponent
    //public float flinchChance;
    //public float confusionChance;

    //// Chance to raise or lower your own stats or your opponent's stats
    //// along with which stat to change and how much to change it by
    //public Stats raiseOpponent;
    //public Stats lowerOpponent;
    //public Stats raiseSelf;
    //public Stats lowerSelf;
    //public float chanceToChangeStat;
    //public float percentChangeStat;
    //public int absoluteChangeStat;

    //// Miscellaneous
    //public bool switchActivePokemon;
    //public string callbackDescription;

    // This could change everything
    public delegate void CallbackFunc(Pokemon attacker, Pokemon defender, int damageDealt);

    public CallbackFunc callback;

    public MoveCallback()
    {
        //// status effects
        //possibleStatus = Status.NONE;
        //chanceToInflictStatusEffect = 0.0f;

        //// abnormal status effects
        //flinchChance    = 0.0f;
        //confusionChance = 0.0f;

        //// Raising/lowering stats
        //raiseOpponent       = Stats.NONE;
        //lowerOpponent       = Stats.NONE;
        //raiseSelf           = Stats.NONE;
        //lowerSelf           = Stats.NONE;
        //chanceToChangeStat  = 0.0f;
        //percentChangeStat   = 0.0f;
        //absoluteChangeStat  = 0;

        //// miscellaneous
        //switchActivePokemon = false;
        //callbackDescription = "";

        callback = new CallbackFunc(defaultCallback);
    }

    public MoveCallback(CallbackFunc call)
    {
        callback = call;
    }

    public void defaultCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        
    }
}
