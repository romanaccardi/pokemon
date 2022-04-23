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
    public static Move fireFang;
    public static Move chainsaw;
    public static Move tailWhip;

    // Fluffy V

    // Oatmeal

    // Lady Edelmarch
    public static Move twilightAxe;
    public static Move rest;
    public static Move rage;
    public static Move punch;

    // Bernie

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
        fireFang = new Move("FIRE FANG");
        fireFang.attribute = GameController.fire;
        fireFang.power = 65;
        fireFang.special = false;
        fireFang.accuracy = 0.95f;
        fireFang.callback = new MoveCallback(chanceToBurn);

        // Chainsaw
        chainsaw = new Move("CHAINSAW");
        chainsaw.attribute = GameController.electric;
        chainsaw.power = 140;
        chainsaw.special = false;
        chainsaw.accuracy = 0.6f;

        // Tail Whip
        tailWhip = new Move("TAIL WHIP");
        tailWhip.attribute = GameController.normal;
        tailWhip.power = 0;
        tailWhip.special = true;
        tailWhip.callback = new MoveCallback(lowerDefense1);

        ///
        /// FLUFFY V
        /// 

        ///
        /// OATMEAL
        /// 

        ///
        /// LADY EDELMARCH
        /// 
        // Twilight axe
        twilightAxe = new Move("TWILIGHT AXE");
        twilightAxe.attribute = GameController.ghost;
        twilightAxe.power = 150;
        twilightAxe.special = false;
        twilightAxe.accuracy = 0.5f;

        // Rest
        rest = new Move("REST");
        rest.attribute = GameController.normal;
        rest.power = 0;
        rest.special = true;
        rest.callback = new MoveCallback(restCallback);

        // Rage
        rage = new Move("RAGE");
        rage.attribute = GameController.fighting;
        rage.power = 0;
        rage.special = true;
        rage.callback = new MoveCallback(rageCallback);

        // Punch
        punch = new Move("PUNCH");
        punch.attribute = GameController.fighting;
        punch.power = 70;
        punch.special = false;

        ///
        /// BERNIE
        /// 

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

    public static void lowerDefense1(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        defender.statBlock.changeStage(Stat.DEFENSE, -1);
        DialogueManager.instance.addToQueue(defender.name + "'s DEFENSE fell!");
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
        DialogueManager.instance.addToQueue(attacker.name + " ended all status effects/debuffs!");
    }

    public static void meteorCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.changeStage(Stat.SPECIAL_ATTACK, -2);
        DialogueManager.instance.addToQueue(attacker.name + "'s SP. ATTACK sharply fell!");
    }

    public static void restCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.clearStatuses();
        attacker.addStatusEffect(Status.ASLEEP);
        attacker.statBlock.accuracy_stage = 0;
        attacker.statBlock.current_hp = attacker.statBlock.base_hp;
        DialogueManager.instance.addToQueue(attacker.name + " fell asleep!");
    }

    public static void rageCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.confused = true;
        attacker.statBlock.changeStage(Stat.ATTACK, 3);
        attacker.statBlock.changeStage(Stat.DEFENSE, 1);
        DialogueManager.instance.addToQueue(attacker.name + "'s ATTACK drastically rose!");
        DialogueManager.instance.addToQueue(attacker.name + "'s DEFENSE rose!");
        DialogueManager.instance.addToQueue(attacker.name + " became confused!");
    }

}
