namespace States
{
    public class PlayState : BaseState
    {
        public PlayState(GameFSM stateMachine) : base("Play", stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            ((GameFSM)stateMachine).charactersSplineFollower.follow = true;
        }
    }
}
