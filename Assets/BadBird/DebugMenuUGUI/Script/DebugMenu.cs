using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BadBird.DebugMenuUGUI
{
	public class DebugMenu : MonoBehaviour
	{
		public static DebugMenu instance;

		public GameObject msgText;
		public string debugVersionMsg = "";

//		Scrollbar scrollbar;
		RectTransform msgPanelContent;
		RectTransform completeMsgPanelRect;
		Toggle logToggle;
		Toggle warningToggle;
		Toggle errorToggle;
		Toggle otherToggle;
		Toggle collapseToggle;
		Toggle updateToggle;
		Canvas rootCanvas;
		Dictionary<string, int> collapseDictionary = new Dictionary<string, int>();
		Dictionary<string, GameObject> collapseDictionaryObj = new Dictionary<string, GameObject>();
		const float updateDelay = 1f;
		bool isWaitingRefresh = false;

        void Awake()
		{
#if FINAL_BUILD
			Destroy(gameObject);
#else
			instance = this;
			DontDestroyOnLoad(gameObject);
#endif
        }

        void Start ()
		{
			rootCanvas = GetComponent<Canvas>();
			msgPanelContent = transform.FindChild("MainPanel/MsgPanel/ScrollView/Viewport/Content").GetComponent<RectTransform>();
			logToggle = transform.FindChild("MainPanel/TopPanel/LogToggle").GetComponent<Toggle>();
			warningToggle = transform.FindChild("MainPanel/TopPanel/WarningToggle").GetComponent<Toggle>();
			errorToggle = transform.FindChild("MainPanel/TopPanel/ErrorToggle").GetComponent<Toggle>();
			otherToggle = transform.FindChild("MainPanel/TopPanel/OtherToggle").GetComponent<Toggle>();
			collapseToggle = transform.FindChild("MainPanel/BottomPanel/CollapseToggle").GetComponent<Toggle>();
			updateToggle = transform.FindChild("MainPanel/BottomPanel/UpdateToggle").GetComponent<Toggle>();
			completeMsgPanelRect = transform.FindChild("CompleteMsgPanel").GetComponent<RectTransform>();
//			scrollbar = transform.FindChild("MainPanel/MsgPanel/ScrollView/Scrollbar").GetComponent<Scrollbar>();

			logToggle.isOn = false;
			Close();

			DebugMsgPool.AddOther(debugVersionMsg);

			Input.simulateMouseWithTouches = true;
		}

        enum Direction
        {
            None,
            Up,
            Down,
            Left,
            Right
        }

        Direction HandDirection(Vector2 StartPos, Vector2 EndPos)
        {
            Direction mDirection = Direction.None;
            float threshold = 200;

            if (Mathf.Abs(StartPos.x - EndPos.x) > Mathf.Abs(StartPos.y - EndPos.y))
            {
                if (StartPos.x - EndPos.x >= threshold)
                {
                    //left
                    mDirection = Direction.Left;
                }
                if (StartPos.x - EndPos.x < -threshold)
                {
                    //right
                    mDirection = Direction.Right;
                }
            }
            else
            {
                if (StartPos.y - EndPos.y >= threshold)
                {
                    //down
                    mDirection = Direction.Down;
                }
                if (StartPos.y - EndPos.y < -threshold)
                {
                    //up
                    mDirection = Direction.Up;
                }
            }
            return mDirection;
        }


        void Update()
		{
			if(Input.GetKeyDown(KeyCode.Delete))
				Open();

			if(Input.touchCount == 3)
			{
//                 Direction mDirection = Direction.None;
//                 foreach (Touch t in Input.touches)
//                 {
//                     
//                     if ( t.phase == TouchPhase.Began)
//                     {
//                         //Debug.Log("[TestP]Began");
//                         m_screenPos = Input.touches[0].position;
//                     }
// 
//                     if((t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended))
//                     {
//                         Vector2 pos = new Vector2();
//                         //Debug.Log("[TestP]Ended");
//                         pos = Input.touches[0].position;
//                         mDirection = HandDirection(m_screenPos, pos);
//                         //Debug.Log("[TestP]mDirection: " + mDirection.ToString());
//                     }
//                 }
// 
//                 if(mDirection == Direction.Up)
//                 {
//                     Open();
//                 }
                
                bool match = true;
				bool end = false;
				foreach (Touch t in Input.touches)
				{
					match &= (t.position.y / Screen.height) < 1 / 6.0f;
					end |= (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended);
				}
				if (match && end)
					Open();
            }

			if(Input.GetKey(KeyCode.Insert))
			{
				Debug.LogWarning(System.DateTime.UtcNow.Ticks.ToString() + " <> " + Time.deltaTime);
			}
		}

		void OnEnable()
		{
			Application.logMessageReceived += HandleLog;
		}

		void OnDisable()
		{
			Application.logMessageReceived -= HandleLog;
		}

		public void DisplayCompleteMsgPanel(string msg)
		{
			completeMsgPanelRect.anchoredPosition = new Vector2(0f, 0f);
			completeMsgPanelRect.FindChild("MsgPanel/Text").GetComponent<Text>().text = msg;
		}

		public void CloseCompleteMsgPanel()
		{
			completeMsgPanelRect.anchoredPosition = new Vector2(99999f, 0f);
		}

		public void Clear()
		{
			DebugMsgPool.Clear();
			UpdateDebugConsole();
		}

		public void ClearMsgPanel()
		{
			collapseDictionary.Clear();
			collapseDictionaryObj.Clear();
			foreach(Transform child in msgPanelContent)
			{
				GameObject.Destroy(child.gameObject);
			}
		}

		public void Open()
		{
			#if !CUSTOM
			rootCanvas.enabled = true;
			InvokeUpdate();
			#endif
		}

		public void Close()
		{
			CloseCompleteMsgPanel();
			rootCanvas.enabled = false;
		}

		private void HandleLog(string logString, string stackTrace, LogType type)
		{
			string msg = logString + "\n" + stackTrace;
			switch(type)
			{
			case LogType.Error:
			case LogType.Exception:
				DebugMsgPool.AddError(msg);
				break;
			case LogType.Warning:
				DebugMsgPool.AddWarning(msg);
				break;
			case LogType.Log:
				DebugMsgPool.AddLog(msg);
				break;
			default:
				DebugMsgPool.AddOther(msg);
				break;
			}

			InvokeUpdate();
		}

		public void OnUpdateToggleChange()
		{
			if(updateToggle.isOn)
			{
				InvokeUpdate();
			}
		}

		public void OnToggleChange()
		{
			InvokeUpdate();
		}

		public void InvokeUpdate()
		{
			if(isWaitingRefresh == false)
			{
				isWaitingRefresh = true;
				Invoke("UpdateDebugConsole", updateDelay);
				Invoke("ResetIsWaitingRefresh", updateDelay);
			}
		}

		public void AppendText(string prefix, string msg)
		{
			string completeMsg = prefix + msg;
			GameObject msgTextClone = null;

			if(collapseToggle.isOn)
			{
				if(collapseDictionary.ContainsKey(completeMsg))
				{
					collapseDictionary[completeMsg] += 1;
					msgTextClone = collapseDictionaryObj[completeMsg];
					msgTextClone.GetComponent<Text>().text = "(" + collapseDictionary[completeMsg] + ")" + completeMsg;
				}
				else
				{
					collapseDictionary.Add(completeMsg, 1);
					msgTextClone = AddNewMsgText(completeMsg);
					collapseDictionaryObj.Add(completeMsg, msgTextClone);
				}
			}
			else
			{
				AddNewMsgText(completeMsg);
			}
		}

		GameObject AddNewMsgText(string msg)
		{
			GameObject msgTextClone = Instantiate<GameObject>(msgText);
			msgTextClone.transform.SetParent(msgPanelContent);
			
			Text msgTxt = msgTextClone.GetComponent<Text>();
			msgTxt.text = msg;
			
			RectTransform rectTransform = msgTextClone.GetComponent<RectTransform>();
			rectTransform.localScale = Vector3.one;

			return msgTextClone;
		}

		public void UpdateDebugConsole()
		{
			if(rootCanvas.enabled && updateToggle.isOn)
			{
				ClearMsgPanel();

				var i = 0;

				if(logToggle.isOn)
					for(i = 0; i < DebugMsgPool.LogPool.Count; i++)
						AppendText("[Log]:", DebugMsgPool.LogPool[i]);

				if(warningToggle.isOn)
					for(i = 0; i < DebugMsgPool.WarningPool.Count; i++)
						AppendText("<color=#ffa500ff>[Warning]:</color>", DebugMsgPool.WarningPool[i]);

				if(errorToggle.isOn)
					for(i = 0; i < DebugMsgPool.ErrorPool.Count; i++)
						AppendText("<color=#ff0000ff>[Error]:</color>", DebugMsgPool.ErrorPool[i]);

				if(otherToggle.isOn)
					for(i = 0; i < DebugMsgPool.OtherPool.Count; i++)
						AppendText("<color=#000080ff>[Other]:</color>", DebugMsgPool.OtherPool[i]);
			}
		}

		public void Test()
		{
			Debug.LogError("test");
			Debug.LogWarning("test");
			Debug.Log(123);
			Debug.Log("abc");
			int value = 0;
			Assert.AreNotEqual<int>(0, value, "assert message");
		}

		void ResetIsWaitingRefresh()
		{
			isWaitingRefresh = false;
		}
	}
}
