using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //public Button poke0;
    //public Button poke1;
    //public Button poke2;
    //public Button poke3;
    //public Button poke4;
    //public Button poke5;

    public Animator menuAnimator;

    private enum GameObjectIndices
    {
        NAME = 0,
        MOVES = 1,
        IMAGE = 2,
        ALLY_HEALTH_ACTUAL_BAR = 3
    }

    public List<Button> buttons;

    public void Awake()
    {

    }

    public void Start()
    {

    }

    public void Update()
    {

    }

    public void showOnStartup()
    {
        ShowPopup(menuAnimator);
    }

    public void ShowPopup(Animator animator)
    {
        updateButtons();
        animator.SetBool("visible", true);

        // Stop all of the dialogue in the queue
        DialogueManager.instance.pauseDialogue();
    }

    public void HidePopup(Animator animator)
    {
        animator.SetBool("visible", false);

        // Start the dialogue queue again
        DialogueManager.instance.startDialogue();
    }

    // Called by the close button, which you can't do if your pokemon's fainted, can you?
    public void ClosePopup(Animator animator)
    {
        if (!GameController.instance.canCloseMenu())
        {
            return;
        }

        HidePopup(animator);
    }

    public void OpenPopup(Animator animator)
    {
        if (!GameController.instance.canOpenMenu())
        {
            return;
        }

        ShowPopup(animator);
    }

    public void switchPokemonTo(int pokeNumber, Animator animator)
    {
        if( pokeNumber >= GameController.instance.friendlyPokemon.Count )
        {
            return;
        }
        Pokemon correspondingPokemon = GameController.instance.friendlyPokemon[pokeNumber];

        // Check to make sure that the pokemon that you're trying to switch it to
        // is conscious and not the active pokemon
        if( !correspondingPokemon.conscious || correspondingPokemon == GameController.instance.getActivePokemon() )
        {
            return;
        }

        // If all of those have been checked, hide the popup,
        // save the pokeNumber, and change the game state
        GameController.instance.switchToPokemon(pokeNumber);

        HidePopup(animator);

        updateButtons();
    }

    // This was nobody's first choice
    public void switchPokemon0(Animator animator)
    {
        switchPokemonTo(0, animator);
    }
    public void switchPokemon1(Animator animator)
    {
        switchPokemonTo(1, animator);
    }
    public void switchPokemon2(Animator animator)
    {
        switchPokemonTo(2, animator);
    }
    public void switchPokemon3(Animator animator)
    {
        switchPokemonTo(3, animator);
    }
    public void switchPokemon4(Animator animator)
    {
        switchPokemonTo(4, animator);
    }
    public void switchPokemon5(Animator animator)
    {
        switchPokemonTo(5, animator);
    }

    public void updateButtons()
    {
        List<Pokemon> friendlyPokemon = GameController.instance.friendlyPokemon;

        // Retrieve all Pokemon information and update the buttons accordingly
        for (int i = 0; i < buttons.Count && i < friendlyPokemon.Count; ++i)
        {
            // Get the corresponding pokemon
            Pokemon correspondingPokemon = GameController.instance.friendlyPokemon[i];

            // And the corresponding button as a reference
            Button button = buttons[i];

            // Update name
            button.transform.GetChild((int)GameObjectIndices.NAME).GetComponent<UnityEngine.UI.Text>().text = correspondingPokemon.name;

            // Update moveset
            Text moveset = button.transform.GetChild((int)GameObjectIndices.MOVES).GetComponent<UnityEngine.UI.Text>();
            moveset.text = "";
            for(int j = 0; j < correspondingPokemon.moves.Count; ++j)
            {
                moveset.text += correspondingPokemon.moves[j].name + "\n";
            }

            // Update sprite
            Image image = button.transform.GetChild((int)GameObjectIndices.IMAGE).GetComponent<UnityEngine.UI.Image>();
            image.sprite = correspondingPokemon.frontSprite;

            // Update health bar
            Image healthBar = button.transform.GetChild((int)GameObjectIndices.ALLY_HEALTH_ACTUAL_BAR).GetComponent<UnityEngine.UI.Image>();
            healthBar.fillAmount = (float)correspondingPokemon.statBlock.current_hp / correspondingPokemon.statBlock.base_hp;
        }
    }
}
