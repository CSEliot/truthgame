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
using System.Runtime.CompilerServices;

namespace Sandbox;

public sealed class GameManager : Component
{

	public enum GameState
	{
		Waiting,
		PreRound,
		InRound,
		PostRound,
	}
	
	public GameState _gameState;
	
	[Property] public int SEED;
	[Property] public GameObject Floor;
	[Property] public TextRenderer WorldText;

	private float _inRoundMaxTime = 34;
	private float _inRoundStartTime;
	
	// private Text
	
	protected override void OnStart()
	{
		
		_gameState = GameState.Waiting;
		Game.SetRandomSeed( SEED );
	}

	protected override void OnUpdate()
	{
		var truthCCs = Scene.GetAllComponents<TruthCC>().ToArray();

		if ( truthCCs.Length <= 1 )
			_gameState = GameState.Waiting;
		
		// Log.Info("Game State: " + _gameState);
		if ( _gameState == GameState.Waiting )
		{
			WorldText.Text = "Waiting for players ...";
			if ( truthCCs.Length > 1)
			{
				// Log.Info(truthCCs.Length + " player(s) found");
				var everyoneReady = true;
				foreach ( var truthCC in truthCCs )
				{
					if ( truthCC.IsReady == false )
					{
						everyoneReady = false;
						break;
					}
				}

				if ( everyoneReady )
				{
					_gameState = GameState.PreRound;
				}
			}
		}

		if ( _gameState == GameState.PreRound )
		{
			WorldText.Text = "Choosing Random Player ...";
			if( Networking.IsHost )
			{
				var randPlayer = Game.Random.Next( 0, truthCCs.Length ); 
				var truthingPlayerID = truthCCs.ElementAt(randPlayer).Network.OwnerId;
				HelperSetModeInRound( truthingPlayerID );
			}
		}
		
		if ( _gameState == GameState.InRound )
		{
			WorldText.Text = "" + (_inRoundStartTime + _inRoundMaxTime - Time.Now).FloorToInt();
			if( Networking.IsHost) 
			{
				if( truthCCs.Length >= 1 && truthCCs.All( t => t.IsAcceptingTruth || t.PlayerState == TruthCC.EPlayerState.Truthing ) )
					HelperSetModePostRound(truthCCs);
				else if ( Time.Now - _inRoundStartTime > _inRoundMaxTime )
					HelperKillTruthingPlayer(truthCCs);
			}
		}

	}
	[Rpc.Broadcast]
	private void HelperKillTruthingPlayer( TruthCC[] truthCCs )
	{
		foreach ( var truthCC in truthCCs )
		{
			if ( truthCC.PlayerState == TruthCC.EPlayerState.Truthing )
			{
				// if(truthCC.Network.IsOwner)
				// 	Floor.Enabled = false;
				truthCC.GameObject.Tags.Add("notsolid");
				truthCC.GameObject.GetComponent<TruthCC>().Destroy();
			}
		}
	}

	[Rpc.Broadcast]
	private void HelperSetModeInRound( Guid truthingPlayerID )
	{
		_gameState = GameState.InRound;
		_inRoundStartTime = Time.Now;
		var truthCCs = Scene.GetAllComponents<TruthCC>().ToArray();
		foreach ( var player in truthCCs )
		{
			if ( player.Network.OwnerId == truthingPlayerID )
				player.PlayerState = TruthCC.EPlayerState.Truthing;
			else
				player.PlayerState = TruthCC.EPlayerState.Voting;
		}
	}
	
	[Rpc.Broadcast]
	private void HelperSetModePostRound( TruthCC[] truthCCs)
	{
		_gameState = GameState.PreRound;
		Log.Info("IN POST ROUND!");
		foreach ( var player in truthCCs )
		{
			player.PlayerState = TruthCC.EPlayerState.Waiting;
			player.IsAcceptingTruth = false;
		}
	}
}

