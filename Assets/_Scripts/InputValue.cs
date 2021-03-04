using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InputValue : MonoBehaviour
{
    public static Difficulty gameDifficulty;

    public TextMeshProUGUI difficultyText;

    public static PlayerSkill playerSkill;

    public TextMeshProUGUI playerSkillText;

    public GameObject message;

    public GameObject announcement;

    // Start is called before the first frame update
    void Start()
    {
        gameDifficulty = Difficulty.NONE;

        playerSkill = PlayerSkill.NONE;

        StartCoroutine(HideMessage(3.0f, announcement));
    }

    // Update is called once per frame
    void Update()
    {
        difficultyText.text = Enum.GetName(typeof(Difficulty), (int)gameDifficulty);

        playerSkillText.text = Enum.GetName(typeof(PlayerSkill), (int)playerSkill);
    }

    public void SkillRandomize()
    {
        int count = Enum.GetValues(typeof(Difficulty)).Length;
        int level = UnityEngine.Random.Range(1, count);

        playerSkill = (PlayerSkill)level;
        Debug.Log(playerSkill.ToString());
    }


    public void EasySelected()
    {
        gameDifficulty = Difficulty.EASY;
    }


    public void MediumSelected()
    {
        gameDifficulty = Difficulty.MEDIUM;
    }


    public void HardSelected()
    {
        gameDifficulty = Difficulty.HARD;
    }


    public void OnLockClicked()
    {
        if(gameDifficulty != Difficulty.NONE && playerSkill != PlayerSkill.NONE)
        {
            SceneManager.LoadScene("GamePlayScene");
        }
        else
        {
            message.gameObject.SetActive(true);
            StartCoroutine(HideMessage(2.0f, message));
        }
    }

    IEnumerator HideMessage(float delay, GameObject message)
    {
        yield return new WaitForSeconds(delay);
        message.gameObject.SetActive(false);
    }
}
