using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pokeball : MonoBehaviour
{
    private Sprite pokeball;
    public SpriteRenderer sr;
    public Texture2D pokeball_tex;
    public Animator animator;
    public Text text;
    private static string victory = "Congratulations!  Roberta was caught!";

    void Awake()
    {
        sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);

    }

    // Start is called before the first frame update
    void Start()
    {
        pokeball = Sprite.Create(pokeball_tex, new Rect(0.0f, 0.0f, pokeball_tex.width, pokeball_tex.height), new Vector2(0.5f, 0.5f), 200.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void capture()
    {
        StartCoroutine("capturing");
    }

    public IEnumerator capturing()
    {
        yield return new WaitForSeconds(1.5f);
        sr.sprite = pokeball;
        // shake 3 times
        for(int i = 0; i < 3; ++i)
        {
            // TODO - play animation
            animator.SetTrigger("shake");
            yield return new WaitForSeconds(1.0f);
        }

        float allTheMarbles = Random.Range(0.0f, 1.0f);
        float captureChance = SaveHP.instance.chanceToCapture();

        if(allTheMarbles < captureChance)
        {
            text.text = victory;
            Invoke("goToVictory", 3);
        }
        else
        {
            goToDefeat();
        }
    }

    void goToVictory()
    {
        SceneManager.LoadScene(sceneName: "Victory");
    }

    void goToDefeat()
    {
        SceneManager.LoadScene(sceneName: "Defeat");
    }
}
