using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
public class DinoSoundPlayer : MonoBehaviour
{
    private const string soundFolder = "Sounds/";
    private AudioSource soundPlayer;
    private AudioSource walkSoundPlayer; 

    void Awake()
    {
        soundPlayer = GetComponents<AudioSource>()[0];
        walkSoundPlayer = GetComponents<AudioSource>()[1];

        // determine level's terrain for walking sound
        if (SceneManager.GetActiveScene().name == "LevelOneScene")
            walkSoundPlayer.clip = Resources.Load<AudioClip>(soundFolder + "Step_Grass_Loop");
        else
            walkSoundPlayer.clip = Resources.Load<AudioClip>(soundFolder + "Step_Stone_Loop");
    }

    // Stop any current sound and play the next sound by their file name in the resource folder
    private void PlayClip(string soundName)
    {
        soundPlayer.clip = Resources.Load<AudioClip>(soundFolder + soundName);

        if (soundPlayer.isPlaying)
            soundPlayer.Stop();
        soundPlayer.Play();
    }

    public void playJumpSfx() { PlayClip("Jump_2"); }
    public void playKickSfx() { PlayClip("Kick_Dino"); }
    public void playRoarSfx() { PlayClip("Dino_Scream"); }
    public void playDamagedSfx() { PlayClip("Heart"); }
    public void playDeadSfx() { PlayClip("Game_Over_Lava"); }

    // Play walking sfx only when no other sounds are playing
    public void playWalkSfx(bool toPlay)
    {
        Debug.Log(toPlay);
        if (toPlay && !walkSoundPlayer.isPlaying) 
            walkSoundPlayer.Play();
        else if (!toPlay && walkSoundPlayer.isPlaying)
            walkSoundPlayer.Stop();
    }
}
