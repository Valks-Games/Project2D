namespace Project2D;

public partial class UIWorldSettings : Node
{
	[Export] public NodePath NodePathWorld { get; set; }

	private World World { get; set; }

	private float HeatOffset { get; set; }
	private float MoistureOffset { get; set; }

	public override void _Ready()
	{
		World = GetNode<World>(NodePathWorld);
	}

	private void _on_moisture_value_changed(float value) => MoistureOffset = value;
	private void _on_temperature_value_changed(float value) => HeatOffset = value;
	private void _on_generate_pressed() => World.Generate(MoistureOffset, HeatOffset);
}
