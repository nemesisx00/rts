using Godot;
using System;

namespace Rts.Nodes
{
	public partial class LevelBase : Node2D
	{
		[Signal]
		public delegate void LevelCompletedEventHandler();
	}
}
