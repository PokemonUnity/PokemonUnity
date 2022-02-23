//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace PokemonUnity
{
    /// <summary>
    /// Splash scene is when a game boots up and they display the title card (where you `Press Start` before start menu)
    /// </summary>
    /// <remarks>
    /// In the case of this template, the preload scene and splash scene roles are reversed;
    /// instead of loading logo in preload, the template loads logo in splash and start game in menu
    /// </remarks>
    public class ProcedureSplash : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // TODO: Add a Splash animation, skip it here
            // In the editor mode, directly enter the preloading process; otherwise, check the version
            //ToDo: Should check version IF online (or server available) otherwise continue to preloading
            ChangeState(procedureOwner, GameEntry.Base.EditorResourceMode ? typeof(ProcedurePreload) : typeof(ProcedureCheckVersion));
        }
    }
}
