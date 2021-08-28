using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class Chaser : NPC.Enemy
    {
        public Chaser(NavMeshAgent a) : base(a)
        {
            MovementSpeed = 16;
            AngularSpeed = 200;
            Acceleration = 16;
            StoppingDistance = 1;
            Health = 250f;
        }
    }
}
