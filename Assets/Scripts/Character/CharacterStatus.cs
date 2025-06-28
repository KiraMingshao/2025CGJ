using System;
using UnityEngine;

namespace Character {
    [Serializable]
    public class Status {
        public int maxHealth;
        public int health;
        public int increaseResilience;
        public int resilience;
        public int maxImbalance;
        public int imbalance;
        public int attack;
        public float attackSpeed;
        public int maxEnergy;
        public int lowEnergyBoundary;
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

