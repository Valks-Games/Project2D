using System.Xml.Linq;

namespace Project2D;

public class UINoiseSettings
{
	public PanelContainer Panel { get; set; }
	private VBoxContainer VBox { get; set; }
	private FastNoiseLite Noise { get; set; } = new();

	public UINoiseSettings(string name)
	{
		Panel = new PanelContainer();
		var marginContainer = new MarginContainer();
		VBox = new VBoxContainer();

		if (!string.IsNullOrWhiteSpace(name))
			VBox.AddChild(new Label {
				Text = name.AddSpaceBeforeEachCapital(),
				HorizontalAlignment = HorizontalAlignment.Center
			});

		foreach (var direction in new string[] { "left", "right", "up", "down" })
			marginContainer.AddThemeConstantOverride($"margin_{direction}", 5);

		Panel.AddChild(marginContainer);
		marginContainer.AddChild(VBox);

		var textureRect = new TextureRect();
		var noiseTexure = new NoiseTexture2D();
		noiseTexure.Noise = Noise;
		noiseTexure.Width = 400;
		noiseTexure.Height = 200;
		textureRect.Texture = noiseTexure;
		textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;

		VBox.AddChild(textureRect);

		HBox("Noise Type", OptionButton(Enum.GetValues(typeof(FastNoiseLite.NoiseTypeEnum)), (item) => 
			Noise.NoiseType = (FastNoiseLite.NoiseTypeEnum)item));

		HBox("Seed", LineEdit((text) => 
		{
			var seed = 0;

			foreach (var c in text)
				seed += c;

			Noise.Seed = seed;
		}));

		HBox("Frequency", HSlider(new SettingsSlider { 
			MinValue = 0.001f,
			MaxValue = 0.05f,
			Step = 0.001f
		}, (v) => Noise.Frequency = (float)v));

		VBox.AddChild(new Label { Text = "Offset" });

		var hbox = new HBoxContainer();

		var offsetRange = 1000;

		hbox.AddChild(HSlider(new SettingsSlider { 
			MinValue = -offsetRange,
			MaxValue = offsetRange,
			Step = 0.001f
		}, (v) => {
			Noise.Offset = new Vector3((float)v, Noise.Offset.y, Noise.Offset.z);
		}));

		hbox.AddChild(HSlider(new SettingsSlider { 
			MinValue = -offsetRange,
			MaxValue = offsetRange,
			Step = 0.001f
		}, (v) => {
			Noise.Offset = new Vector3(Noise.Offset.x, (float)v, Noise.Offset.z);
		}));

		VBox.AddChild(hbox);

		VBox.AddChild(new Label {
			Text = "Fractal",
			HorizontalAlignment = HorizontalAlignment.Center
		});

		HBox("Type", OptionButton(Enum.GetValues(typeof(FastNoiseLite.FractalTypeEnum)), (item) => 
		{
			Noise.FractalType = (FastNoiseLite.FractalTypeEnum)item;
		}));

		HBox("Octaves", HSlider(new SettingsSlider
		{
			MinValue = 1,
			MaxValue = 10,
			Value = 5,
			Step = 1
		}, (v) =>
		{
			Noise.FractalOctaves = (int)v;
		}));

		HBox("Lacunarity", HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 10,
			Value = 2,
			Step = 0.001f
		}, (v) =>
		{
			Noise.FractalLacunarity = (float)v;
		}));

		HBox("Gain", HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 10,
			Value = 0.5f,
			Step = 0.001f
		}, (v) =>
		{
			Noise.FractalGain = (float)v;
		}));

		HBox("Weighted Strength", HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 1,
			Value = 0,
			Step = 0.001f
		}, (v) =>
		{
			Noise.FractalWeightedStrength = (float)v;
		}));

		VBox.AddChild(new Label {
			Text = "Domain Warp",
			HorizontalAlignment = HorizontalAlignment.Center
		});

		var checkbox = new CheckBox();
		checkbox.Toggled += (v) => Noise.DomainWarpEnabled = v;
		HBox("Enabled", checkbox);

		HBox("Type", OptionButton(Enum.GetValues(typeof(FastNoiseLite.DomainWarpTypeEnum)), (item) => 
		{
			Noise.DomainWarpType = (FastNoiseLite.DomainWarpTypeEnum)item;
		}));

		HBox("Amplitude", HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 100,
			Value = 0,
			Step = 0.001f
		}, (v) =>
		{
			Noise.DomainWarpAmplitude = (float)v;
		}));

		HBox("Frequency", HSlider(new SettingsSlider { 
			MinValue = 0,
			MaxValue = 1,
			Step = 0.001f
		}, (v) =>
		{
			Noise.DomainWarpFrequency = (float)v;
		}));

		HBox("Fractal Type", OptionButton(Enum.GetValues(typeof(FastNoiseLite.DomainWarpFractalTypeEnum)), (item) => 
		{
			Noise.DomainWarpFractalType = (FastNoiseLite.DomainWarpFractalTypeEnum)item;
		}));

		HBox("Fractal Octaves", HSlider(new SettingsSlider
		{
			MinValue = 1,
			MaxValue = 10,
			Value = 5,
			Step = 1
		}, (v) =>
		{
			Noise.DomainWarpFractalOctaves = (int)v;
		}));

		HBox("Fractal Lacunarity", HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 10,
			Value = 2,
			Step = 0.001f
		}, (v) =>
		{
			Noise.DomainWarpFractalLacunarity = (float)v;
		}));

		HBox("Fractal Gain", HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 10,
			Value = 0.5f,
			Step = 0.001f
		}, (v) =>
		{
			Noise.DomainWarpFractalGain = (float)v;
		}));
	}

	private void HBox(string text, Node child2)
	{
		var hbox = new HBoxContainer();
		hbox.AddChild(new Label { 
			CustomMinimumSize = new Vector2(150, 0),
			Text = text 
		});
		hbox.AddChild(child2);
		VBox.AddChild(hbox);
	}

	private HSlider HSlider(SettingsSlider settings, Action<double> valueChanged)
	{
		var slider = new HSlider
		{
			MinValue = settings.MinValue,
			MaxValue = settings.MaxValue,
			Step = settings.Step,
			Value = settings.Value,
			SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
			SizeFlagsVertical = (int)Control.SizeFlags.Fill
		};

		slider.ValueChanged += (value) => valueChanged(value);

		return slider;
	}

	private LineEdit LineEdit(Action<string> textChanged)
	{
		var lineEdit = new LineEdit {
			SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
		};

		lineEdit.TextChanged += (text) => textChanged(text);

		return lineEdit;
	}

	private OptionButton OptionButton(Array items, Action<long> itemSelected)
	{
		var optionButton = new OptionButton();
		optionButton.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;

		foreach (var item in items)
			optionButton.AddItem(item + "");

		optionButton.ItemSelected += (item) => itemSelected(item);

		return optionButton;
	}
}
