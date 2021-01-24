using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisualEffects : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Match3.AddDeathPiece += ShakingPlatform;
    }

    private void OnDisable()
    {
        Match3.AddDeathPiece -= ShakingPlatform;
    }

    private void ShakingPlatform(NodePiece nodePiece)
    {
        animator.SetTrigger("Shaking");
    }
}
