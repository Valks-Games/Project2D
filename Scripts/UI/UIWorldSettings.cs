namespace Project2D;

public partial class UIWorldSettings : Node
{
	[Export] public NodePath NodePathWorld { get; set; }

	[Export] public NodePath NodePathMoistureIntensity { get; set; }
	[Export] public NodePath NodePathMoistureFrequency { get; set; }
	[Export] public NodePath NodePathMoistureSeed { get; set; }
	[Export] public NodePath NodePathTemperatureIntensity { get; set; }
	[Export] public NodePath NodePathTemperatureFrequency { get; set; }
	[Export] public NodePath NodePathTemperatureSeed { get; set; }
	[Export] public NodePath NodePathUpdateOnEdit { get; set; }
	[Export] public NodePath NodePathChunkSize { get; set; }

	private World World { get; set; }
	private bool UpdateOnEdit { get; set; }
	private WorldSettings WorldSettings { get; set; } = new();

	public override void _Ready()
	{
		World = GetNode<World>(NodePathWorld);

		// Set values from what is set in the UI
		WorldSettings.MoistureIntensity = (float)GetNode<Slider>(NodePathMoistureIntensity).Value;
		WorldSettings.MoistureFrequency = (float)GetNode<Slider>(NodePathMoistureFrequency).Value;
		WorldSettings.MoistureSeed = GetNode<LineEdit>(NodePathMoistureSeed).Text;
		WorldSettings.TemperatureIntensity = (float)GetNode<Slider>(NodePathTemperatureIntensity).Value;
		WorldSettings.TemperatureFrequency = (float)GetNode<Slider>(NodePathTemperatureFrequency).Value;
		WorldSettings.TemperatureSeed = GetNode<LineEdit>(NodePathTemperatureSeed).Text;
		UpdateOnEdit = GetNode<CheckBox>(NodePathUpdateOnEdit).ButtonPressed;
		WorldSettings.ChunkSize = int.Parse(GetNode<LineEdit>(NodePathChunkSize).Text);

		// Immediately generate the world
		World.Generate(WorldSettings);
	}

	private void _on_moisture_offset_value_changed(float v)
	{
		WorldSettings.MoistureIntensity = v;
		UpdateWorldOnEdit();
	}

	private void _on_moisture_frequency_value_changed(float v)
	{
		WorldSettings.MoistureFrequency = v;
		UpdateWorldOnEdit();
	}

	private void _on_moisture_seed_text_changed(string v)
	{
		WorldSettings.MoistureSeed = v;
		UpdateWorldOnEdit();
	}

	private void _on_temperature_offset_value_changed(float v)
	{
		WorldSettings.TemperatureIntensity = v;
		UpdateWorldOnEdit();
	}

	private void _on_temperature_frequency_value_changed(float v)
	{
		WorldSettings.TemperatureFrequency = v;
		UpdateWorldOnEdit();
	}

	private void _on_temperature_seed_text_changed(string v)
	{
		WorldSettings.TemperatureSeed = v;
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
	public float MoistureIntensity { get; set; }
	public float MoistureFrequency { get; set; }
	public string MoistureSeed { get; set; }
	public float TemperatureIntensity { get; set; }
	public float TemperatureFrequency { get; set; }
	public string TemperatureSeed { get; set; }
}
