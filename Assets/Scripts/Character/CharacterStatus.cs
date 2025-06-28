using System;
using UnityEngine;

namespace Character {
    [Serializable]
    public class Status {
        public readonly int maxHealth;
        public int health;
        public readonly int increaseResilience;
        public int resilience;
        public readonly int maxImbalance;
        public int imbalance;
        public int attack;
        public float attackSpeed;
        public readonly int maxEnergy;
        public readonly int lowEnergyBoundary;
        public int energy;
        public float speed;
        public float maxJumpSpeed;
        public float jumpChargeSpeed;

        public Status() {
        }

        public Status(int maxHealth, int maxImbalance, int maxEnergy, int lowEnergyBoundary, int increaseResilience) {
            this.maxHealth = maxHealth;
            this.maxImbalance = maxImbalance;
            this.maxEnergy = maxEnergy;
            this.lowEnergyBoundary = lowEnergyBoundary;
            this.increaseResilience = increaseResilience;
        }
    }
}

