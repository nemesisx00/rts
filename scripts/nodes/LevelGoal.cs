using Godot;
using System;

namespace Rts.Nodes
{
	public partial class LevelGoal : LevelBase
	{
		private Area2D goal;
		
		public override void _Ready()
		{
			goal = GetNode<Area2D>("%Goal");
			goal.BodyEntered += handleBodyEntered;
		}
		
		protected void handleBodyEntered(Node2D body)
		{
			if(body is ControllableUnit unit)
			{
				EmitSignal(nameof(LevelCompleted));
			}
		}
	}
}
