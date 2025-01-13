using Cysharp.Threading.Tasks;
using MyCore.SaveLoadSystem;

namespace MyCore.StateMachine
{
    public class LoadingSceneApplicationState : ApplicationStateMachineBaseState
    {
        private LevelLoader levelLoader;
        private ISaveLoadSystem sls;
        public LoadingSceneApplicationState(ApplicationContext context, LevelLoader levelLoader) : base(context)
        {
            this.levelLoader = levelLoader;
        }

        public override void Enter()
        {
            levelLoader.LoadLevelScene(sls.GetData<int>(SaveDataIDHolder.LevelIDDataHolder.CURRENT_LEVEL));
        }

        public override void Exit()
        {
            
        }
    }
}