   
    using System;  
    
    using System.Collections.Generic;
    
    namespace DataDefine.Ghost 
    { 
        public class CIPlayer : DataDefine.IPlayer, Library.Synchronize.IGhost
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

            public CIPlayer(Guid id, Type ghost_type, bool have_return )
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
            
            

                System.Boolean _Main;

                System.Boolean DataDefine.IPlayer.Main { get{ return _Main;} }

                System.Action<DataDefine.Move> _MoveEvent;
                
                event System.Action<DataDefine.Move> DataDefine.IPlayer.MoveEvent
                {
                    add { _MoveEvent += value;}
                    remove { _MoveEvent -= value;}
                }
        }
    }

            using System;  
            using System.Collections.Generic;
    
            namespace DataDefine.Event.IPlayer 
            { 
                public class MoveEvent : Synchronization.Interface.IEventProxyCreator
                {
                    Type _Type;
                    string _Name;
            
                    public MoveEvent()
                    {
                        _Name = "MoveEvent";
                        _Type = typeof(DataDefine.IPlayer);                   
            
                    }
    
                    Delegate Synchronization.Interface.IEventProxyCreator.Create(Guid soul_id, string event_id, Synchronization.PreGenerated.InvokeEventCallback invoke_event)
                    {                
                        var closure = new Synchronization.PreGenerated.GenericEventClosure<DataDefine.Move>(soul_id, event_id, invoke_event);                
                        return new Action<DataDefine.Move>(closure.Run);
                    }
        
                    Type Synchronization.Interface.IEventProxyCreator.GetType()
                    {
                        return _Type;
                    }            

                    string Synchronization.Interface.IEventProxyCreator.GetName()
                    {
                        return _Name;
                    }            
                }
            }
