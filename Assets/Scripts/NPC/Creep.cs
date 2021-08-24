using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class Creep : NPC.Enemy
    {
        public Creep(NavMeshAgent a) : base(a)
        {
            MovementSpeed = 4;
            AngularSpeed = 140;
            Acceleration = 8;
            StoppingDistance = 1;
            Health = 250f;
        }
    }
}
