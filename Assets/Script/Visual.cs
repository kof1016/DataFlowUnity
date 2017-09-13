/*
 * 模擬視覺端，先QueryNotifier<對應功能的介面>，掛上事件，取得結果顯示於畫面
 * sample如下
 */
using DataDefine;

using Library.Framework;
using Library.Synchronize;

using Regulus.Utility;

using Synchronization.Interface;

using IUpdatable = Library.Utility.IUpdatable;

namespace Script
{
    public class Visual : IUpdatable
    {
        private readonly IGhostQuerier _GhostQuerier;
        private readonly Command _Command;
        private Regulus.Utility.Console.IViewer _Viewer;

        public Visual(IGhostQuerier ghost_querier,
                      Command command,
                      Regulus.Utility.Console.IViewer viewer)
        {
            _GhostQuerier = ghost_querier;
            _Command = command;
            _Viewer = viewer;
        }

        void IBootable.Launch()
        {
            _Command.Register(
                              "start",
                              () =>
                                  {
                                      _GhostQuerier.QueryNotifier<IVerify>().Supply += _SupplyVerify;
                                      _GhostQuerier.QueryNotifier<IMove>().Supply += _SupplyMove;
                                  });
        }

        private void _Add(int result)
        {
            _Viewer.WriteLine($"result = {result}");
        }

        void IBootable.Shutdown()
        {
            _Shutdown();
        }

        private void _Shutdown()
        {
            _GhostQuerier.QueryNotifier<IVerify>().Supply -= _SupplyVerify;
            _GhostQuerier.QueryNotifier<IVerify2>().Supply -= _SupplyVerify2;
        }

        private void _SupplyVerify2(IVerify2 obj)
        {
            var result = obj.Login("1", "1");
        }

        private void _SupplyVerify(IVerify obj)
        {
            // command 使用方法2
            _Command.RegisterLambda<IVerify, string, string, Value<bool>>
                    (
                     obj,
                     (instance, a1, a2) => instance.Login(a1, a2),
                     result => { _Viewer.WriteLine($"回傳{result}"); });


            //_Command.Register("Login []", () => _Login(obj));

            
        }

        private void _SupplyMove(IMove obj)
        {
            _Command.RegisterLambda(this, instance => instance.Walk());

            
        }

        private void Walk()
        {
            _Viewer.WriteLine("visual walking");
        }

        private void _LoginA(string a, string b)
        {

        }


        private void _Login(IVerify obj)
        {
            var result = obj.Login("1", "1");
            result.OnValueEvent += res =>
                {
                    if (res)
                    {
                        // login ok
                    }
                };
        }

        bool IUpdatable.Update()
        {
            return true;
        }
    }
}