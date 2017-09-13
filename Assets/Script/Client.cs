using Library.Framework;

using LSNetwork;

using Regulus.Remoting;
using Regulus.Utility;

using UnityEngine;

using IUpdatable = Library.Utility.IUpdatable;
using Updater = Library.Utility.Updater;

namespace Script
{
    public class Client : MonoBehaviour, IUpdatable
    {
        private readonly Center _Center;

        private readonly Connector _Connector;

        private readonly Updater _Updater;

        private Command _Command;

        private Regulus.Utility.Console.IViewer _Viewer;

        public Console Console;

        // private Logic _Logic;

        // private Visual _Visual;
        public Client()
        {
            // _Command = Console.Command;
            // _Viewer = Console;
            _Center = new Center();
            _Updater = new Updater();

            _Connector = new Connector(new Protocol());

            // _Updater.Add(_Center);
            // _Updater.Add(new Logic(_Center.Binder, _Viewer));
            // _Updater.Add(new Visual(_Center.GhostQuerier, _Command, _Viewer));
        }

        void IBootable.Launch()
        {
            // _Updater
        }

        void IBootable.Shutdown()
        {
            // ((IUpdatable)_Center).Shutdown();
        }

        bool IUpdatable.Update()
        {
            // _Updater.Working();
            return true;
        }

        public void Start()
        {
            _Command = Console.Command;
            _Viewer = Console;

            _Updater.Add(_Center);
            _Updater.Add(new Logic(_Center.Binder, _Viewer, _Connector));
            _Updater.Add(new Visual(_Center.GhostQuerier, _Command, _Viewer));

            _Connector.Agent.Launch();

            _Connector.OpenerEvent += _OpenerHandler;

            _Command.RegisterLambda<Connector, string, int>(_Connector, (ins, ip, port) => ins.Connect(ip, port));
        }

        private void _OpenerHandler(Opener opener)
        {
            _Command.Unregister("Connect");
            opener.PickerEvent += _PickerHandler;
            _Command.RegisterLambda<Opener, int>(opener, (ins, count) => ins.Open(count));
        }

        private void _PickerHandler(Picker picker)
        {
            picker.UpdateEvent += _UpdatePick;
            picker.NextEvent += _BattleFieldHandler;
            _Command.RegisterLambda(picker, ins => ins.GetCharactors(), _Charactors);
            _Command.RegisterLambda(picker, ins => ins.Query());
            _Command.RegisterLambda<Picker, int, Value<bool>>(picker, (ins, number) => ins.Select(number), _ReturnValue);
        }

        private void _UpdatePick(PlayerRecord[] records)
        {
            foreach(var playerRecord in records)
            {
                _Viewer.WriteLine($"player:{playerRecord.Player} charactor:{playerRecord.Charactor}");
            }
        }

        private void _BattleFieldHandler(BattleField battle)
        {
            _Command.Unregister("GetCharactors");
            _Command.Unregister("Query");
            _Command.Unregister("Select");
            _Updater.Add(new BattleFieldHandler(_Connector.Agent, battle, _Viewer, _Command));
        }

        private void _Charactors(int[] charactors)
        {
            foreach(var charactor in charactors)
            {
                _Viewer.WriteLine("charactor " + charactor);
            }
        }

        private void _ReturnValue(Value<bool> val)
        {
            val.OnValue += select_result =>
                {
                    if(select_result)
                    {
                        _Viewer.WriteLine("Select surccess.");
                    }
                    else
                    {
                        _Viewer.WriteLine("Select fail.");
                    }
                };
        }

        private void Update()
        {
            _Connector.Agent.Update();
            _Updater.Working();
        }

        private void OnDestroy()
        {
            _Connector.Agent.Shutdown();
        }
    }
}
