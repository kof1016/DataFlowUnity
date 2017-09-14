/*
 * 模擬視覺端，先QueryNotifier<對應功能的介面>，掛上事件，取得結果顯示於畫面
 * sample如下
 */

using System;
using System.Collections.Generic;

using DataDefine;

using GameLogic.Play;

using UnityEngine;

namespace Script
{
    public class Visual : MonoBehaviour
    {
        //private Move _Move;

        private IInput _Input;

        private List<Player> _Players;

        public GameObject PlayerPrefab;
        private void Start()
        {
            _Players = new List<Player>();
          //  _Move = new DataDefine.Move();

            Entry.GetQuerier().QueryNotifier<IPlayer>().Supply += CreatePlayer;
            Entry.GetQuerier().QueryNotifier<IInput>().Supply += PlayerInput;
        }

        private void CreatePlayer(IPlayer obj)
        {
            var instance = GameObject.Instantiate(PlayerPrefab);
            
            var playerComponent = instance.GetComponent<Player>();

            playerComponent.SetPlayer(obj);
            
            // create prefab instance

            //obj.MoveEvent += (move) => { _Move = move; };
        }

        private void PlayerInput(IInput input)
        {
            _Input = input;
        }

        private void Update()
        {
            if(_Input == null)
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                _Input.OpCode("Forward");
            }
            else if(Input.GetKeyUp(KeyCode.Space))
            {
                _Input.OpCode("Stop");
            }
        }

        private void OnDestroy()
        {
            Entry.GetQuerier().QueryNotifier<IInput>().Supply -= PlayerInput;
            Entry.GetQuerier().QueryNotifier<IPlayer>().Supply -= CreatePlayer;
        }
    }
}
