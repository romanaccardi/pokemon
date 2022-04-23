using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonList : MonoBehaviour
{
    public static void initPokemon()
    {
        // TODO
        // FLUFFY V
        Pokemon fluffy5 = new Pokemon("FLUFFY V");

        fluffy5.types.Add(GameController.rock);

        fluffy5.moves.Add(Movelist.tackle);

        fluffy5.sprite          = GameController.instance.fluffy5Sprite;
        fluffy5.frontSprite     = GameController.instance.fluffy5FrontSprite;
        fluffy5.spriteScale     = new Vector3(2.5f, 2.5f, 0);

        // FLUFFY IV
        Pokemon fluffy4 = new Pokemon("FLUFFY IV");

        fluffy4.types.Add(GameController.dark);

        fluffy4.moves.Add(Movelist.tackle);

        fluffy4.sprite          = GameController.instance.fluffy4Sprite;
        fluffy4.frontSprite     = GameController.instance.fluffy4FrontSprite;
        fluffy4.spriteScale     = new Vector3(5.0f, 5.0f, 0);

        // OATMEAL
        Pokemon oatmeal = new Pokemon("OATMEAL");

        oatmeal.types.Add(GameController.dragon);
        oatmeal.types.Add(GameController.fairy);

        oatmeal.moves.Add(Movelist.tackle);

        oatmeal.sprite          = GameController.instance.oatmealSprite;
        oatmeal.frontSprite     = GameController.instance.oatmealFrontSprite;
        oatmeal.spriteScale     = new Vector3(5.0f, 5.0f, 0);

        // LADY EDELMARCH
        Pokemon ladyEdelmarch = new Pokemon("LADY EDELMARCH");

        ladyEdelmarch.types.Add(GameController.normal);
        ladyEdelmarch.types.Add(GameController.fighting);

        ladyEdelmarch.moves.Add(Movelist.tackle);
        
        ladyEdelmarch.sprite        = GameController.instance.ladyEdelmarchSprite;
        ladyEdelmarch.frontSprite   = GameController.instance.ladyEdelmarchFrontSprite;
        ladyEdelmarch.spriteScale   = new Vector3(1.0f, 1.0f, 0);

        // TIMLEX
        Pokemon timlex = new Pokemon("TIMLEX");

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
        Pokemon bernie = new Pokemon("BERNIE");

        bernie.types.Add(GameController.steel);

        bernie.moves.Add(Movelist.tackle);

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
