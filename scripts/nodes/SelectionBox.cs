using Godot;
using System;

public partial class SelectionBox : Node2D
{
	public bool Active { get; set; } = false;
	[Export]
	public Color Color { get; set; } = new Color(0, 1, 0);
	public Rect2 Rectangle { get; set; }
	
	public override void _Draw()
	{
		if(Active)
			DrawRect(Rectangle, Color, false);
	}
}
