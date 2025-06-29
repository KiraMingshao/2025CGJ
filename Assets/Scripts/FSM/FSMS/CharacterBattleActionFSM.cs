using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AI.FSM {
    [RequiredComponent(typeof(Character.Character))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterBattleActionFSM : FSMBase {
        public Character.Character character { get; private set; }
        public Collider2D footCollider;
        public Collider2D bodyCollider;
        public Collider2D sweepCollider;
        public Collider2D chopUpCollider;
        public Collider2D chopDownCollider;
        public Collider2D crouchCollider;

        [Header("Wave")]
        public GameObject wave;
        public float waveStrengthFactor = 0.8f;
        public float waveVelocityFactor = 5f;

        [HideInInspector]
        public float jumpCharge;
        [HideInInspector]
        public Rigidbody2D rb;

        protected override void Init() {
            this.character = this.GetComponent<Character.Character>();
            this.sweepCollider.enabled = false;
            this.chopUpCollider.enabled = false;
            this.chopDownCollider.enabled = false;
            this.rb = this.GetComponent<Rigidbody2D>();
            this.crouchCollider.enabled = false;
        }

        protected override void SetUpFSM() {
            base.SetUpFSM();

            IdleState idleState = new IdleState();
            idleState.AddMap(FSMTriggerID.AttackKeyPressed, FSMStateID.Attack);
            idleState.AddMap(FSMTriggerID.UnderAttack, FSMStateID.UnderAttack);
            idleState.AddMap(FSMTriggerID.JumpKeyPressed, FSMStateID.JumpCharge);
            idleState.AddMap(FSMTriggerID.CrouchKeyPressed, FSMStateID.Crouch);
            this._states.Add(idleState);

            AttackState attackState = new AttackState();
            attackState.AddMap(FSMTriggerID.AnimateFinished, FSMStateID.Idle);
            attackState.AddMap(FSMTriggerID.UnderAttack, FSMStateID.UnderAttack);
            this._states.Add(attackState);

            JumpChargeState jumpChargeState = new JumpChargeState();
            jumpChargeState.AddMap(FSMTriggerID.JumpKeyReleased, FSMStateID.Jump);
            jumpChargeState.AddMap(FSMTriggerID.UnderAttack, FSMStateID.UnderAttack);
            jumpChargeState.AddMap(FSMTriggerID.CrouchKeyPressed, FSMStateID.Idle);
            this._states.Add(jumpChargeState);

            JumpState jumpState = new JumpState();
            jumpState.AddMap(FSMTriggerID.OnGround, FSMStateID.Idle);
            jumpState.AddMap(FSMTriggerID.UnderAttack, FSMStateID.UnderAttack);
            this._states.Add(jumpState);

            CrouchState crouchState = new CrouchState();
            crouchState.AddMap(FSMTriggerID.CrouchKeyReleased, FSMStateID.Idle);
            crouchState.AddMap(FSMTriggerID.AttackKeyPressed, FSMStateID.Attack);
            this._states.Add(crouchState);

            UnderAttackState underAttackState = new UnderAttackState();
            underAttackState.AddMap(FSMTriggerID.AnimateFinished, FSMStateID.Idle);
            this._states.Add(underAttackState);
        }
    }
}