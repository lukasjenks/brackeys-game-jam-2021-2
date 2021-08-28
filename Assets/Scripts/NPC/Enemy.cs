using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class Enemy : AI.Entity
    {
        private string _projectile;

        public string Projectile
        {
            get { return _projectile; }
            set { _projectile = value; }
        }
        private float _health;
        public Enemy(NavMeshAgent a)
        {
            _agent = a;
            _health = 100f;
        }

        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public bool TakeDamage(float dmg)
        {
            if (_health - dmg < 0)
            {
                return true; //signifies death
            }
            else
            {
                _health -= dmg;
                return false;
            }
        }
    }
}
