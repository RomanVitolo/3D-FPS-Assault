﻿using Interfaces;
using UnityEngine;

namespace FSM
{
    public class ReloadState<T> : FSMState<T>
    {
        public IAttack _Attack;
        public ReloadState(IAttack attack)
        {
            _Attack = attack;
        }       
        public override void Enter()
        {
            _Attack.Reload(true);
            Debug.Log("Reloading");
        }                        
    }
}