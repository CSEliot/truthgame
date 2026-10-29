// Project: truth
// File:    GameManager.cs
// Author:  cseliot
// Created: 2026.10.29.07.10.14
// Edited: 2026.10.29.07.38.15
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

using System;

namespace Sandbox;

public sealed class GameManager : Component
{

	enum GameState
	{
		Waiting,
		PreRound,
		InRound,
		PostRound,
	}
	
	private GameState _gameState;
	
	private List<TruthCC> _Players =  new();
	private int currentTruthingPlayer = -1;
	// private Text
	
	protected override void OnStart()
	{
		_gameState = GameState.Waiting;
	}

	protected override void OnUpdate()
	{
		if ( _gameState == GameState.Waiting )
		{
			var allPlayers = Scene.FindAllWithTag( "Player" ).ToList();
			if ( allPlayers.Count > 1)
			{
				var everyoneReady = true;
				List<TruthCC> players = new ();
				foreach ( var player in allPlayers )
				{
					var playerComponent = player.GetComponent<TruthCC>();
					players.Add( playerComponent );
					if ( playerComponent.IsReady == false )
					{
						everyoneReady = false;
						break;
					}
				}

				if ( everyoneReady )
				{
					_Players = players;
					_gameState = GameState.PreRound;
				}
			}
		}

		if ( _gameState == GameState.PreRound )
		{
			if( Networking.IsHost )
			{
				currentTruthingPlayer++;
				var truthingPlayerID = _Players[currentTruthingPlayer].Network.OwnerId;
				SetModeInRoundHelper( truthingPlayerID );
			}
		}
	}

	[Rpc.Broadcast]
	private void SetModeInRoundHelper( Guid truthingPlayerID )
	{
		_gameState = GameState.InRound;
		foreach ( var player in _Players )
		{
			if ( player.Network.OwnerId == truthingPlayerID )
				player.PlayerState = TruthCC.EPlayerState.Truthing;
			else
				player.PlayerState = TruthCC.EPlayerState.Voting;
		}
	}
}

