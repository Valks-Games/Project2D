using Newtonsoft.Json.Linq;

namespace Project2D;

public partial class UIWorldSettings : Node
{
	[Export] public NodePath NodePathWorld { get; set; }
	[Export] public NodePath NodePathVBox { get; set; }

	private World World { get; set; }
	private WorldSettings WorldSettings { get; set; } = new();

	public override void _Ready()
	{
		World = GetNode<World>(NodePathWorld);

		var parent = GetNode<VBoxContainer>(NodePathVBox);

		var settingsMoisture = CreateSettingsSection("Moisture", 
				new SettingsSlider
				{
					Name = "Dry",
					MaxValue = 1f,
					Step = 0.01f,
					Value = 0.5f
				},
				new SettingsSlider
				{
					Name = "Wet",
					MaxValue = 1f,
					Step = 0.01f
				},
				new SettingsSlider
				{
					Name = "Frequency",
					Value = 0.005f,
					MaxValue = 0.05f,
					Step = 0.001f
				},
				new SettingsSlider
				{
					Name = "Octaves",
					Value = 5,
					MinValue = 1,
					MaxValue = 10,
					Step = 1
				},
				new SettingsSlider
				{
					Name = "Strength",
					Value = 1f,
					MaxValue = 2f,
					Step = 0.01f
				}
			);

		var settingsMoistureDomainWarp = CreateSettingsSection("MoistureDomainWarp",
				new SettingsSlider
				{
					Name = "Amplitude",
					Value = 0,
					MaxValue = 75,
					Step = 0.01f
				});

		var settingsTemperature = CreateSettingsSection("Temperature",
				new SettingsSlider
				{
					Name = "Hot",
					MaxValue = 1f,
					Step = 0.01f
				},
				new SettingsSlider
				{
					Name = "Cold",
					MaxValue = 1f,
					Step = 0.01f
				},
				new SettingsSlider
				{
					Name = "Frequency",
					Value = 0.005f,
					MaxValue = 0.05f,
					Step = 0.001f
				},
				new SettingsSlider
				{
					Name = "Octaves",
					Value = 5,
					MinValue = 1,
					MaxValue = 10,
					Step = 1
				},
				new SettingsSlider
				{
					Name = "Strength",
					Value = 1f,
					MaxValue = 2f,
					Step = 0.01f
				}
			);

		var settingsTemperatureDomainWarp = CreateSettingsSection("TemperatureDomainWarp",
				new SettingsSlider
				{
					Name = "Amplitude",
					Value = 0,
					MaxValue = 75,
					Step = 0.01f
				});

		var settingsGeneral = CreateSettingsSection("", 
			new SettingsLineEdit
			{
				Name = "Seed",
				Value = "cat"
			},
			new SettingsCheckBox
			{
				Name = "UpdateOnEdit",
				Pressed = false
			},
			new SettingsLineEdit
			{
				Name = "ChunkSize",
				Value = "100"
			},
			new SettingsLineEdit
			{
				Name = "SpawnSize",
				Value = "4"
			}
		);

		parent.AddChild(settingsMoisture);
		parent.AddChild(settingsMoistureDomainWarp);
		parent.AddChild(settingsTemperature);
		parent.AddChild(settingsTemperatureDomainWarp);
		parent.AddChild(settingsGeneral);
		parent.AddChild(CreateButton("Generate"));

		// Immediately generate the world
		World.Generate(WorldSettings);
	}

	private PanelContainer CreateSettingsSection(string name, params Settings[] sliders)
	{
		var panelContainer = new PanelContainer();
		var marginContainer = new MarginContainer();
		var vbox = new VBoxContainer();

		if (!string.IsNullOrWhiteSpace(name))
			vbox.AddChild(new Label {
				Text = name.AddSpaceBeforeEachCapital(),
				HorizontalAlignment = HorizontalAlignment.Center
			});

		foreach (var direction in new string[] { "left", "right", "up", "down" })
			marginContainer.AddThemeConstantOverride($"margin_{direction}", 5);

		panelContainer.AddChild(marginContainer);
		marginContainer.AddChild(vbox);

		foreach (var element in sliders)
		{
			if (element is SettingsSlider slider)
				vbox.AddChild(CreateSlider(name, slider));

			if (element is SettingsLineEdit lineEdit)
				vbox.AddChild(CreateLineEdit(name, lineEdit));

			if (element is SettingsCheckBox checkBox)
				vbox.AddChild(CreateCheckbox(name, checkBox));
		}
			
		return panelContainer;
	}

	private HBoxContainer CreateSlider(string name, SettingsSlider settings)
	{
		var hbox = CreateHBox(settings.Name);
		var slider = new HSlider {
			MinValue = settings.MinValue,
			MaxValue = settings.MaxValue,
			Step = settings.Step,
			Value = settings.Value,
			SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
			SizeFlagsVertical = (int)Control.SizeFlags.Fill
		};

		WorldSettings.Values[name + settings.Name] = settings.Value;
		slider.ValueChanged += v =>
		{
			WorldSettings.Values[name + settings.Name] = (float)v;

			if ((bool)WorldSettings.Values["UpdateOnEdit"])
				World.Generate(WorldSettings);
		};

		hbox.AddChild(slider);

		return hbox;
	}

	private HBoxContainer CreateLineEdit(string name, SettingsLineEdit settings)
	{
		var hbox = CreateHBox(settings.Name);
		var lineEdit = new LineEdit
		{
			Text = settings.Value,
			SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
		};

		WorldSettings.Values[name + settings.Name] = settings.Value;
		lineEdit.TextChanged += v =>
		{
			WorldSettings.Values[name + settings.Name] = v;

			if ((bool)WorldSettings.Values["UpdateOnEdit"])
				World.Generate(WorldSettings);
		};

		hbox.AddChild(lineEdit);

		return hbox;
	}

	private HBoxContainer CreateCheckbox(string name, SettingsCheckBox settings)
	{
		var hbox = CreateHBox(settings.Name);
		var checkbox = new CheckBox {
			ButtonPressed = settings.Pressed
		};

		WorldSettings.Values[name + settings.Name] = settings.Pressed;
		checkbox.Toggled += v =>
		{
			WorldSettings.Values[name + settings.Name] = v;

			if ((bool)WorldSettings.Values["UpdateOnEdit"])
				World.Generate(WorldSettings);
		};

		hbox.AddChild(checkbox);

		return hbox;
	}

	private HBoxContainer CreateHBox(string name)
	{
		var hbox = new HBoxContainer();

		hbox.AddChild(new Label
		{
			CustomMinimumSize = new Vector2(90, 0),
			Text = name.AddSpaceBeforeEachCapital()
		});

		return hbox;
	}

	private Button CreateButton(string name)
	{
		var button = new Button();
		button.Text = name;
		button.Pressed += () => World.Generate(WorldSettings);

		return button;
	}
}

public class WorldSettings
{
	public Dictionary<string, object> Values { get; set; } = new();
}

public class Settings
{
	public string Name { get; set; }
}

public class SettingsSlider : Settings
{
	public float Value { get; set; }
	public float MinValue { get; set; }
	public float MaxValue { get; set; }
	public float Step { get; set; }
}

public class SettingsLineEdit : Settings
{
	public string Value { get; set; }
}

public class SettingsCheckBox : Settings
{
	public bool Pressed { get; set; }
}
