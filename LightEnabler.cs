using System.Collections.Generic;
using System.Linq;
using Godot;

namespace MidnightChristmas; 

public partial class LightEnabler : Node3D {
	[Export] public bool Enabled { get; set; } = true;
	
	public override void _Process(double delta) {
		var lights = GetLightsInScene()
			.OrderBy(l => l.GlobalPosition.DistanceTo(GlobalPosition))
			.ToList();

		const int max_lights = 7;
		var lightsOn = lights.Take(max_lights);
		foreach (var light in lightsOn) {
			light.Visible = true;
		}

		var lightsOff = lights.Skip(max_lights);
		foreach (var light in lightsOff) {
			light.Visible = !Enabled;
		}
	}

	private IEnumerable<Light3D> GetLightsInScene() {
		return GetTree().GetNodesInGroup("light").ToArray().Cast<Light3D>();
	}
}