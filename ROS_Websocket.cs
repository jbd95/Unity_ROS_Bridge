/*      
 *      Unity_ROS_Bridge.cs
 *      This project creates a Websocket connection between Unity and ROS.
 *      Heracleia Lab 2017-2018
 *      Author: James Brady
 *      
 */

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
    public string IP;
    public string Port = "9090";

	// Use this for initialization
	void Start () {
        // send a message to ROS with the op, topic, and type
		ROS_Message send_message = new ROS_Message (Message_OP, Message_Topic, Message_Type);

        // initialize the websocket connection to the instance of ROS
		ws = new WebSocket ("ws://" + IP + ":" + Port);
		ws.Connect ();

        // send the ROS message
		ws.Send(JsonUtility.ToJson(send_message));

        // set the message recieved callback
		ws.OnMessage += (sender, e) => DecodeMessage (JSON.Parse(e.Data));
	}

    // close the websocket when the unity application closes
	void OnApplicationQuit()
	{
		ws.Close();
	}
	
	// Update is called once per frame
	void Update () {
		Move.transform.position = new_pos;
		Debug.Log (new_pos);
	}

    /// <summary>
    /// Uses a JSON parser to decode the message recieved from ROS
    /// </summary>
    /// <param name="message"></param>
	void DecodeMessage(JSONNode message)
	{
        // this is a sample decoding function used to decode coordinates
		new_pos.x = message["msg"]["pose"]["position"]["x"];
		new_pos.y = message["msg"]["pose"]["position"]["y"];
		new_pos.z = message["msg"]["pose"]["position"]["z"];
	}

    /// <summary>
    /// This class is used to send a message to ROS
    /// Parameters:
    ///     _op - the action to take with the message; i.e. "subscribe"
    ///     _topic - the topic of the ROS message
    ///     _type - the data type of the ROS message
    /// </summary>
	[Serializable]
	public class ROS_Message{

        string op;
		string topic;
		string type;

        public ROS_Message(string _op, string _topic, string _type)
        {
            op = _op;
            topic = _topic;
            type = _type;
        }

        public string GetOP()
        {
            return op;
        }
        public string GetTopic()
        {
            return topic;
        }
        public string GetType()
        {
            return type;
        }
	}

}



