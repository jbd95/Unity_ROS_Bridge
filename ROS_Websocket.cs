using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using SimpleJSON;


public class ROS_Websocket : MonoBehaviour {

	WebSocket ws;
	Vector3 new_pos = new Vector3 ();
	public GameObject Move;
	public string Message_OP;
	public string Message_Topic;
	public string Message_Type;

	// Use this for initialization
	void Start () {
		ROS_Message send_message = new ROS_Message ();
		send_message.op = Message_OP;
		send_message.topic = Message_Topic;
		send_message.type = Message_Type;

		ws = new WebSocket ("ws://192.168.1.2:9090");
		ws.Connect ();

		ws.Send(JsonUtility.ToJson(send_message));
		ws.OnMessage += (sender, e) => DecodeMessage (JSON.Parse(e.Data));



		/*new_pos.x = new_position ["msg"] ["x"];
		new_pos.y = new_position ["msg"] ["y"];
		new_pos.z = new_pos.z;*/
	}

	void OnApplicationQuit()
	{
		ws.Close ();
	}
	
	// Update is called once per frame
	void Update () {
		Move.transform.position = new_pos;
		Debug.Log (new_pos);
	}

	void DecodeMessage(JSONNode message)
	{
		
		new_pos.x = message["msg"]["pose"]["position"]["x"];
		new_pos.y = message["msg"]["pose"]["position"]["y"];
		new_pos.z = message["msg"]["pose"]["position"]["z"];
		//Debug.Log (message);

	}


	[Serializable]
	public class ROS_Message{
		public string op;
		public string topic;
		public string type;
	}

}



