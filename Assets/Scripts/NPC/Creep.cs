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
            MovementSpeed = 9;
            AngularSpeed = 140;
            Acceleration = 8;
            StoppingDistance = 15;
            Health = 100f;
        }
    }
}
