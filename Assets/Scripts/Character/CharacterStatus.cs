using System;

namespace Character {
    [Serializable]
    public class Status {
        public readonly int maxHealth;
        public int health;
        public int resilience;
        public readonly int maxImbalance;
        public int imbalance;
        public int attack;
        public float attackSpeed;
        public int energy;
        public float speed;

        public Status() {
        }

        public Status(int maxHealth, int maxImbalance) {
            this.maxHealth = maxHealth;
            this.maxImbalance = maxImbalance;
        }
    }
}

