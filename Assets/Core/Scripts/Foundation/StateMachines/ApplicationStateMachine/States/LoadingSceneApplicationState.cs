using Cysharp.Threading.Tasks;

namespace MyCore.StateMachine
{
    public class LoadingSceneApplicationState : ApplicationStateMachineBaseState
    {
        private LevelLoader levelLoader;
        //private ISaveLoadSystem sls;
        public LoadingSceneApplicationState(ApplicationContext context, LevelLoader levelLoader) : base(context)
        {
            this.levelLoader = levelLoader;
        }

        public override void Enter()
        {
            levelLoader.LoadLevelScene(1);
        }

        public override void Exit()
        {
            
        }
    }
}