﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonList : MonoBehaviour
{
    /*
     * TimLex - swords dance abusing special sweeper
     * Fluffy IV - Fast physical sweeper
     * Fluffy V - Bulky physical utility
     * Lady Edelmarch - Bulky physical sweeper
     * Oatmeal - Bulky special sweeper
     * Bernie - Bulky physical sweeper
     */
    public static void initPokemon()
    {
        int hp;
        int atk;
        int def;
        int spAtk;
        int spDef;
        int spe;

        // TODO
        // FLUFFY V
        hp      = 100;
        atk     = 110;
        def     = 100;
        spAtk   = 15;
        spDef   = 120;
        spe     = 90;
        StatBlock fluffyVStatblock = new StatBlock(hp, atk, def, spAtk, spDef, spe);
        Pokemon fluffy5 = new Pokemon("FLUFFY V", fluffyVStatblock);

        fluffy5.types.Add(GameController.rock);

        fluffy5.moves.Add(Movelist.rockTomb);
        fluffy5.moves.Add(Movelist.growl);
        fluffy5.moves.Add(Movelist.packTactics);
        fluffy5.moves.Add(Movelist.dynamax);

        fluffy5.sprite          = GameController.instance.fluffy5Sprite;
        fluffy5.frontSprite     = GameController.instance.fluffy5FrontSprite;
        fluffy5.spriteScale     = new Vector3(2.5f, 2.5f, 0);

        // FLUFFY IV
        hp      = 130;
        atk     = 120;
        def     = 110;
        spAtk   = 15;
        spDef   = 110;
        spe     = 110;
        StatBlock fluffyIVStatblock = new StatBlock(hp, atk, def, spAtk, spDef, spe);
        Pokemon fluffy4 = new Pokemon("FLUFFY IV", fluffyIVStatblock);

        fluffy4.types.Add(GameController.dark);

        fluffy4.moves.Add(Movelist.bite);
        fluffy4.moves.Add(Movelist.fireFang);
        fluffy4.moves.Add(Movelist.chainsaw);
        fluffy4.moves.Add(Movelist.tailWhip);

        fluffy4.sprite          = GameController.instance.fluffy4Sprite;
        fluffy4.frontSprite     = GameController.instance.fluffy4FrontSprite;
        fluffy4.spriteScale     = new Vector3(5.0f, 5.0f, 0);

        // OATMEAL
        hp      = 90;
        atk     = 100;
        def     = 100;
        spAtk   = 120;
        spDef   = 100;
        spe     = 130;
        StatBlock oatmealStatblock = new StatBlock(hp, atk, def, spAtk, spDef, spe);
        Pokemon oatmeal = new Pokemon("OATMEAL", oatmealStatblock);

        oatmeal.types.Add(GameController.dragon);
        oatmeal.types.Add(GameController.fairy);

        oatmeal.moves.Add(Movelist.breathWeapon);
        oatmeal.moves.Add(Movelist.waterPulse);
        oatmeal.moves.Add(Movelist.iceBeam);
        oatmeal.moves.Add(Movelist.hurricane);

        oatmeal.sprite          = GameController.instance.oatmealSprite;
        oatmeal.frontSprite     = GameController.instance.oatmealFrontSprite;
        oatmeal.spriteScale     = new Vector3(5.0f, 5.0f, 0);

        // LADY EDELMARCH
        hp      = 250;
        atk     = 100;
        def     = 95;
        spAtk   = 20;
        spDef   = 40;
        spe     = 65;
        StatBlock edelmarchStatblock = new StatBlock(hp, atk, def, spAtk, spDef, spe);
        Pokemon ladyEdelmarch = new Pokemon("LADY EDELMARCH", edelmarchStatblock);

        ladyEdelmarch.types.Add(GameController.normal);
        ladyEdelmarch.types.Add(GameController.fighting);

        ladyEdelmarch.moves.Add(Movelist.twilightAxe);
        ladyEdelmarch.moves.Add(Movelist.rest);
        ladyEdelmarch.moves.Add(Movelist.rage);
        ladyEdelmarch.moves.Add(Movelist.punch);

        ladyEdelmarch.sprite        = GameController.instance.ladyEdelmarchSprite;
        ladyEdelmarch.frontSprite   = GameController.instance.ladyEdelmarchFrontSprite;
        ladyEdelmarch.spriteScale   = new Vector3(1.0f, 1.0f, 0);

        // TIMLEX
        hp      = 90;
        atk     = 100;
        def     = 110;
        spAtk   = 120;
        spDef   = 110;
        spe     = 110;
        StatBlock timlexStatblock = new StatBlock(hp, atk, def, spAtk, spDef, spe);
        Pokemon timlex = new Pokemon("TIMLEX", timlexStatblock);

        timlex.types.Add(GameController.steel);
        timlex.types.Add(GameController.psychic);

        timlex.moves.Add(Movelist.edictStrike);
        timlex.moves.Add(Movelist.enlarge);
        timlex.moves.Add(Movelist.spiderClimb);
        timlex.moves.Add(Movelist.edictProtect);

        timlex.sprite        = GameController.instance.timlexSprite;
        timlex.frontSprite   = GameController.instance.timlexFrontSprite;
        timlex.spriteScale   = new Vector3(2.5f, 2.5f, 0);

        // BERNIE
        hp      = 150;
        atk     = 80;
        def     = 120;
        spAtk   = 15;
        spDef   = 120;
        spe     = 70;
        StatBlock bernieStatblock = new StatBlock(hp, atk, def, spAtk, spDef, spe);
        Pokemon bernie = new Pokemon("BERNIE", bernieStatblock);

        bernie.types.Add(GameController.steel);

        bernie.moves.Add(Movelist.tackle); // keep this
        bernie.moves.Add(Movelist.sword);
        bernie.moves.Add(Movelist.moralSupport);
        bernie.moves.Add(Movelist.niceGuy);

        bernie.sprite       = GameController.instance.bernieSprite;
        bernie.frontSprite  = GameController.instance.bernieFrontSprite;
        bernie.spriteScale  = new Vector3(2.0f, 2.0f, 0);

        GameController.instance.friendlyPokemon.Add(timlex);
        GameController.instance.friendlyPokemon.Add(ladyEdelmarch);
        GameController.instance.friendlyPokemon.Add(fluffy4);
        GameController.instance.friendlyPokemon.Add(fluffy5);
        GameController.instance.friendlyPokemon.Add(oatmeal);
        GameController.instance.friendlyPokemon.Add(bernie);
    }
}
