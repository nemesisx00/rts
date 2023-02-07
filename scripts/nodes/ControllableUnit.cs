using Godot;
using System;

namespace Rts.Nodes
{
	public partial class ControllableUnit : CharacterBody2D
	{
		private sealed class NodePaths
		{
			public const string SelectedOutline = "%SelectedOutline";
		}
		
		[Export]
		public string UnitName { get; set; }
		[Export(PropertyHint.Range, "0,10,or_greater")]
		public uint MoveSpeed { get; set; } = 2;
		public bool Moving { get; private set; } = false;
		
		public bool Selected { get { return GetNode<Sprite2D>(NodePaths.SelectedOutline).Visible; } }
		
		private Vector2 targetPosition = Vector2.Zero;
		private Sprite2D outline;
		
		public override void _Process(double delta)
		{
			if(Position.Equals(targetPosition))
				Moving = false;
			
			if(Moving)
			{
				Velocity = (targetPosition - Position) * MoveSpeed;
				MoveAndSlide();
			}
		}
		
		public override void _Ready()
		{
			outline = GetNode<Sprite2D>(NodePaths.SelectedOutline);
		}
		
		public void deselect()
		{
			outline.Visible = false;
		}
		
		public void move(Vector2 destination)
		{
			targetPosition = destination;
			Moving = true;
		}
		
		public void select()
		{
			outline.Visible = true;
		}
	}
}
