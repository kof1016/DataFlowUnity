using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BadBird.DebugMenuUGUI
{
	public class DebugMsgText : MonoBehaviour
	{
		Text msgText;

		void Start()
		{
			msgText = GetComponent<Text>();
		}

		public void OnClick()
		{
			DebugMenu.instance.DisplayCompleteMsgPanel(msgText.text);
		}
	}
}