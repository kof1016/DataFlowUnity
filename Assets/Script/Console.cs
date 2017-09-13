using System;
using System.Collections.Generic;

using Regulus.Utility;

using UnityEngine;

namespace Script
{
    public class Console
            : MonoBehaviour,
              Regulus.Utility.Console.IInput,
              Regulus.Utility.Console.IViewer
    {
        private event Regulus.Utility.Console.OnOutput _OnOutputEvent;

        private const int _LineCounts = 25;

        private const int _MaxLineCounts = 100;

        private Regulus.Utility.Console _Console;

        private Queue<string> _Messages;

        private string _LastMessage;

        private Vector2 _ScrollView;

        public string Input;

        public string Title;

        //private Library.Utility.IUpdatable _CoreUpdatable;

        public Command Command => _Console.Command;

        event Regulus.Utility.Console.OnOutput Regulus.Utility.Console.IInput.OutputEvent
        {
            add
            {
                _OnOutputEvent += value;
            }

            remove
            {
                _OnOutputEvent -= value;
            }
        }

        void Regulus.Utility.Console.IViewer.Write(string message)
        {
            _LastMessage += message;
        }

        void Regulus.Utility.Console.IViewer.WriteLine(string message)
        {
            _WriteLine(message);
        }

        public Console()
        {
            _Messages = new Queue<string>();
            _Console = new Regulus.Utility.Console(this, this);

            Input = string.Empty;
            _LastMessage = string.Empty;
            _ScrollView = Vector2.zero;
        }

        // Use this for initialization
        private void Start()
        {
           // Singleton<Log>.Instance.RecordEvent += _WriteLine;
        }

        private void OnGUI()
        {
            GUILayout.Window(0, new Rect(0, 0, Screen.width / 2.0f, Screen.height), _WindowHandler, Title);

            // GUILayout.Window(0, new Rect(0, 0, 500  / 2, 500), _WindowHandler, Title); 
        }

        private void _WindowHandler(int id)
        {
            GUILayout.BeginVertical();

            _ScrollView = GUILayout.BeginScrollView(_ScrollView, GUILayout.Width(Screen.width / 2.0f), GUILayout.Height(Screen.height * 0.9f));

            foreach(var message in _Messages)
            {
                GUILayout.Label(message);
            }

            if(_LastMessage.Length > 0)
            {
                GUILayout.Label(_LastMessage);
            }

            GUILayout.EndScrollView();

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            Input = GUILayout.TextField(Input);

            if (GUILayout.Button("Send") && Input != string.Empty)
            {
                _WriteLine(Input);

                var args = Input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                _OnOutputEvent?.Invoke(args);

                Input = string.Empty;
            }

            GUILayout.EndHorizontal();
        }

        // Update is called once per frame
        private void Update()
        {
            //_CoreUpdatable.Update();
        }

        private void _WriteLine(string text)
        {
            _Messages.Enqueue(_LastMessage + text);
            if(_Messages.Count > Console._MaxLineCounts)
            {
                _Messages.Dequeue();
            }

            _LastMessage = string.Empty;

            _ScrollView.y = Mathf.Infinity;
        }

        public void WriteLine(string text)
        {
            var lines = text.Split('\n', '\r');
            foreach(var line in lines)
            {
                _WriteLine(line);
            }
        }
    }
}
