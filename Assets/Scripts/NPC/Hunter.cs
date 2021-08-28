using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class Hunter : NPC.Enemy
    {
        public Hunter(NavMeshAgent a) : base(a)
        {
            MovementSpeed = 65;
            AngularSpeed = 200;
            Acceleration = 200;
            StoppingDistance = 2;
            Health = 250f;
        }
    }
}
