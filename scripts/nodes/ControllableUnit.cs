using Godot;
using System;

namespace Rts.Nodes
{
	public partial class ControllableUnit : CharacterBody2D
	{
		private sealed class NodePaths
		{
			public const string SelectedOutline = "%SelectedOutline";
			public const string Sprite = "%Sprite";
			public const string Timer = "%Timer";
		}
		
		[Export]
		public string UnitName { get; set; }
		[Export]
		public Texture2D Sprite { get; set; }
		[Export(PropertyHint.Range, "0,10,or_greater")]
		public float MoveSpeed { get; set; } = 200.0f;
		[Export]
		public float PathProximity { get; set; } = 4.0f;
		[Export]
		public float TargetProximity { get; set; } = 4.0f;
		[Export]
		public uint StruggleTime { get; set; } = 5;
		
		public bool Selected { get { return GetNode<Sprite2D>(NodePaths.SelectedOutline).Visible; } }
		
		private Vector2 targetPosition = Vector2.Zero;
		private Sprite2D outline;
		private Timer timer;
		private NavigationAgent2D navAgent;
		
		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			
			if(!navAgent.IsTargetReached())
			{
				Velocity = (navAgent.GetNextLocation() - Position).Normalized() * MoveSpeed;
				MoveAndSlide();
				
				if(timer.IsStopped() && GetSlideCollisionCount() > 0)
					timer.Start();
			}
		}
		
		public override void _Ready()
		{
			base._Ready();
			
			targetPosition = Position;
			
			GetNode<Sprite2D>(NodePaths.Sprite).Texture = Sprite;
			
			timer = GetNode<Timer>(NodePaths.Timer);
			timer.WaitTime = StruggleTime;
			timer.Timeout += handleTimeout;
			
			navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
			navAgent.PathDesiredDistance = PathProximity;
			navAgent.TargetDesiredDistance = TargetProximity;
		}
		
		public void deselect()
		{
			GetNode<Sprite2D>(NodePaths.SelectedOutline).Visible = false;
		}
		
		private void handleTimeout()
		{
			if(GetSlideCollisionCount() > 0)
				navAgent.TargetLocation = Position;
		}
		
		public void move(Vector2 destination)
		{
			navAgent.TargetLocation = destination;
		}
		
		public void select()
		{
			GetNode<Sprite2D>(NodePaths.SelectedOutline).Visible = true;
		}
	}
}
