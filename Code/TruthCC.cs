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

	private CameraComponent _camera;

	public enum EPlayerState
	{
		Waiting,
		Voting,
		Truthing
	}

	[Property, Sync] public EPlayerState PlayerState { get; set; } = EPlayerState.Waiting;
	
	[Property, Sync] public string VotingText { get; set; }
	
	[Property, Sync] public string TruthingText { get; set; }
	
	[Property, Sync] public string WaitingText { get; set; }

	protected override void OnStart()
	{
	}

	protected override void OnUpdate()
	{
		if ( IsProxy )
			return;
		
		Log.Info("Player State: " + PlayerState);
		
		if(_camera == null)
			_camera = Scene.Camera;
		
		if (Input.Keyboard.Pressed( "R" ))
		{
			IsReady = true;
			Log.Info("Pressed R! - " + Network.OwnerId);
		}
		
		DrawOnScreen();
	}

	private void DrawOnScreen()
	{
		if( _camera == null )
			Log.Error("Scene Camera never found!");

		float scale = _camera!.ScreenRect.Size.y / 1080f;

		using var painter = _camera.BeginHud();

		painter.TextStyle = new TextStyle()
		{
			FontName = "Poppins", FontSize = 32 * scale, Color = Color.White, Alignment = TextFlag.Center | TextFlag.DontClip,
		};
		
		string speakingText = PlayerState == EPlayerState.Truthing ? TruthingText : VotingText;
		speakingText = PlayerState == EPlayerState.Waiting ? WaitingText : speakingText;
		painter.Text("SPEAK:: " + speakingText, new Rect(50 * scale, 50 * scale, 400 * scale, 400 * scale)  );
	}
}

