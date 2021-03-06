﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// Informs server of inventory mangling
/// </summary>
public class InventoryInteractMessage : ClientMessage<InventoryInteractMessage>
{
	public byte Slot;
	public NetworkInstanceId Subject;

	public override IEnumerator Process()
	{
//		Debug.Log("Processed " + ToString());

		yield return WaitFor(Subject, SentBy);

		NetworkObjects[1].GetComponent<PlayerNetworkActions>().ValidateInvInteraction(decodeSlot(Slot), NetworkObjects[0]);
	}

	public static InventoryInteractMessage Send(GameObject subject, string hand)
	{
		var msg = new InventoryInteractMessage
		{
			Subject = subject.GetComponent<NetworkIdentity>().netId,
			Slot = encodeSlot(hand)
		};
		msg.Send();
		return msg;
	}

	private static byte encodeSlot(string slotEventString)
	{
		switch ( slotEventString )
		{
			case "leftHand": 	return 1;
			case "rightHand": 	return 2;
			case "suit": 		return 3;
			case "belt": 		return 4;
			case "feet": 		return 5;
			case "head": 		return 6;
			case "mask": 		return 7;
			case "uniform": 	return 8;
			case "neck": 		return 9;
			case "ear": 		return 10;
			case "eyes": 		return 11;
			case "hands": 		return 12;
			case "id": 			return 13;
			case "back": 		return 14;
			case "storage01": 	return 15;
			case "storage02": 	return 16;
			case "suitStorage": return 17;
		}
		return 0;
	}
	private static string decodeSlot(byte slotEventByte)
	{
		//we better start using enums for that soon!
		switch ( slotEventByte )
		{
			case 1: return "leftHand";
			case 2: return "rightHand";
			case 3:  return "suit";
			case 4:  return "belt";
			case 5:  return "feet";
			case 6:  return "head";
			case 7:  return "mask";
			case 8:  return "uniform";
			case 9:  return "neck";
			case 10: return "ear";
			case 11: return "eyes";
			case 12: return "hands";
			case 13: return "id";
			case 14: return "back";
			case 15: return "storage01";
			case 16: return "storage02";
			case 17: return "suitStorage";
			default: return null;
		}
	}

	public override string ToString()
	{
		return string.Format("[InventoryInteractMessage Subject={0} Slot={3} Type={1} SentBy={2}]",
			Subject, MessageType, SentBy, decodeSlot(Slot));
	}
	
	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		Slot = reader.ReadByte();
		Subject = reader.ReadNetworkId();

	}	
	public override void Serialize(NetworkWriter writer)
	{
		base.Serialize(writer);
		writer.Write(Slot);
		writer.Write(Subject);
	}
	
}
