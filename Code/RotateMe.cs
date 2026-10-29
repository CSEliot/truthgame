// Project: truth
// File:    RotateMe.cs
// Author:  cseliot
// Created: 2026.10.29.17.10.03
// Edited: 2026.10.29.17.33.03
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

public sealed class RotateMe : Component
{
	[Property] public float Speed { get; set; }
	[Property] public float Denominator { get; set; }

	protected override void OnStart()
	{

	}

	protected override void OnFixedUpdate()
	{
		LocalRotation *= Rotation.FromPitch( Speed / Denominator);
	}
}

