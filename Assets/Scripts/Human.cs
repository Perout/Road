using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Human : MonoBehaviour
{
    [SerializeField] private Transform _fixationPoint;
    private Animator _animator;

    public Transform FixationPoint => _fixationPoint;//читаем,но не меняем

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void Run()
    {
        _animator.SetBool("Run", true);
    }
    public void StopRun()
    {
        _animator.SetBool("Run", false);
    }

}
