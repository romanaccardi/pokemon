
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    START,
    WAITING_FOR_INPUT,
    INPUT_RECEIVED,
    EXECUTING_PLAYER_MOVE,
    EXECUTING_ENEMY_MOVE,
    FINISHING_TURN_EXECUTION,
    VICTORY,
    LAST_CHANCE,
    POKE_BALL
}

public enum PlayerInput
{
    MOVE_0 = 0,
    MOVE_1 = 1,
    MOVE_2 = 2,
    MOVE_3 = 3,
    SWITCH_ACTIVE_POKEMON = 4,
    POKEBALL = 5
}

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameState gameState;

    /// <summary>
    /// Attributes
    /// </summary>
    public static Attribute normal;
    public static Attribute fire;
    public static Attribute water;
    public static Attribute electric;
    public static Attribute grass;
    public static Attribute ice;
    public static Attribute fighting;
    public static Attribute poison;
    public static Attribute ground;
    public static Attribute flying;
    public static Attribute psychic;
    public static Attribute bug;
    public static Attribute rock;
    public static Attribute ghost;
    public static Attribute dragon;
    public static Attribute dark;
    public static Attribute steel;
    public static Attribute fairy;

    /// <summary>
    /// Pokemon
    /// </summary>
    public static Pokemon enemyPokemon;
    public List<Pokemon> friendlyPokemon;

    // Okay, let's just store all of the sprites here
    public Sprite fluffy5Sprite;
    public Sprite fluffy5FrontSprite;
    public Sprite fluffy4Sprite;
    public Sprite fluffy4FrontSprite;
    public Sprite oatmealSprite;
    public Sprite oatmealFrontSprite;
    public Sprite ladyEdelmarchSprite;
    public Sprite ladyEdelmarchFrontSprite;
    public Sprite timlexSprite;
    public Sprite timlexFrontSprite;
    public Sprite bernieSprite;
    public Sprite bernieFrontSprite;

    public Sprite robertaSprite;

    // miscellany
    public int turnCount;
    public GameObject activePokemonGameObject;
    public GameObject enemyPokemonGameObject;
    public PlayerInput queuedPlayerAction;
    public int friendlyPokemonSavedSpeed;
    public int enemyPokemonSavedSpeed;
    public int currentPokemonIndex;
    public bool previousPokemonFainted;

    // Animation stuff
    public GameObject menuControllerGameObject;

    void Awake()
    {
        instance = this;
        turnCount = 0;

        friendlyPokemon = new List<Pokemon>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize all of the attributes
        initAttributes();

        // Initialize all of the moves
        Movelist.initMoves();

        // Initialize the Pokemon
        roberta();
        PokemonList.initPokemon();

        EnemyPokemon.instance.loadEnemyPokemon(enemyPokemon);

        // Set the game state
        gameState = GameState.START;
        // For all intents and purposes, the previous pokemon fainted
        this.previousPokemonFainted = true;

        DialogueManager.instance.addToQueue("A wild " + getEnemyPokemon().name + " appeared!");
    }

    private bool friendlyPokemonIsFaster()
    {
        return friendlyPokemonSavedSpeed >= enemyPokemonSavedSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Don't execute any state changes if there is still queued dialogue
        // This prevents turns from overlapping on each other
        if( !DialogueManager.instance.isEmpty() )
        {
            return;
        }

        // If we're waiting on buffering to finish, just skip everything
        switch( gameState )
        {
            case GameState.START:
                menuControllerGameObject.GetComponent<MenuController>().showOnStartup();
                this.gameState = GameState.WAITING_FOR_INPUT;
                break;
            case GameState.FINISHING_TURN_EXECUTION:
                // If the enemy pokemon is unconscious, we've already moved to the victory scene
                // Otherwise, open up the menu and then change to waiting for input
                if(checkLost())
                {
                    gameState = GameState.LAST_CHANCE;
                    return;
                }
                if(!consciousnessCheck())
                {
                    menuControllerGameObject.GetComponent<MenuController>().showOnStartup();
                }

                // can just switch to waiting for input; if we've gotten here,
                // then we know that the dialogue queue is empty
                ++this.turnCount;

                // Change type every 6 turns
                if(this.turnCount % 6 == 0 && this.turnCount != 0)
                {
                    DialogueManager.instance.addToQueue("Life is finding a way...");
                    this.enemyPokemonGameObject.GetComponent<EnemyPokemon>().chooseNewAttribute();
                }
                gameState = GameState.WAITING_FOR_INPUT;
                break;
            case GameState.INPUT_RECEIVED:
                // save the speeds of the respective pokemon if there have been any turns before now
                if(this.turnCount > 0)
                {
                    this.enemyPokemonSavedSpeed = enemyPokemon.statBlock.current_spe;
                    this.friendlyPokemonSavedSpeed = getActivePokemon().statBlock.current_spe;
                }

                switch (queuedPlayerAction)
                {
                    case PlayerInput.MOVE_0:
                    case PlayerInput.MOVE_1:
                    case PlayerInput.MOVE_2:
                    case PlayerInput.MOVE_3:
                        // check which pokemon is faster and change the
                        // game state accordingly
                        if (friendlyPokemonIsFaster())
                        {
                            gameState = GameState.EXECUTING_PLAYER_MOVE;
                        }
                        else
                        {
                            gameState = GameState.EXECUTING_ENEMY_MOVE;
                        }

                        break;
                    case PlayerInput.SWITCH_ACTIVE_POKEMON:
                        // If the current Pokemon is still conscious, then say come back
                        if( getActivePokemon() != null && getActivePokemon().conscious )
                        {
                            DialogueManager.instance.addToQueue(getActivePokemon().name + ", come back!");
                            // TODO - animate the Pokemon coming back
                        }

                        ActivePokemon.instance.switchActivePokemon(this.friendlyPokemon[currentPokemonIndex]);

                        // Now, unless the previous pokemon fainted, execute enemy turn
                        if(!this.previousPokemonFainted)
                        {
                            this.gameState = GameState.EXECUTING_ENEMY_MOVE;
                        }
                        else
                        {
                            this.previousPokemonFainted = false;
                            this.gameState = GameState.WAITING_FOR_INPUT;
                        }
                        break; // TODO
                    case PlayerInput.POKEBALL:
                        break; // TODO
                    default:
                        break;
                }
                // Increment turn count
                ++this.turnCount;
                break;
            case GameState.EXECUTING_PLAYER_MOVE:
                // execute the player move
                playerMove();

                // ensure that both parties are still conscious
                // before proceeding
                if( !consciousnessCheck() )
                {
                    if (getEnemyPokemon().conscious)
                    {
                        gameState = GameState.FINISHING_TURN_EXECUTION;
                    }
                    return;
                }

                // if the player is going first, then switch the game state to the enemy move
                // otherwise, say that we're done with the turn
                if(friendlyPokemonIsFaster())
                {
                    gameState = GameState.EXECUTING_ENEMY_MOVE;
                }
                else
                {
                    gameState = GameState.FINISHING_TURN_EXECUTION;
                }
                break;
            case GameState.EXECUTING_ENEMY_MOVE:
                // execute the enemy move
                enemyMove();

                // ensure that both parties are still conscious
                // before proceeding
                if (!consciousnessCheck())
                {
                    if(getEnemyPokemon().conscious)
                    {
                        gameState = GameState.FINISHING_TURN_EXECUTION;
                    }
                    return;
                }

                // if the player is slower, that means that they haven't already gone;
                // so let them go
                // however, also check to make sure that their queued input was a move
                // this check is ugly, but basically okay since I'm depending on the
                // numerical values of the enums elsewhere
                if ((int)queuedPlayerAction <= (int)PlayerInput.MOVE_3 && !friendlyPokemonIsFaster())
                {
                    gameState = GameState.EXECUTING_PLAYER_MOVE;
                }
                else
                {
                    gameState = GameState.FINISHING_TURN_EXECUTION;
                }
                break;
            case GameState.VICTORY:
                SceneManager.LoadScene(sceneName: "Victory");
                break;
            case GameState.LAST_CHANCE:
                // TODO - transition to last chance scene
                SaveHP.instance.current_hp = this.getEnemyPokemon().statBlock.current_hp;
                SaveHP.instance.base_hp = this.getEnemyPokemon().statBlock.base_hp;
                SceneManager.LoadScene(sceneName: "LastChance");
                gameState = GameState.WAITING_FOR_INPUT;
                break;
            case GameState.POKE_BALL:
            case GameState.WAITING_FOR_INPUT:
            default:
                break;
        }
    }

    public void throwPokeBall()
    {
        queuedPlayerAction = PlayerInput.POKEBALL;
        gameState = GameState.INPUT_RECEIVED;
    }

    public bool checkLost()
    {

        bool lost = true;
        foreach (Pokemon friendly in this.friendlyPokemon)
        {
            if (friendly.conscious)
            {
                lost = false;
            }
        }
        return lost;
    }

    public bool consciousnessCheck()
    {
        // If the enemy pokemon is unconscious, congratulations, you won!
        if( !getEnemyPokemon().conscious )
        {
            Debug.Log("here");
            // TODO - play animation
            gameState = GameState.VICTORY;
            DialogueManager.instance.addToQueue(getEnemyPokemon().name + " fainted!");
            return false;
        }
        if( !getActivePokemon().conscious )
        {
            // clarify that the previous pokemon fainted so that Roberta doesn't attack immediately
            this.previousPokemonFainted = true;
            gameState = GameState.FINISHING_TURN_EXECUTION;
            DialogueManager.instance.addToQueue(getActivePokemon().name + " fainted!");
            // TODO - probably play animation
            return false;
        }
        return true;
    }

    public void switchToPokemon(int pokeNumber)
    {
        this.gameState           = GameState.INPUT_RECEIVED;
        this.queuedPlayerAction  = PlayerInput.SWITCH_ACTIVE_POKEMON;
        this.currentPokemonIndex = pokeNumber;
    }

    private void initAttributes()
    {
        normal = new Attribute(TypeSpecifier.NORMAL);
        fire = new Attribute(TypeSpecifier.FIRE);
        water = new Attribute(TypeSpecifier.WATER);
        electric = new Attribute(TypeSpecifier.ELECTRIC);
        grass = new Attribute(TypeSpecifier.GRASS);
        ice = new Attribute(TypeSpecifier.ICE);
        fighting = new Attribute(TypeSpecifier.FIGHTING);
        poison = new Attribute(TypeSpecifier.POISON);
        ground = new Attribute(TypeSpecifier.GROUND);
        flying = new Attribute(TypeSpecifier.FLYING);
        psychic = new Attribute(TypeSpecifier.PSYCHIC);
        bug = new Attribute(TypeSpecifier.BUG);
        rock = new Attribute(TypeSpecifier.ROCK);
        ghost = new Attribute(TypeSpecifier.GHOST);
        dragon = new Attribute(TypeSpecifier.DRAGON);
        dark = new Attribute(TypeSpecifier.DARK);
        steel = new Attribute(TypeSpecifier.STEEL);
        fairy = new Attribute(TypeSpecifier.FAIRY);

        //////////////////////////////////////////////
        //        Type strengths/weaknesses         //
        //////////////////////////////////////////////

        // Normal
        normal.weakTo(fighting);

        normal.immuneTo(ghost);

        // fire
        fire.weakTo(water);
        fire.weakTo(ground);
        fire.weakTo(rock);

        fire.resistantTo(fire);
        fire.resistantTo(grass);
        fire.resistantTo(ice);
        fire.resistantTo(bug);
        fire.resistantTo(steel);
        fire.resistantTo(fairy);

        // water
        water.weakTo(electric);
        water.weakTo(grass);

        water.resistantTo(fire);
        water.resistantTo(water);
        water.resistantTo(ice);
        water.resistantTo(steel);

        // electric
        electric.weakTo(ground);

        electric.resistantTo(electric);
        electric.resistantTo(flying);
        electric.resistantTo(steel);

        // grass
        grass.weakTo(fire);
        grass.weakTo(ice);
        grass.weakTo(poison);
        grass.weakTo(flying);
        grass.weakTo(bug);

        grass.resistantTo(water);
        grass.resistantTo(electric);
        grass.resistantTo(grass);
        grass.resistantTo(ground);

        // ice
        ice.weakTo(fire);
        ice.weakTo(fighting);
        ice.weakTo(rock);
        ice.weakTo(steel);

        ice.resistantTo(ice);

        // fighting
        fighting.weakTo(flying);
        fighting.weakTo(psychic);
        fighting.weakTo(fairy);

        fighting.resistantTo(bug);
        fighting.resistantTo(rock);
        fighting.resistantTo(dark);

        // poison
        poison.weakTo(ground);
        poison.weakTo(psychic);

        poison.resistantTo(grass);
        poison.resistantTo(fighting);
        poison.resistantTo(poison);
        poison.resistantTo(bug);

        // ground
        ground.weakTo(water);
        ground.weakTo(grass);
        ground.weakTo(ice);

        ground.resistantTo(poison);
        ground.resistantTo(rock);

        ground.immuneTo(electric);

        // flying
        flying.weakTo(electric);
        flying.weakTo(ice);
        flying.weakTo(rock);

        flying.resistantTo(grass);
        flying.resistantTo(fighting);
        flying.resistantTo(bug);

        flying.immuneTo(ground);

        // psychic
        psychic.weakTo(bug);
        psychic.weakTo(ghost);
        psychic.weakTo(dark);

        psychic.resistantTo(fighting);
        psychic.resistantTo(psychic);

        // bug
        bug.weakTo(fire);
        bug.weakTo(flying);
        bug.weakTo(rock);

        bug.resistantTo(grass);
        bug.resistantTo(fighting);
        bug.resistantTo(ground);

        // rock
        rock.weakTo(water);
        rock.weakTo(grass);
        rock.weakTo(fighting);
        rock.weakTo(ground);
        rock.weakTo(steel);

        rock.resistantTo(normal);
        rock.resistantTo(fire);
        rock.resistantTo(poison);
        rock.resistantTo(flying);

        // ghost
        ghost.weakTo(ghost);
        ghost.weakTo(dark);

        ghost.resistantTo(poison);
        ghost.resistantTo(bug);

        ghost.immuneTo(normal);
        ghost.immuneTo(fighting);

        // dragon
        dragon.weakTo(ice);
        dragon.weakTo(dragon);
        dragon.weakTo(fairy);

        dragon.resistantTo(fire);
        dragon.resistantTo(water);
        dragon.resistantTo(electric);
        dragon.resistantTo(grass);

        // dark
        dark.weakTo(fighting);
        dark.weakTo(bug);
        dark.weakTo(fairy);

        dark.resistantTo(ghost);
        dark.resistantTo(dark);

        dark.immuneTo(psychic);

        // steel
        steel.weakTo(fire);
        steel.weakTo(fighting);
        steel.weakTo(ground);

        steel.resistantTo(normal);
        steel.resistantTo(grass);
        steel.resistantTo(ice);
        steel.resistantTo(flying);
        steel.resistantTo(psychic);
        steel.resistantTo(bug);
        steel.resistantTo(rock);
        steel.resistantTo(dragon);
        steel.resistantTo(steel);
        steel.resistantTo(fairy);

        steel.immuneTo(poison);

        // fairy
        fairy.weakTo(poison);
        fairy.weakTo(steel);

        fairy.resistantTo(fighting);
        fairy.resistantTo(bug);
        fairy.resistantTo(dark);

        fairy.immuneTo(dragon);
    }

    private void roberta()
    {
        // Initialize enemy pokemon
        int hp      = 1000;
        int atk     = 120; // TODO
        int def     = 100;
        int spAtk   = 120;
        int spDef   = 100;
        int spe     = 80;
        StatBlock statBlock = new StatBlock(hp, atk, def, spAtk, spDef, spe);
        enemyPokemon = new Pokemon("DIRE T-REX", statBlock);
        EnemyPokemon.instance.loadEnemyPokemon(enemyPokemon);

        enemyPokemon.sprite = this.robertaSprite;
        enemyPokemon.types.Add(rock);

        enemyPokemon.moves.Add(Movelist.bite);
        enemyPokemon.moves.Add(Movelist.stomp);
        enemyPokemon.moves.Add(Movelist.meteor);
        enemyPokemon.moves.Add(Movelist.amber);
        //enemyPokemon.moves.Add(Movelist.earthquake);

        // utility moves
        //enemyPokemon.moves.Add(Movelist.lifeFindsAWay);
        //enemyPokemon.moves.Add(Movelist.mustGoFaster);
        //enemyPokemon.moves.Add(Movelist.cleverGirl);
        //enemyPokemon.moves.Add(Movelist.closerThanTheyAppear);
    }

    private void playerMove()
    {
        Pokemon activePokemon = getActivePokemon();
        ActivePokemon.instance.executeMoveAt((int)queuedPlayerAction);
    }

    public bool canCloseMenu()
    {
        return getActivePokemon() != null && getActivePokemon().conscious;
    }

    public bool canOpenMenu()
    {
        return gameState == GameState.WAITING_FOR_INPUT;
    }

    private void enemyMove()
    {
        EnemyPokemon.instance.executeMove();
    }

    public Pokemon getEnemyPokemon()
    {
        return enemyPokemon;
    }

    public Pokemon getActivePokemon()
    {
        return activePokemonGameObject.GetComponent<ActivePokemon>().pokemon;
    }
}