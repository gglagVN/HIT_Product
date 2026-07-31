using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource voiceSource;

    [SerializeField]
    private AudioSource footstepSource;

    [Header("Zombie")]
    public AudioClip[] idleClips;
    public AudioClip[] detectClips;
    public AudioClip[] attackClips;
    public AudioClip[] hurtClips;
    public AudioClip[] deathClips;
    public AudioClip[] walkFootsteps;
    public AudioClip[] runFootsteps;

    [Header("Gunner")]
    public AudioClip[] gunnerShootClips;
    public AudioClip[] gunnerHurtClips;
    public AudioClip[] gunnerDeathClips;
    public AudioClip[] gunnerWalkFootsteps;
    public AudioClip[] gunnerRunFootsteps;

    [Header("Settings")]
    [Range(0.8f, 1.2f)]
    public float minPitch = 0.95f;

    [Range(0.8f, 1.2f)]
    public float maxPitch = 1.05f;



    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        voiceSource.pitch = Random.Range(minPitch, maxPitch);

        voiceSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);

        voiceSource.pitch = 1f;
    }
    public void StartWalkLoop()
    {
        if (walkFootsteps.Length == 0)
            return;

        if (footstepSource.isPlaying &&
            footstepSource.clip == walkFootsteps[0])
            return;

        footstepSource.Stop();

        footstepSource.clip = walkFootsteps[0];

        footstepSource.loop = true;

        footstepSource.Play();
    }

    public void StartRunLoop()
    {
        if (runFootsteps.Length == 0)
            return;

        if (footstepSource.isPlaying &&
            footstepSource.clip == runFootsteps[0])
            return;

        footstepSource.Stop();

        footstepSource.clip = runFootsteps[0];

        footstepSource.loop = true;

        footstepSource.Play();
    }

    public void StopFootstep()
    {
        if (!footstepSource.isPlaying)
            return;

        footstepSource.Stop();
    }

    //================ ZOMBIE =================

    public void PlayIdle()
    {
        PlayRandom(idleClips);
    }

    public void PlayDetect()
    {
        PlayRandom(detectClips);
    }

    public void PlayAttack()
    {
        PlayRandom(attackClips);
    }

    public void PlayHurt()
    {
        PlayRandom(hurtClips);
    }

    public void PlayDeath()
    {
        PlayRandom(deathClips);
    }

    public void PlayWalkFootstep()
    {
        PlayRandom(walkFootsteps);
    }

    public void PlayRunFootstep()
    {
        PlayRandom(runFootsteps);
    }

    //================ GUNNER =================

    public void PlayGunnerShoot()
    {
        PlayRandom(gunnerShootClips);
    }

    public void PlayGunnerHurt()
    {
        PlayRandom(gunnerHurtClips);
    }

    public void PlayGunnerDeath()
    {
        PlayRandom(gunnerDeathClips);
    }

    public void PlayGunnerWalk()
    {
        PlayRandom(gunnerWalkFootsteps);
    }

    public void PlayGunnerRun()
    {
        PlayRandom(gunnerRunFootsteps);
    }
}