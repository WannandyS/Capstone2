using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource attack1Audio;
    public AudioSource enemyAttackAudio;
    public AudioSource attack2Audio;
    public AudioSource attack3Audio;
    public AudioSource jumpAudio;
    public AudioSource moveAudio;
    public AudioSource hurtAudio;
    public AudioSource collectHeartAudio;
    public AudioSource enemyHurtAudio;
    public AudioSource keyAudio;
    public AudioSource arrowAudio;
    public AudioSource enemyDied1Audio;
    public AudioSource enemyDied2Audio;
    public AudioSource collectGemAudio;
    public AudioSource playButtonAudio;
    public AudioSource otherButtonAudio;
    public AudioSource winAudio;

    public void PlayAttack1Sound()
    {
        attack1Audio.Play();
    }

    public void PlayAttack2Sound()
    {
        attack2Audio.Play();
    }

    public void PlayAttack3Sound()
    {
        attack3Audio.Play();
    }

    public void PlayJumpSound()
    {
        jumpAudio.Play();
    }

    public void PlayWalkSound()
    {
        moveAudio.Play();
    }

    public void PlayHurtSound()
    {
        hurtAudio.Play();
    }

    public void PlayHeartSound()
    {
        collectHeartAudio.Play();
    }

    public void PlayEnemySlashSound()
    {
        enemyAttackAudio.Play();
    }

    public void PlayEnemyHurtSound()
    {
        enemyHurtAudio.Play();
    }

    public void PlayKeySound()
    {
        keyAudio.Play();
    }

    public void PlayShootSound()
    {
        arrowAudio.Play();
    }

    public void PlayEnemyDied1Sound()
    {
        enemyDied1Audio.Play();
    }

    public void PlayEnemyDied2Sound()
    {
        enemyDied2Audio.Play();
    }

    public void PlayCollectGemSound()
    {
        collectGemAudio.Play();
    }

    public void PlayButtonSound()
    {
        playButtonAudio.Play();
    }

    public void PlayOtherButtonSound()
    {
        otherButtonAudio.Play();
    }

    public void PlayWinSound() 
    {
        winAudio.Play(); 
    }
}
