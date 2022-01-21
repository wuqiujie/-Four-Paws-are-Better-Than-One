using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScreenController : MonoBehaviour
{
    public Image bossImage;

    public Sprite happyBoss;
    public Sprite sadBoss;
    public Sprite angryBoss;
    public Sprite ooohBoss;

    [Header("SFX")]
    public AudioClip happySound;
    public AudioClip angrySound;
    public AudioClip sadSound;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MakeBossAngry()
    {
        bossImage.sprite = angryBoss;
        audioSource.clip = angrySound;
        audioSource.Play();
    }

    public void MakeBossSad()
    {
        bossImage.sprite = sadBoss;
        audioSource.clip = sadSound;
        audioSource.Play();
    }

    public void MakeBossOooh()
    {
        bossImage.sprite = ooohBoss;
        audioSource.clip = happySound;
        audioSource.Play();
    }

    public void MakeBossHappy()
    {
        bossImage.sprite = happyBoss;
    }
}
