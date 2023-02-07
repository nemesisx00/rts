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
		
		public bool Selected { get { return GetNode<Sprite2D>(NodePaths.SelectedOutline).Visible; } }
		
		private Sprite2D outline;
		
		public override void _Ready()
		{
			outline = GetNode<Sprite2D>(NodePaths.SelectedOutline);
		}
		
		public void deselect()
		{
			outline.Visible = false;
		}
		
		public void select()
		{
			outline.Visible = true;
		}
	}
}
