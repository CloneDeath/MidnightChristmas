using Godot;
using MidnightChristmas.Engine;

namespace MidnightChristmas.House.Tree; 

public partial class Tree : Node3D {
	private Node3D present => GetNode<Node3D>("Present");
	private Interactable interactable => GetNode<Interactable>("Interactable");
	public void _on_interactable_interacted() {
		present.Visible = true;
		interactable.Enabled = false;
	}
}