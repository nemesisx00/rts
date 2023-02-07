using Godot;
using System;

namespace Rts.Nodes
{
	public partial class ControllableUnit : CharacterBody2D
	{
		private sealed class NodePaths
		{
			public const string SelectedOutline = "%SelectedOutline";
			public const string Timer = "%Timer";
		}
		
		[Export]
		public string UnitName { get; set; }
		[Export(PropertyHint.Range, "0,10,or_greater")]
		public uint MoveSpeed { get; set; } = 2;
		[Export]
		public uint TargetProximity { get; set; } = 1;
		[Export]
		public uint StruggleTime { get; set; } = 5;
		
		public bool Selected { get { return GetNode<Sprite2D>(NodePaths.SelectedOutline).Visible; } }
		
		private Vector2 targetPosition = Vector2.Zero;
		private Sprite2D outline;
		private Timer timer;
		
		public override void _PhysicsProcess(double delta)
		{
			if(Position.DistanceTo(targetPosition) > TargetProximity)
			{
				Velocity = (targetPosition - Position) * MoveSpeed;
				MoveAndSlide();
				
				if(timer.IsStopped() && GetSlideCollisionCount() > 0)
				{
					timer.Start();
				}
			}
		}
		
		public override void _Ready()
		{
			targetPosition = Position;
			outline = GetNode<Sprite2D>(NodePaths.SelectedOutline);
			
			timer = GetNode<Timer>(NodePaths.Timer);
			timer.WaitTime = StruggleTime;
			timer.Timeout += handleTimeout;
		}
		
		public void deselect()
		{
			outline.Visible = false;
		}
		
		private void handleTimeout()
		{
			if(GetSlideCollisionCount() > 0)
				targetPosition = Position;
		}
		
		public void move(Vector2 destination)
		{
			targetPosition = destination;
		}
		
		public void select()
		{
			outline.Visible = true;
		}
	}
}
