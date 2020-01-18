using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DinoSoundPlayer : MonoBehaviour
{
    private const string soundFolder = "Sounds/";
    private AudioSource soundPlayer;
    private string currentTerrain = "";

    void Awake()
    {
        soundPlayer = GetComponent<AudioSource>();   
    }

    // Stop any current sound and play the next sound by their file name in the resource folder
    private void PlayNext(string soundName)
    {
        soundPlayer.clip = Resources.Load<AudioClip>(soundFolder + soundName);

        if (soundPlayer.isPlaying)
            soundPlayer.Stop();
        soundPlayer.Play();
    }

    public void playJumpSfx() { PlayNext("Jump_2"); }
    public void playKickSfx() { PlayNext("Jump_2"); }   // TODO change for kicking sfx
    public void playRoarSfx() { PlayNext("Dino_Scream"); }
    public void playDamagedSfx() { PlayNext("Heart"); }
    public void playDeadSfx() { PlayNext("Game_Over_Lava"); }

    // Play walking sfx only when no other sounds are playing
    //public void playWalkSfx(string terrain)
    //{
    //    if (!soundPlayer.isPlaying)
    //    {
    //        // switch terrain sfx only if dino is stepping on new terrain, otherwise keep current
    //        if (currentTerrain != terrain)   
    //        {
    //            currentTerrain = terrain;
    //            switch (terrain)
    //            {
    //                case "Grass":
    //                    soundPlayer.clip = Resources.Load<AudioClip>(soundFolder + "WalkGrass");
    //                    break;
    //                case "Stone":
    //                    soundPlayer.clip = Resources.Load<AudioClip>(soundFolder + "WalkStone");
    //                    break;
    //                default: break;
    //            }
    //        }
    //        soundPlayer.Play();
    //    }
    //}
}
