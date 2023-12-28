using UnityEngine.UI;

namespace States
{
    public class StartState : BaseState
    {
        private Image _drawToStartImage;
        public StartState(GameFSM stateMachine) : base("Start", stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _drawToStartImage = ((GameFSM)stateMachine).drawToStartImage;
            _drawToStartImage.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            _drawToStartImage.gameObject.SetActive(false);
        }
    }
}
