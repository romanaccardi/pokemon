using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movelist : MonoBehaviour
{
    void Awake()
    {
        initMoves();
    }

    public static Move tackle;
    public static Move stomp;
    public static Move splash;
    public static Move bite;

    // TimLex
    public static Move edictStrike;
    public static Move enlarge;
    public static Move spiderClimb;
    public static Move edictProtect;

    // Fluffy IV

    // Roberta
    public static Move mustGoFaster;
    public static Move lifeFindsAWay;
    public static Move cleverGirl;
    public static Move meteor;
    public static Move closerThanTheyAppear;

    public static void initMoves()
    {
        ///
        /// SHARED
        ///
        // Tackle
        tackle = new Move("TACKLE");
        tackle.attribute = GameController.normal;
        tackle.power = 40;
        tackle.special = false;

        // Stomp
        stomp = new Move("STOMP");
        stomp.attribute = GameController.normal;
        stomp.power = 65;
        stomp.special = false;

        // Bite
        bite = new Move("BITE");
        bite.attribute = GameController.dark;
        bite.power = 60;
        bite.special = false;
        bite.callback = new MoveCallback(chanceToFlinch);

        // Splash
        splash = new Move("SPLASH");
        splash.attribute = GameController.water;
        splash.power = 0;
        splash.special = true;

        ///
        /// TIMLEX MOVES
        ///
        // Edict: Strike
        edictStrike = new Move("EDICT STRIKE");
        edictStrike.attribute = GameController.steel;
        edictStrike.power = 60;
        edictStrike.special = true;

        // Enlarge
        enlarge = new Move("ENLARGE");
        enlarge.attribute = GameController.psychic;
        enlarge.power = 0;
        enlarge.special = true;
        enlarge.callback = new MoveCallback(enlargeCallback);

        // Spider-Climb
        spiderClimb = new Move("SPIDER CLIMB");
        spiderClimb.attribute = GameController.bug;
        spiderClimb.power = 0;
        spiderClimb.special = true;
        spiderClimb.callback = new MoveCallback(spiderClimbCallback);

        // Edict: Protect
        edictProtect = new Move("EDICT PROTECT");
        edictProtect.attribute = GameController.psychic;
        edictProtect.power = 0;
        edictProtect.special = true;
        edictProtect.callback = new MoveCallback(raiseDefense1);

        ///
        /// FLUFFY IV
        /// 

        // Bite

        // Fire Fang


        ///
        /// ROBERTA
        /// 
        // Meteor
        meteor = new Move("METEOR");
        meteor.attribute = GameController.dragon;
        meteor.power = 130;
        meteor.special = true;
        meteor.accuracy = 0.9f;
        meteor.executeCallbackEvenIfEnemyFainted = true;
        meteor.callback = new MoveCallback(meteorCallback);

        // Must Go Faster
        mustGoFaster = new Move("MUST GO FASTER");
        mustGoFaster.attribute = GameController.ground;
        mustGoFaster.power = 0;
        mustGoFaster.special = true;
        mustGoFaster.callback = new MoveCallback(mustGoFasterCallback);

        // Life Finds a Way
        lifeFindsAWay = new Move("LIFE FINDS A WAY");
        lifeFindsAWay.attribute = GameController.bug;
        lifeFindsAWay.power = 0;
        lifeFindsAWay.special = true;
        lifeFindsAWay.callback = new MoveCallback(lifeFindsAWayCallback);

        // Clever Girl
        cleverGirl = new Move("CLEVER GIRL");
        cleverGirl.attribute = GameController.psychic;
        cleverGirl.power = 0;
        cleverGirl.special = true;
        cleverGirl.callback = new MoveCallback(cleverGirlCallback);

        // Closer than they appear
        closerThanTheyAppear = new Move("CLOSER THAN THEY APPEAR");
        closerThanTheyAppear.attribute = GameController.ground;
        closerThanTheyAppear.power = 0;
        closerThanTheyAppear.special = true;
        closerThanTheyAppear.callback = new MoveCallback(lowerEvade2);

    }

    ///
    /// Reusable callbacks
    ///
    public static void chanceToFlinch(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        // 10% chance
        if (Random.Range(0.0f, 1.0f) < 0.1f)
        {
            defender.flinch = true;
        }
    }

    public static void raiseDefense1(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.changeStage(Stat.DEFENSE, 1);
        DialogueManager.instance.addToQueue(attacker.name + "'s DEFENSE rose!");
    }

    public static void lowerEvade2(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        defender.statBlock.changeStage(Stat.EVASION, -2);
        DialogueManager.instance.addToQueue(defender.name + "'s EVADE sharply fell!");
    }

    public static bool chanceToApplyStatus(Pokemon defender, Status status, float chance)
    {
        if (Random.Range(0.0f, 1.0f) < chance)
        {
            return defender.addStatusEffect(status);
        }
        return false;
    }

    public static void chanceToBurn(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        // 10% chance
        if (chanceToApplyStatus(defender, Status.BURNED, 0.1f))
        { 
            DialogueManager.instance.addToQueue(defender.name + " was burned!");
        }
    }

    ///
    /// Specific callbacks
    ///
    public static void enlargeCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.changeStage(Stat.ATTACK, 1);
        DialogueManager.instance.addToQueue(attacker.name + "'s ATTACK rose!");
    }
    
    public static void spiderClimbCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.changeStage(Stat.EVASION, 1);
        DialogueManager.instance.addToQueue(attacker.name + "'s evasion rose!");
    }

    public static void mustGoFasterCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.changeStage(Stat.SPEED, 2);
        DialogueManager.instance.addToQueue(attacker.name + "'s SPEED sharply rose!");
    }

    public static void lifeFindsAWayCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.current_hp = 
            (attacker.statBlock.current_hp + 150 > attacker.statBlock.base_hp
            ? attacker.statBlock.base_hp
            : attacker.statBlock.current_hp + 150);
        DialogueManager.instance.addToQueue(attacker.name + " regained health!");
    }

    public static void cleverGirlCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.resetStatsExceptHP();
        DialogueManager.instance.addToQueue(attacker.name + " ended all status effects and debuffs on her!");
    }

    public static void meteorCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.changeStage(Stat.SPECIAL_ATTACK, -2);
        DialogueManager.instance.addToQueue(attacker.name + "'s SP. ATTACK sharply fell!");
    }

    
}
