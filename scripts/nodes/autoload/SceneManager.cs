using Godot;

namespace Rts.Nodes.Autoload
{
	public partial class SceneManager : Node
	{
		public sealed class Scenes
		{
			public const string MainMenu = "res://scenes/MainMenu.tscn";
			public const string Gameplay = "res://scenes/Gameplay.tscn";
			public const string Level1 = "res://scenes/Level1.tscn";
			public const string Level2 = "res://scenes/Level2.tscn";
		}
		
		public const string NodePath = "/root/SceneManager";
		
		public bool HasStoredScene { get { return storedScene is Node; } }
		
		private Node storedScene = null;
		
		public void changeScene(string resourcePath)
		{
			var resource = GD.Load<PackedScene>(resourcePath);
			if(resource is PackedScene)
			{
				var newScene = resource.InstantiateOrNull<Node>();
				if(newScene is Node)
				{
					var tree = GetTree();
					tree.CurrentScene.QueueFree();
					tree.Root.AddChild(newScene);
					tree.CurrentScene = newScene;
				}
			}
		}
		
		public void freeStoredScene(bool force = false)
		{
			if(HasStoredScene)
			{
				if(force)
					storedScene.Free();
				else
					storedScene.QueueFree();
				storedScene = null;
			}
		}
		
		public void restoreStoredScene()
		{
			if(HasStoredScene)
			{
				var tree = GetTree();
				tree.CurrentScene.QueueFree();
				tree.Root.AddChild(storedScene);
				tree.CurrentScene = storedScene;
				storedScene = null;
			}
		}
		
		public void storeCurrentScene()
		{
			freeStoredScene();
			storedScene = GetTree().CurrentScene.Duplicate();
		}
	}
}
