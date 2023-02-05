using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrossfadeAnimator : MonoBehaviour
{
    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>() ?? GetComponentInParent<Animator>();
        if (Animator == null)
        {
            Debug.LogError($"Animator of {gameObject.name} was not found in parent or children!");
        }
    }

    public void DefaultPlay(string name)
    {
        Animator.SetTrigger(name);
        // Play(name);
        Debug.Log(message: $"Derp: {name}");
    }

    public void Play(string name, float fade = 0.1f, int layer = 0)
    {
        Play(Animator.StringToHash(name), fade, layer);
    }

    private void Play(int hash, float fade, int layer)
    {
        Debug.Log($"Derp2: {hash}");
        // Debug.Log($"Hash: {hash}, fade: {fade}, layer: {layer}");
        Animator.CrossFade(hash, fade, layer);
    }
}