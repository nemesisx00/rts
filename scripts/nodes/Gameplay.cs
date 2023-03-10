using Godot;
using System.Collections.Generic;
using Rts.Nodes.Autoload;

namespace Rts.Nodes
{
	public partial class Gameplay : Node2D
	{
		public bool Dragging { get; private set; } = false;
		public Vector2 DragStart { get; private set; } = Vector2.Zero;
		public List<ControllableUnit> SelectedUnits { get; set; } = new List<ControllableUnit>();
		
		private SelectionBox selectionBox;
		private Node2D map;
		
		public override void _Ready()
		{
			selectionBox = GetNode<SelectionBox>("%SelectionBox");
			map = GetNode<Node2D>("%Map");
			
			if(map.GetChildCount() < 1)
				loadLevel(SceneManager.Scenes.Level1);
			else
			{
				refreshSelectedUnits();
				refreshLevelSignals();
			}
		}
		
		public override void _UnhandledInput(InputEvent e)
		{
			if(e is InputEventKey iek)
			{
				processKeyboardPress(iek);
			}
			else if(e is InputEventMouseButton iemb)
			{
				switch(iemb.ButtonIndex)
				{
					case MouseButton.Left:
						processLeftClick(iemb);
						break;
					case MouseButton.Right:
						processRightClick(iemb);
						break;
				}	
			}
			else if(e is InputEventMouseMotion iemm)
			{
				selectionBox.Active = Dragging;
				if(Dragging)
					selectionBox.Rectangle = new Rect2(DragStart, iemm.Position - DragStart);
				selectionBox.QueueRedraw();
			}
		}
		
		public void refreshSelectedUnits()
		{
			SelectedUnits.Clear();
			foreach(var node in map.GetChild(0).GetChildren())
			{
				if(node is ControllableUnit unit && unit.Selected)
					SelectedUnits.Add(unit);
			}
		}
		
		public void refreshLevelSignals()
		{
			if(map.GetChild<LevelBase>(0) is LevelBase level)
			{
				level.LevelCompleted += handleLevelCompleted;
			}
		}
		
		private List<ControllableUnit> detectUnits(Vector2 start, Vector2 end)
		{
			var shape = PhysicsServer2D.RectangleShapeCreate();
			var size = (end - start) / 2;
			PhysicsServer2D.ShapeSetData(shape, size);
			
			var query = new PhysicsShapeQueryParameters2D();
			query.ShapeRid = shape;
			query.Transform = new Transform2D(0, (start + end) / 2);
			query.CollisionMask = 2;
			var collisions = GetWorld2d().DirectSpaceState.IntersectShape(query);
			
			PhysicsServer2D.FreeRid(shape);
			
			var units = new List<ControllableUnit>();
			foreach(var dict in collisions)
			{
				var collider = dict["collider"].As<ControllableUnit>();
				if(collider is ControllableUnit unit && !units.Contains(unit))
				{
					units.Add(unit);
				}
			}
			
			return units;
		}
		
		private void handleLevelCompleted()
		{
			if(map.GetChild(0) is Level1 level)
				loadLevel(SceneManager.Scenes.Level2);
			else
				GetNode<SceneManager>(SceneManager.NodePath).changeScene(SceneManager.Scenes.MainMenu);
		}
		
		private void loadLevel(string levelPath)
		{
			var resource = GD.Load<PackedScene>(levelPath);
			if(resource is PackedScene)
			{
				var instance = resource.Instantiate();
				if(instance is LevelBase level)
				{
					SelectedUnits.Clear();
					foreach(var node in map.GetChildren())
						node.QueueFree();
					
					level.LevelCompleted += handleLevelCompleted;
					map.CallDeferred("add_child", level);
				}
			}
		}
		
		private void processKeyboardPress(InputEventKey iek)
		{
			switch(iek.Keycode)
			{
				case Key.Escape:
					SelectedUnits.ForEach(u => u.deselect());
					SelectedUnits.Clear();
					break;
				case Key.F10:
					var sceneManager = GetNode<SceneManager>(SceneManager.NodePath);
					sceneManager.storeCurrentScene();
					sceneManager.changeScene(SceneManager.Scenes.MainMenu);
					break;
			}
		}
		
		private void processLeftClick(InputEventMouseButton iemb)
		{
			if(iemb.Pressed)
			{
				Dragging = true;
				DragStart = iemb.Position;
			}
			else if(Dragging)
			{
				Dragging = false;
				var units = detectUnits(DragStart, iemb.Position);
				if(units.Count > 0)
				{
					SelectedUnits.ForEach(u => u.deselect());
					SelectedUnits.Clear();
					SelectedUnits.AddRange(units);
					SelectedUnits.ForEach(u => u.select());
				}
			}
		}
		
		private void processRightClick(InputEventMouseButton iemb)
		{
			if(!iemb.Pressed)
			{
				SelectedUnits.ForEach(u => u.move(iemb.Position));
			}
		}
	}
}
