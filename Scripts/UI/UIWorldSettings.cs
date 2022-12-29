namespace Project2D;

public partial class UIWorldSettings : Node
{
	[Export] public NodePath NodePathWorld { get; set; }

	[Export] public NodePath NodePathMoistureWet { get; set; }
	[Export] public NodePath NodePathMoistureDry { get; set; }
	[Export] public NodePath NodePathMoistureFrequency { get; set; }
	[Export] public NodePath NodePathSeed { get; set; }
	[Export] public NodePath NodePathTemperatureHot { get; set; }
	[Export] public NodePath NodePathTemperatureCold { get; set; }
	[Export] public NodePath NodePathTemperatureFrequency { get; set; }
	[Export] public NodePath NodePathUpdateOnEdit { get; set; }
	[Export] public NodePath NodePathChunkSize { get; set; }
	[Export] public NodePath NodePathSpawnSize { get; set; }

	private World World { get; set; }
	private bool UpdateOnEdit { get; set; }
	private WorldSettings WorldSettings { get; set; } = new();

	public override void _Ready()
	{
		World = GetNode<World>(NodePathWorld);

		// Set values from what is set in the UI
		WorldSettings.MoistureWetness = (float)GetNode<Slider>(NodePathMoistureWet).Value;
		WorldSettings.MoistureDryness = (float)GetNode<Slider>(NodePathMoistureDry).Value;
		WorldSettings.MoistureFrequency = (float)GetNode<Slider>(NodePathMoistureFrequency).Value;
		WorldSettings.TemperatureHot = (float)GetNode<Slider>(NodePathTemperatureHot).Value;
		WorldSettings.TemperatureCold = (float)GetNode<Slider>(NodePathTemperatureCold).Value;
		WorldSettings.TemperatureFrequency = (float)GetNode<Slider>(NodePathTemperatureFrequency).Value;
		UpdateOnEdit = GetNode<CheckBox>(NodePathUpdateOnEdit).ButtonPressed;
		WorldSettings.ChunkSize = int.Parse(GetNode<LineEdit>(NodePathChunkSize).Text);
		WorldSettings.Seed = GetNode<LineEdit>(NodePathSeed).Text;
		WorldSettings.SpawnSize = int.Parse(GetNode<LineEdit>(NodePathSpawnSize).Text);

		// Immediately generate the world
		World.Generate(WorldSettings);
	}

	private void _on_spawn_size_text_changed(string v)
	{
		if (!int.TryParse(v, out int num))
			return;

		WorldSettings.SpawnSize = num;
		UpdateWorldOnEdit();
	}

	private void _on_moisture_wetness_value_changed(float v)
	{
		WorldSettings.MoistureWetness = v;
		UpdateWorldOnEdit();
	}

	private void _on_moisture_dryness_value_changed(float v)
	{
		WorldSettings.MoistureDryness = v;
		UpdateWorldOnEdit();
	}

	private void _on_moisture_frequency_value_changed(float v)
	{
		WorldSettings.MoistureFrequency = v;
		UpdateWorldOnEdit();
	}

	private void _on_temperature_hot_value_changed(float v)
	{
		WorldSettings.TemperatureHot = v;
		UpdateWorldOnEdit();
	}

	private void _on_temperature_cold_value_changed(float v)
	{
		WorldSettings.TemperatureCold = v;
		UpdateWorldOnEdit();
	}

	private void _on_temperature_frequency_value_changed(float v)
	{
		WorldSettings.TemperatureFrequency = v;
		UpdateWorldOnEdit();
	}

	private void _on_seed_text_changed(string v)
	{
		WorldSettings.Seed = v;
		UpdateWorldOnEdit();
	}

	private void _on_chunk_size_text_changed(string v)
	{
		if (!int.TryParse(v, out int num))
			return;

		WorldSettings.ChunkSize = num;
		UpdateWorldOnEdit();
	}

	private void _on_update_on_edit_toggled(bool v) => UpdateOnEdit = v;
	private void _on_generate_pressed() => World.Generate(WorldSettings);

	private void UpdateWorldOnEdit()
	{
		if (UpdateOnEdit)
			World.Generate(WorldSettings);
	}
}

public class WorldSettings
{
	public int ChunkSize { get; set; }
	public int SpawnSize { get; set; }
	public float MoistureWetness { get; set; }
	public float MoistureDryness { get; set; }
	public float MoistureFrequency { get; set; }
	public float TemperatureHot { get; set; }
	public float TemperatureCold { get; set; }
	public float TemperatureFrequency { get; set; }
	public string Seed { get; set; }
}
