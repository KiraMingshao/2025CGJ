using DG.Tweening;
namespace AI.FSM
{
    public class IdleState : FSMState
    {
        protected override void init()
        {
            this.StateID = FSMStateID.Idle;
        }
        public override void OnStateEnter(FSMBase fsm)
        {
            if (fsm is CharacterBattleActionFSM characterFSM)
            {
                StartShake(characterFSM);
            }
        }

        public override void OnStateStay(FSMBase fsm)
        {
            base.OnStateStay(fsm);

        }

        public override void OnStateExit(FSMBase fsm)
        {
            if (fsm is CharacterBattleActionFSM characterFSM)
            {

            }
        }

        private void StartShake(CharacterBattleActionFSM characterFSM)
        {

            characterFSM.transform.DORotate(new UnityEngine.Vector3(0, 0, 30), (float)(30 - characterFSM.transform.eulerAngles.z) * 0.2f).OnComplete(
              () =>
              {
                  Sequence seq = DOTween.Sequence();
                  //添加动画到序列中
                  seq.Append(characterFSM.transform.DORotate(new UnityEngine.Vector3(0, 0, -30), (float)(30 - characterFSM.transform.eulerAngles.z) * 0.2f));
                  seq.Append(characterFSM.transform.DORotate(new UnityEngine.Vector3(0, 0, 30), (float)(30 - characterFSM.transform.eulerAngles.z) * 0.2f));

                  seq.SetLoops(-1);
              }
            );           
        }

    }
}