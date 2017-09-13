//using Library.Framework;
//
//using LSNetwork;
//using IUpdatable = Library.Utility.IUpdatable;
//namespace Script
//{
//    public class LockstepController : IUpdatable
//    {
//        private Connector _Connector;
//
//        public LockstepController()
//        {
//            _Connector = new Connector(new Protocol());
//        }
//
//        void IBootable.Launch()
//        {
//            _Connector.Agent.Launch();
//
//            _Connector.OpenerEvent += _OpenerHandler;
//
//            _Command.RegisterLambda<Connector, string, int>(_Connector, (ins, ip, port) => ins.Connect(ip, port));
//        }
//
//        void IBootable.Shutdown()
//        {
//            throw new System.NotImplementedException();
//        }
//
//        bool IUpdatable.Update()
//        {
//            throw new System.NotImplementedException();
//        }
//
//        private void _OpenerHandler(Opener opener)
//        {
//            _Command.Unregister("Connect");
//            opener.PickerEvent += _PickerHandler;
//            _Command.RegisterLambda<Opener, int>(opener, (ins, count) => ins.Open(count));
//        }
//
//        private void _PickerHandler(Picker picker)
//        {
//            picker.UpdateEvent += _UpdatePick;
//            picker.NextEvent += _BattleFieldHandler;
//            _Command.RegisterLambda(picker, ins => ins.GetCharactors(), _Charactors);
//            _Command.RegisterLambda(picker, ins => ins.Query());
//            _Command.RegisterLambda<Picker, int, Value<bool>>(picker, (ins, number) => ins.Select(number), _ReturnValue);
//        }
//
//        private void _UpdatePick(PlayerRecord[] records)
//        {
//            foreach (var playerRecord in records)
//            {
//                _Viewer.WriteLine($"player:{playerRecord.Player} charactor:{playerRecord.Charactor}");
//            }
//        }
//
//        private void _BattleFieldHandler(BattleField battle)
//        {
//            _Command.Unregister("GetCharactors");
//            _Command.Unregister("Query");
//            _Command.Unregister("Select");
//            _Updater.Add(new BattleFieldHandler(_Connector.Agent, battle, _Viewer, _Command));
//        }
//
//        private void _Charactors(int[] charactors)
//        {
//            foreach (var charactor in charactors)
//            {
//                _Viewer.WriteLine("charactor " + charactor);
//            }
//        }
//
//        private void _ReturnValue(Value<bool> val)
//        {
//            val.OnValue += select_result =>
//                {
//                    if (select_result)
//                    {
//                        _Viewer.WriteLine("Select surccess.");
//                    }
//                    else
//                    {
//                        _Viewer.WriteLine("Select fail.");
//                    }
//                };
//        }
//    }
//}