using Godot;

namespace MidnightChristmas.House.Cookie; 

public partial class Cookie : Node3D {
	public static int Count;
	public void _on_area_3d_body_entered(Node3D body) {
		Count++;
		QueueFree();
	}
}