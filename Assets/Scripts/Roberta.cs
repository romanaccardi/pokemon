using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Roberta : MonoBehaviour
{
    public Animator animator;
    public Text text;
    public SpriteRenderer spriteRenderer;
    public static bool success = false;

    public GameObject pokeball;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPress()
    {
        animator.SetBool("capturing", true);
        pokeball.GetComponent<Pokeball>().capture();
    }
}
