using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public interface IEntity
    {
        void MoveToPosition(Vector3 dest);
    }

    public class Entity : IEntity
    {
        protected NavMeshAgent _agent;
        private float _movementSpeed;
        private float _angularSpeed;
        private float _acceleration;
        private float _stoppingDistance;

        public float MovementSpeed
        {
            get { return _movementSpeed; }
            set
            {
                _movementSpeed = value;
                _agent.speed = value;
            }
        }

        public float AngularSpeed
        {
            get { return _angularSpeed; }
            set
            {
                _angularSpeed = value;
                _agent.angularSpeed = value;
            }
        }

        public float Acceleration
        {
            get { return _acceleration; }
            set
            {
                _acceleration = value;
                _agent.acceleration = value;
            }
        }

        public float StoppingDistance
        {
            get { return _stoppingDistance; }
            set
            {
                _stoppingDistance = value;
                _agent.stoppingDistance = value;
            }
        }

        public float RemainingDistance()
        {
            return _agent.remainingDistance;
        }

        public bool IsStopped()
        {
            return _agent.isStopped;
        }

        public void Warp(Vector3 dest)
        {
            _agent.Warp(dest);
        }

        public void MoveToPosition(Vector3 dest)
        {
            _agent.SetDestination(dest);
        }

        // Gets the vector of the velocity relative to the game object's position
        public Vector3 GetVelocityRelative(Rigidbody r, Transform t)
        {
            return r.velocity - t.position;
        }
    }
}
