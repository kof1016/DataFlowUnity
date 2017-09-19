using UnityEngine;

namespace Script
{
    public class DisplaySwitch : MonoBehaviour
    {
        public KeyCode Key;

        public bool Show;

        public MonoBehaviour Target;

        // Use this for initialization
        private void Start()
        {
            Target.enabled = Show;
        }

        private void OnGUI()
        {
            if(GUILayout.Button("按"))
            {
                Show = !Show;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if(Input.GetKeyDown(Key))
            {
               Debug.Log("按~");
                Show = !Show;
            }

            if(Target.enabled != Show)
            {
                Target.enabled = Show;
            }
        }

        public void Trigger()
        {
            Show = !Show;
        }
    }
}
