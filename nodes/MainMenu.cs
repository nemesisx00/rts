using Godot;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		GetNode<Button>("%StartGame").Pressed += startHandler;
		GetNode<Button>("%Quit").Pressed += quitHandler;
	}
	
	private void startHandler()
	{
		GetNode<SceneManager>(SceneManager.NodePath).changeScene(SceneManager.Scenes.Gameplay);
	}
	
	private void quitHandler()
	{
		GetTree().Root.PropagateNotification((int)NotificationWmCloseRequest);
	}
}
