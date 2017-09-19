using DataDefine;

using UnityEngine;

using UnityStandardAssets.Utility;

namespace Script
{
    public class Player : MonoBehaviour
    {
        private IPlayer _Player;

        private Vector3 _Direction;

        // Use this for initialization
        void Start ()
        {
            _Direction = new Vector3();
        }
	
        // Update is called once per frame
        void Update ()
        {
            var dir = _Direction * UnityEngine.Time.deltaTime;            
		    transform.position = transform.position  + dir;
        }


        public void SetPlayer(IPlayer player)
        {
            _Player = player;
            
            player.MoveEvent += Player_MoveEvent;

            player.IsMain().OnValueEvent += (result) =>
                {
                    if(result)
                        Object.FindObjectOfType<FollowTarget>().target = transform;
                };
                
        }

        private void Player_MoveEvent(Move move)
        {
            transform.position = new Vector3(move.StartPositionX , 0 , move.StartPositionY);
            _Direction = new Vector3(move.DirectionX , 0 , move.DirectionY); 
        }


    }
}
