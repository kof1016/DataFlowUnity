using Library.Framework;

using Regulus.Utility;

using Synchronization.Interface;

namespace Script
{
    public class Center : Library.Utility.IUpdatable
    {
        private readonly Library.Utility.Updater _Updater;

        public readonly ISoulBinder Binder;

        public readonly IGhostQuerier GhostQuerier;

        public Center()
        {
            _Updater = new Library.Utility.Updater();

            var agent = new SyncLocal.Agent();
            GhostQuerier = agent.GhostQuerier; // client
            Binder = agent.Binder; // server
            _Updater.Add(agent);
        }

        void IBootable.Launch()
        {
            _Updater.Add(GhostQuerier);
        }

        void IBootable.Shutdown()
        {
            _Updater.Shutdown();
        }

        bool Library.Utility.IUpdatable.Update()
        {
            _Updater.Working();
            return true;
        }
    }
}
