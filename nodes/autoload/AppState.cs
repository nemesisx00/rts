using Godot;

namespace Rts.Nodes.Autoload
{
	public partial class AppState : Node
	{
		public const string NodePath = "/root/AppState";
		
		public bool DebugMode { get; set; } = true;
		
		public override void _Ready()
		{
			GetTree().AutoAcceptQuit = false;
		}
		
		public override void _Notification(long what)
		{
			switch(what)
			{
				case NotificationWmCloseRequest:
					GetNode<SceneManager>(SceneManager.NodePath).freeStoredScene(true);
					if(DebugMode)
					{
						GD.Print("DEBUG Orphan Nodes ----- START");
						PrintOrphanNodes();
						GD.Print("DEBUG Orphan Nodes ----- END");
					}
					GetTree().Quit();
					break;
			}	
		}
	}
}
