// Project: truth
// File:    TruthCC.cs
// Author:  cseliot
// Created: 2026.10.29.07.10.28
// Edited: 2026.10.29.07.40.28
// 
// Copyright (c) 2026 KiteLion Games, LLC. All rights reserved.
// 
// This source code is the property of KiteLion Games, LLC and may not be
// copied, distributed, modified, or used in any way without prior written
// permission from KiteLion Games, LLC.
// 
// Description:
// [Provide a brief description of what this file/class does.]
// 
// License:
// This code is provided "as is," without warranty of any kind, express or
// implied, including but not limited to the warranties of merchantability,
// fitness for a particular purpose, and noninfringement. In no event shall
// the authors or copyright holders be liable for any claim, damages, or
// other liability, whether in an action of contract, tort, or otherwise,
// arising from, out of, or in connection with the software or the use or
// other dealings in the software.

namespace Sandbox;

public sealed class TruthCC : Component
{
	[Property, Sync] public bool IsReady { get; set; }

	public enum EPlayerState
	{
		Waiting,
		Voting,
		Truthing
	}

	[Property, Sync] public EPlayerState PlayerState { get; set; } = EPlayerState.Waiting;
	

	protected override void OnStart()
	{

	}

	protected override void OnUpdate()
	{
		if ( IsProxy )
			return;
		
		if (Input.Keyboard.Pressed( "R" ))
		{
			IsReady = true;
		}
	}
}

