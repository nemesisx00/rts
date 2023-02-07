using Godot;

public partial class SceneManager : Node
{
	public sealed class Scenes
	{
		public const string MainMenu = "res://MainMenu.tscn";
		public const string Gameplay = "res://Gameplay.tscn";
	}
	
	public const string NodePath = "/root/SceneManager";
	
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
		if(storedScene is Node)
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
		if(storedScene is Node)
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
