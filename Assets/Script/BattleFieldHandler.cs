using System;

using Drone;

using Library.Framework;

using LSIntegrate;

using LSNetwork;

using Regulus.Remoting;

using Command = Regulus.Utility.Command;
using IUpdatable = Library.Utility.IUpdatable;

namespace Script
{
    internal class BattleFieldHandler : IUpdatable, IControlSystem, IPhysicsSystem
    {
        private readonly IAgent _ConnectorAgent;
        private readonly BattleField _Battle;
        private readonly Regulus.Utility.Console.IViewer _Viewer;
        private readonly Command _Command;

        public BattleFieldHandler(IAgent connector_agent, BattleField battle, Regulus.Utility.Console.IViewer viewer, Command command)
        {
            _ConnectorAgent = connector_agent;
            _Battle = battle;
            _Viewer = viewer;
            _Command = command;
        }

        void IBootable.Launch()
        {
            _Viewer.WriteLine("Battle Start.");
            _Battle.Start(this, this, new TurnBuffer<ITurn>(1000));
            _Command.RegisterLambda<BattleFieldHandler, int, string>(this, (ins, charactor, op) => ins.Execute(charactor, op));
        }

        public void Execute(int player, string op)
        {
            var command = new CommandPackage
                              {
                                  OpCode = op,
                                  PlayerId = player
                              };
            _Battle.Send(command);
        }

        void IBootable.Shutdown()
        {
            _Command.Unregister("Execute");
            _Battle.End();
        }

        bool IUpdatable.Update()
        {
            _Battle.Tick();
            return true;
        }

        void IControlSystem.Execute(ICommand InCommand)
        {
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