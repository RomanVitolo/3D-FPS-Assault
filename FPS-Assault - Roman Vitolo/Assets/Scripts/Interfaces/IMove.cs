﻿using System;
using UnityEngine;

namespace Interfaces
{
    public interface IMove
    {   
        void Move(Vector3 direction);
        void Hide();              
        void Idle();
        void Run();                          
    }
}