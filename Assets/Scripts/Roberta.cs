using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roberta : MonoBehaviour
{
    public Sprite pokeball;
    public Animator animator;
    public Texture2D pokeball_tex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onPress()
    {
        Invoke("seeIfItWorked", 3);
    }

    void seeIfItWorked()
    {
        Invoke("goToVictory", 3);
    }

    void goToVictory()
    {

    }

    void goToDefeat()
    {

    }
}
