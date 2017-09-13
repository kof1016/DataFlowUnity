   
    using System;  
    
    using System.Collections.Generic;
    
    namespace DataDefine.Ghost 
    { 
        public class CIVerify : DataDefine.IVerify, Library.Synchronize.IGhost
        {
            private event Library.Synchronize.CallMethodCallback _OnCallMethodEvent;
            
            event Library.Synchronize.CallMethodCallback Library.Synchronize.IGhost.CallMethodEvent
            {
                add { this._OnCallMethodEvent += value; }
                remove { this._OnCallMethodEvent -= value; }
            }

            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;

            readonly Type _GhostType;

            public CIVerify(Guid id, Type ghost_type, bool have_return )
            {
                _HaveReturn = have_return ;
                _GhostIdName = id;            
                _GhostType = ghost_type;
            }
            
            Guid Library.Synchronize.IGhost.GetID()
            {
                return _GhostIdName;
            }

            object Library.Synchronize.IGhost.GetInstance()
            {
                return this;
            }

            bool Library.Synchronize.IGhost.IsReturnType()
            {
                return _HaveReturn;
            }

            Type Library.Synchronize.IGhost.GetType()
            {
                return _GhostType;
            }
            
            
                Library.Synchronize.Value<System.Boolean> DataDefine.IVerify.Login(System.String _1,System.String _2)
                {                    
                     var returnValue = new Library.Synchronize.Value<System.Boolean>();
                    var info = typeof(DataDefine.IVerify).GetMethod("Login");
                    _OnCallMethodEvent(info , new object[] {_1 ,_2} , returnValue);                    
                    return returnValue;
                }


        }
    }
