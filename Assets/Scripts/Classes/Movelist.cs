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
    public static Move splash;

    // TimLex
    public static Move edictStrike;
    public static Move enlarge;
    public static Move spiderClimb;
    public static Move edictProtect;

    // Roberta
    public static Move mustGoFaster;
    public static Move lifeFindsAWay;

    public static void initMoves()
    {
        ///
        /// DEFAULTS
        ///
        // Tackle
        tackle = new Move("TACKLE");
        tackle.attribute = GameController.normal;
        tackle.power = 20;
        tackle.special = false;

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
        edictStrike.special = false;

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
        edictProtect.callback = new MoveCallback(edictProtectCallback);

        ///
        /// ROBERTA
        /// 
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
    }

    public static void enlargeCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.changeStage(Stat.ATTACK, 1);
        DialogueManager.instance.addToQueue(attacker.name + "'s ATTACK rose!");
    }
    
    public static void spiderClimbCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        // TODO - find some way to raise evasion
    }

    public static void edictProtectCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.changeStage(Stat.DEFENSE, 1);
        DialogueManager.instance.addToQueue(attacker.name + "'s DEFENSE rose!");
    }

    public static void mustGoFasterCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.changeStage(Stat.SPEED, 2);
        DialogueManager.instance.addToQueue(attacker.name + "'s DEFENSE sharply rose!");
    }

    public static void lifeFindsAWayCallback(Pokemon attacker, Pokemon defender, int damageDealt)
    {
        attacker.statBlock.current_hp = 
            (attacker.statBlock.current_hp + 150 > attacker.statBlock.base_hp
            ? attacker.statBlock.base_hp
            : attacker.statBlock.current_hp + 150);
        DialogueManager.instance.addToQueue(attacker.name + "'s DEFENSE sharply rose!");
    }
}
