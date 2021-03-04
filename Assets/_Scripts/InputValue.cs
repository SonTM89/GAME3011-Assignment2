using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InputValue : MonoBehaviour
{
    [SerializeField] private GameObject gameUI;

    public static Difficulty gameDifficulty;

    [SerializeField] private TextMeshProUGUI difficultyText;

    public static PlayerSkill playerSkill;

    [SerializeField] private TextMeshProUGUI playerSkillText;

    [SerializeField] private GameObject message;

    [SerializeField] private GameObject announcement;

    private bool showAnnouncement;

    // Start is called before the first frame update
    void Start()
    {
        showAnnouncement = false;

        gameDifficulty = Difficulty.NONE;

        playerSkill = PlayerSkill.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        difficultyText.text = Enum.GetName(typeof(Difficulty), (int)gameDifficulty);

        playerSkillText.text = Enum.GetName(typeof(PlayerSkill), (int)playerSkill);

        if(gameUI.activeInHierarchy)
        {
            if(showAnnouncement == false)
            {
                announcement.gameObject.SetActive(true);
                StartCoroutine(HideMessage(3.0f, announcement));

                showAnnouncement = true;
            }
        }
        else
        {
            showAnnouncement = false;
        }
    }


    public void StartGame()
    {
        gameUI.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        gameUI.gameObject.SetActive(false);
        Application.Quit();
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
