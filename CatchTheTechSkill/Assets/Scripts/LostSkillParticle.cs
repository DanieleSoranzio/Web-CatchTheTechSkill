using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class LostSkillParticle : Poolable
{
    private ParticleSystem particleEffect;
    public Vector3 particlePos;

    private void Awake()
    {
        particleEffect = GetComponent<ParticleSystem>();
        var main = particleEffect.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    
    public void PlayParticleEffect()
    {
        particleEffect.transform.position = particlePos;
        particleEffect.Play();
    }
    
    private void OnParticleSystemStopped()
    {
        ReturnToPool();
    }

    
}
