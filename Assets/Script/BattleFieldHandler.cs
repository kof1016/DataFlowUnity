using System;
using System.Collections.Generic;
using System.Linq;

using Drone;

using Library.Framework;

using LSIntegrate;

using LSNetwork;

using Regulus.Remoting;

using SyncLocal;

using Command = Regulus.Utility.Command;
using ISoulBinder = Synchronization.Interface.ISoulBinder;
using IUpdatable = Library.Utility.IUpdatable;

namespace Script
{
    internal class BattleFieldHandler : IUpdatable, IControlSystem, IPhysicsSystem
    {
        private readonly BattleField _Battle;
        private readonly Regulus.Utility.Console.IViewer _Viewer;
        private readonly Command _Command;

        private readonly GameLogic.Play.Center _Center;
        

        public BattleFieldHandler(ISoulBinder binder, BattleField battle, Regulus.Utility.Console.IViewer viewer, Command command)
        {
            _Battle = battle;
            _Viewer = viewer;
            _Command = command;

            _Center = new GameLogic.Play.Center(binder);
        }

        void IBootable.Launch()
        {
            _Viewer.WriteLine("Battle Start.");
            ((IUpdatable)_Center).Launch();
            _Battle.Start(this, this, new TurnBuffer<ITurn>(30000));
            _Command.RegisterLambda<BattleFieldHandler, string>(this, (ins, op) => ins._Send(op));

            foreach(var record in _Battle.GetRecords())
            {
                bool mainPlayer = record.Player == _Battle.GetOwner();
                _Center.JoinPlayer(record.Player, _Battle.GetOwnerCharactor(), mainPlayer);
            }
            _Center.OnMainPlayerOpCodeEvent += _Send;
        }

        private void _Send(string obj)
        {
            var command = new CommandPackage
                              {
                                  OpCode = obj,
                              };
            _Battle.Send(command);
        }

        void IBootable.Shutdown()
        {
            _Command.Unregister("Execute");
            _Center.OnMainPlayerOpCodeEvent -= _Send;
            _Battle.End();
            ((IUpdatable)_Center).Shutdown();
        }

        bool IUpdatable.Update()
        {
            _Battle.Tick();
            ((IUpdatable)_Center).Update();
            return true;
        }

        void IControlSystem.Execute(ICommand InCommand)
        {
            _Center.Execute(InCommand.PlayerId, InCommand.OpCode);

            _Viewer.WriteLine($"Execute Command player:{InCommand.PlayerId} op:{InCommand.OpCode}");
        }

        void IControlSystem.LateSimulate()
        {
           // throw new NotImplementedException();
        }

        void IPhysicsSystem.Simulate()
        {
            //throw new NotImplementedException();
        }

        Action<ICommand> IControlSystem.ExecuteAction
        {
            set
            {
                _Viewer.WriteLine("ExecuteAction.");
            }
        }

        void IPhysicsSystem.LateSimulate()
        {
            //throw new NotImplementedException();
        }
    }
}