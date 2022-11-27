using Godot;

namespace MidnightChristmas.Engine; 

public partial class Interactable : Area3D {
	private Control InteractUI => GetNode<Control>("CanvasLayer/InteractUI");

	[Export] public bool Enabled = true;

	[Signal] public delegate void InteractedEventHandler(); 

	public override void _Process(double delta) {
		base._Process(delta);

		var interaction = GetTree().GetFirstNodeInGroup("interaction");
		var isLookedAt = OverlapsArea(interaction);
		InteractUI.Visible = isLookedAt && Enabled;

		if (isLookedAt && Input.IsActionJustPressed("interact") && Enabled) {
			EmitSignal("Interacted");
		}
	}
}