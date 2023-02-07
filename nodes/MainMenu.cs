using Godot;
using Rts.Nodes.Autoload;

namespace Rts.Nodes
{
	public partial class MainMenu : Control
	{
		private SceneManager sceneManager;
		public override void _Ready()
		{
			sceneManager = GetNode<SceneManager>(SceneManager.NodePath);
			
			var resumeButton = GetNode<Button>("%Resume");
			resumeButton.Visible = sceneManager.HasStoredScene;
			resumeButton.Pressed += resumeHandler;
			
			GetNode<Button>("%StartGame").Pressed += startHandler;
			GetNode<Button>("%Quit").Pressed += quitHandler;
		}
		
		private void resumeHandler()
		{
			sceneManager.restoreStoredScene();
		}
		
		private void startHandler()
		{
			sceneManager.freeStoredScene();
			sceneManager.changeScene(SceneManager.Scenes.Gameplay);
		}
		
		private void quitHandler()
		{
			GetTree().Root.PropagateNotification((int)NotificationWmCloseRequest);
		}
	}
}
