/*
 * 模擬邏輯端的功能，要有什麼功能需實作對應的介面
 * 
 */

using DataDefine;

using Library.Framework;
using Library.Synchronize;
using Library.Utility;

using LSNetwork;

using Synchronization.Interface;

namespace Script
{
    internal class Logic : IUpdatable, IVerify, IMove
    {
        private readonly ISoulBinder _Binder;

        private readonly Regulus.Utility.Console.IViewer _Viewer;

        private Connector _Connector;

        public Logic(ISoulBinder binder, Regulus.Utility.Console.IViewer viewer, Connector connector)
        {
            _Binder = binder;
            _Viewer = viewer;
            _Connector = connector;
        }

        void IMove.Walk()
        {
            _Viewer.WriteLine("logic walk");
        }

        void IBootable.Launch()
        {
            _Viewer.WriteLine("\nTerry Test");

            _Binder.Bind<IVerify>(this);
            _Binder.Bind<IMove>(this);

            
        }

        void IBootable.Shutdown()
        {
            _Binder.Unbind<IVerify>(this);
            _Binder.Unbind<IMove>(this);
        }

        bool IUpdatable.Update()
        {
            return true;
        }

        Value<bool> IVerify.Login(string id, string password)
        {
            _Viewer.WriteLine("logic login");

            return true;
        }
    }
}
