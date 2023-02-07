using Godot;

public partial class AppState : Node
{
	public const string NodePath = "/root/AppState";
	
	public override void _Ready()
	{
		GetTree().AutoAcceptQuit = false;
	}
	
	public override void _Notification(long what)
	{
		switch(what)
		{
			case NotificationWmCloseRequest:
				GetTree().Quit();
				break;
		}	
	}
}
