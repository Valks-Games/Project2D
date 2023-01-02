using FNL     = Godot.FastNoiseLite;
using FNLFT   = Godot.FastNoiseLite.FractalTypeEnum;
using FNLDWT  = Godot.FastNoiseLite.DomainWarpTypeEnum;
using FNLDWFT = Godot.FastNoiseLite.DomainWarpFractalTypeEnum;
using FNLCDF  = Godot.FastNoiseLite.CellularDistanceFunctionEnum;
using FNLCRT  = Godot.FastNoiseLite.CellularReturnTypeEnum;

namespace Project2D;

public class UINoiseSettings
{
	// Settings
	public Settings           SettingsNoise      { get; set; } = new();
	public FractalSettings    SettingsFractal    { get; set; } = new();
	public DomainWarpSettings SettingsDomainWarp { get; set; } = new();
	public CellularSettings   SettingsCellular   { get; set; } = new();

	public  PanelContainer Panel { get; set; }
	private VBoxContainer  VBox  { get; set; }
	private FNL            Noise { get; set; } = new();

	private TextureRect   ControlPreview                     { get; set; }
	private HBoxContainer ControlNoiseType                   { get; set; }
	private HBoxContainer ControlSeed                        { get; set; }
	private HBoxContainer ControlFrequency                   { get; set; }
	private Label         ControlLabelOffset                 { get; set; }
	private HBoxContainer ControlOffset                      { get; set; }
	private Label         ControlLabelFractal                { get; set; }
	private HBoxContainer ControlFractalType                 { get; set; }
	private HBoxContainer ControlFractalOctaves              { get; set; }
	private HBoxContainer ControlFractalLacunarity           { get; set; }
	private HBoxContainer ControlFractalGain                 { get; set; }
	private HBoxContainer ControlFractalWeightedStrength     { get; set; }
	private Label         ControlDomainWarpLabel             { get; set; }
	private HBoxContainer ControlDomainEnabled               { get; set; }
	private HBoxContainer ControlDomainWarpType              { get; set; }
	private HBoxContainer ControlDomainWarpAmplitude         { get; set; }
	private HBoxContainer ControlDomainWarpFrequency         { get; set; }
	private HBoxContainer ControlDomainWarpFractalType       { get; set; }
	private HBoxContainer ControlDomainWarpFractalOctaves    { get; set; }
	private HBoxContainer ControlDomainWarpFractalLacunarity { get; set; }
	private HBoxContainer ControlDomainWarpFractalGain       { get; set; }

	private void PrepareNoise()
	{
		Noise.NoiseType                   = SettingsNoise.NoiseType;
		Noise.Seed                        = SettingsNoise.Seed;
		Noise.Frequency                   = SettingsNoise.Frequency;
		Noise.Offset                      = new Vector3(SettingsNoise.Offset.x, SettingsNoise.Offset.y, Noise.Offset.z);
		Noise.FractalType                 = SettingsFractal.Type;
		Noise.FractalOctaves              = SettingsFractal.Octaves;
		Noise.FractalLacunarity           = SettingsFractal.Lacunarity;
		Noise.FractalGain                 = SettingsFractal.Gain;
		Noise.FractalWeightedStrength     = SettingsFractal.WeightedStrength;
		Noise.DomainWarpEnabled           = SettingsDomainWarp.Enabled;
		Noise.DomainWarpType              = SettingsDomainWarp.Type;
		Noise.DomainWarpAmplitude         = SettingsDomainWarp.Amplitude;
		Noise.DomainWarpFrequency         = SettingsDomainWarp.Frequency;
		Noise.DomainWarpFractalType       = SettingsDomainWarp.FractalType;
		Noise.DomainWarpFractalOctaves    = SettingsDomainWarp.FractalOctaves;
		Noise.DomainWarpFractalLacunarity = SettingsDomainWarp.FractalLacunarity;
		Noise.DomainWarpFractalGain       = SettingsDomainWarp.FractalGain;
		Noise.CellularDistanceFunction    = SettingsCellular.DistanceFunction;
		Noise.CellularJitter              = SettingsCellular.Jitter;
		Noise.CellularReturnType          = SettingsCellular.ReturnTypeFunction;
	}

	public void HideDomainWarp()
	{

	}

	public PanelContainer Create(string name)
	{
		PrepareNoise();

		// Prepare PanelContainer
		Panel = new PanelContainer();
		var marginContainer = new MarginContainer();
		VBox = new VBoxContainer();

		foreach (var direction in new string[] { "left", "right", "up", "down" })
			marginContainer.AddThemeConstantOverride($"margin_{direction}", 5);

		Panel.AddChild(marginContainer);
		marginContainer.AddChild(VBox);

		// SETTINGS
		CreateUI(name);

		return Panel;
	}

	private void CreateUI(string name)
	{
		if (!string.IsNullOrWhiteSpace(name))
			VBox.AddChild(new Label {
				Text = name.AddSpaceBeforeEachCapital(),
				HorizontalAlignment = HorizontalAlignment.Center
			});

		CreateSettings();
		CreateFractalSettings();
		CreateCellularSettings();
		CreateDomainWarpSettings();
	}

	private void CreateSettings()
	{
		CreatePreview();
		CreateNoiseType();
		CreateSeed();
		CreateFrequency();
		CreateOffset();
	}

	private void CreatePreview()
	{
		// Preview
		ControlPreview = new TextureRect();
		var noiseTexure = new NoiseTexture2D();
		noiseTexure.Noise = Noise;
		noiseTexure.Width = 400;
		noiseTexure.Height = 200;
		ControlPreview.Texture = noiseTexure;
		ControlPreview.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;

		VBox.AddChild(ControlPreview);
	}

	private void CreateNoiseType()
	{
		// Noise Type
		var optionButtonNoiseType = OptionButton(Enum.GetValues(typeof(FNL.NoiseTypeEnum)), (item) => 
			Noise.NoiseType = (FNL.NoiseTypeEnum)item);

		optionButtonNoiseType.Selected = (int)SettingsNoise.NoiseType;

		ControlNoiseType = HBox("Noise Type", optionButtonNoiseType);

		VBox.AddChild(ControlNoiseType);
	}

	private void CreateSeed()
	{
		// Seed
		var lineEditSeed = LineEdit((text) => 
		{
			var seed = 0;

			foreach (var c in text)
				seed += c;

			Noise.Seed = seed;
		});

		lineEditSeed.Text = $"{SettingsNoise.Seed}";

		ControlSeed = HBox("Seed", lineEditSeed);

		VBox.AddChild(ControlSeed);
	}

	private void CreateFrequency()
	{
		// Frequency
		var hsliderFrequency = HSlider(new SettingsSlider { 
			MinValue = 0.001f,
			MaxValue = 0.05f,
			Step = 0.001f
		}, (v) => Noise.Frequency = (float)v);

		hsliderFrequency.Value = SettingsNoise.Frequency;

		ControlFrequency = HBox("Frequency", hsliderFrequency);

		VBox.AddChild(ControlFrequency);
	}

	private void CreateOffset()
	{
		// Offset Label
		ControlLabelOffset = new Label { Text = "Offset" };

		VBox.AddChild(ControlLabelOffset);

		// Offset
		ControlOffset = new HBoxContainer();

		var offsetRange = 1000;

		// OffsetX
		var hsliderOffsetX = HSlider(new SettingsSlider { 
			MinValue = -offsetRange,
			MaxValue = offsetRange,
			Step = 0.001f
		}, (v) => {
			Noise.Offset = new Vector3((float)v, Noise.Offset.y, Noise.Offset.z);
		});

		hsliderOffsetX.Value = SettingsNoise.Offset.x;

		ControlOffset.AddChild(hsliderOffsetX);

		// OffsetY
		var hsliderOffsetY = HSlider(new SettingsSlider { 
			MinValue = -offsetRange,
			MaxValue = offsetRange,
			Step = 0.001f
		}, (v) => {
			Noise.Offset = new Vector3(Noise.Offset.x, (float)v, Noise.Offset.z);
		});

		hsliderOffsetY.Value = SettingsNoise.Offset.y;

		ControlOffset.AddChild(hsliderOffsetY);

		VBox.AddChild(ControlOffset);
	}

	private void CreateFractalSettings()
	{
		// Fractal Label
		ControlLabelFractal = new Label {
			Text = "Fractal",
			HorizontalAlignment = HorizontalAlignment.Center
		};

		VBox.AddChild(ControlLabelFractal);

		CreateFractalType();
		CreateFractalOctaves();
		CreateFractalLacunarity();
		CreateFractalGain();
		CreateFractalWeightedStrength();
	}

	private void CreateFractalType()
	{
		// Fractal Type
		var optionButtonFractalType = OptionButton(Enum.GetValues(typeof(FNLFT)), (item) => 
		{
			Noise.FractalType = (FNLFT)item;
		});

		optionButtonFractalType.Selected = (int)SettingsFractal.Type;

		ControlFractalType = HBox("Type", optionButtonFractalType);

		VBox.AddChild(ControlFractalType);
	}

	private void CreateFractalOctaves()
	{
		// Fractal Octaves
		var hsliderFractalOctaves = HSlider(new SettingsSlider
		{
			MinValue = 1,
			MaxValue = 10,
			Value = 5,
			Step = 1
		}, (v) =>
		{
			Noise.FractalOctaves = (int)v;
		});

		hsliderFractalOctaves.Value = SettingsFractal.Octaves;

		ControlFractalOctaves = HBox("Octaves", hsliderFractalOctaves);

		VBox.AddChild(ControlFractalOctaves);
	}

	private void CreateFractalLacunarity()
	{
		// Fractal Lacunarity
		var hsliderFractalLacunarity = HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 10,
			Value = 2,
			Step = 0.001f
		}, (v) =>
		{
			Noise.FractalLacunarity = (float)v;
		});

		hsliderFractalLacunarity.Value = SettingsFractal.Lacunarity;

		ControlFractalLacunarity = HBox("Lacunarity", hsliderFractalLacunarity);

		VBox.AddChild(ControlFractalLacunarity);
	}

	private void CreateFractalGain()
	{
		// Fractal Gain
		var hsliderFractalGain = HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 10,
			Value = 0.5f,
			Step = 0.001f
		}, (v) =>
		{
			Noise.FractalGain = (float)v;
		});

		hsliderFractalGain.Value = SettingsFractal.Gain;

		ControlFractalGain = HBox("Gain", hsliderFractalGain);

		VBox.AddChild(ControlFractalGain);
	}

	private void CreateFractalWeightedStrength()
	{
		// Fractal Weighted Strength
		var hsliderFractalWeightedStrength = HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 1,
			Value = 0,
			Step = 0.001f
		}, (v) =>
		{
			Noise.FractalWeightedStrength = (float)v;
		});

		hsliderFractalWeightedStrength.Value = SettingsFractal.WeightedStrength;

		ControlFractalWeightedStrength = HBox("Weighted Strength", hsliderFractalWeightedStrength);

		VBox.AddChild(ControlFractalWeightedStrength);
	}

	private void CreateCellularSettings()
	{
		CreateCellularDistanceFunction();
		CreateCellularJitter();
		CreateCellularReturnTypeFunction();
	}

	private void CreateCellularDistanceFunction()
	{

	}

	private void CreateCellularJitter()
	{

	}

	private void CreateCellularReturnTypeFunction()
	{

	}

	private void CreateDomainWarpSettings()
	{
		// Domain Warp Label
		ControlDomainWarpLabel = new Label {
			Text = "Domain Warp",
			HorizontalAlignment = HorizontalAlignment.Center
		};

		VBox.AddChild(ControlDomainWarpLabel);

		CreateDomainWarpEnabled();
		CreateDomainWarpType();
		CreateDomainWarpAmplitude();
		CreateDomainWarpFrequency();
		CreateDomainWarpFractalType();
		CreateDomainWarpFractalOctaves();
		CreateDomainWarpFractalLacunarity();
		CreateDomainWarpFractalGain();
	}

	private void CreateDomainWarpEnabled()
	{
		// Domain Enabled
		var checkbox = new CheckBox();
		checkbox.ButtonPressed = SettingsDomainWarp.Enabled;
		checkbox.Toggled += (v) => Noise.DomainWarpEnabled = v;
		ControlDomainEnabled = HBox("Enabled", checkbox);

		VBox.AddChild(ControlDomainEnabled);
	}

	private void CreateDomainWarpType()
	{
		// Domain Type
		var optionButtonDomainType = OptionButton(Enum.GetValues(typeof(FNLDWT)), (item) => 
		{
			Noise.DomainWarpType = (FNLDWT)item;
		});

		optionButtonDomainType.Selected = (int)SettingsDomainWarp.Type;

		ControlDomainWarpType = HBox("Type", optionButtonDomainType);

		VBox.AddChild(ControlDomainWarpType);
	}

	private void CreateDomainWarpAmplitude()
	{
		// Domain Amplitude
		var hsliderDomainAmplitude = HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 100,
			Value = 0,
			Step = 0.001f
		}, (v) =>
		{
			Noise.DomainWarpAmplitude = (float)v;
		});

		hsliderDomainAmplitude.Value = SettingsDomainWarp.Amplitude;

		ControlDomainWarpAmplitude = HBox("Amplitude", hsliderDomainAmplitude);

		VBox.AddChild(ControlDomainWarpAmplitude);
	}

	private void CreateDomainWarpFrequency()
	{
		// Domain Frequency
		var hsliderDomainFrequency = HSlider(new SettingsSlider { 
			MinValue = 0,
			MaxValue = 1,
			Step = 0.001f
		}, (v) =>
		{
			Noise.DomainWarpFrequency = (float)v;
		});

		hsliderDomainFrequency.Value = SettingsDomainWarp.Frequency;

		ControlDomainWarpFrequency = HBox("Frequency", hsliderDomainFrequency);

		VBox.AddChild(ControlDomainWarpFrequency);
	}

	private void CreateDomainWarpFractalType()
	{
		// Domain Warp Fractal Type
		var optionButtonDomainWarpFractalType = OptionButton(Enum.GetValues(typeof(FNLDWFT)), (item) => 
		{
			Noise.DomainWarpFractalType = (FNLDWFT)item;
		});

		optionButtonDomainWarpFractalType.Selected = (int)SettingsDomainWarp.FractalType;

		ControlDomainWarpFractalType = HBox("Fractal Type", optionButtonDomainWarpFractalType);

		VBox.AddChild(ControlDomainWarpFractalType);
	}

	private void CreateDomainWarpFractalOctaves()
	{
		// Domain Warp Fractal Octaves
		var hsliderDomainWarpFractalOctaves = HSlider(new SettingsSlider
		{
			MinValue = 1,
			MaxValue = 10,
			Value = 5,
			Step = 1
		}, (v) =>
		{
			Noise.DomainWarpFractalOctaves = (int)v;
		});

		hsliderDomainWarpFractalOctaves.Value = SettingsDomainWarp.FractalOctaves;

		ControlDomainWarpFractalOctaves = HBox("Fractal Octaves", hsliderDomainWarpFractalOctaves);

		VBox.AddChild(ControlDomainWarpFractalOctaves);
	}

	private void CreateDomainWarpFractalLacunarity()
	{
		// Domain Warp Fractal Lacunarity
		var hsliderDomainWarpFractalLacunarity = HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 10,
			Value = 2,
			Step = 0.001f
		}, (v) =>
		{
			Noise.DomainWarpFractalLacunarity = (float)v;
		});

		hsliderDomainWarpFractalLacunarity.Value = SettingsDomainWarp.FractalLacunarity;

		ControlDomainWarpFractalLacunarity = HBox("Fractal Lacunarity", hsliderDomainWarpFractalLacunarity);

		VBox.AddChild(ControlDomainWarpFractalLacunarity);
	}

	private void CreateDomainWarpFractalGain()
	{
		// Domain Warp Fractal Gain
		var hsliderDomainWarpFractalGain = HSlider(new SettingsSlider
		{
			MinValue = 0,
			MaxValue = 10,
			Value = 0.5f,
			Step = 0.001f
		}, (v) =>
		{
			Noise.DomainWarpFractalGain = (float)v;
		});

		hsliderDomainWarpFractalGain.Value = SettingsDomainWarp.FractalGain;

		ControlDomainWarpFractalGain = HBox("Fractal Gain", hsliderDomainWarpFractalGain);

		VBox.AddChild(ControlDomainWarpFractalGain);
	}

	private HBoxContainer HBox(string text, Node child2)
	{
		var hbox = new HBoxContainer();
		hbox.AddChild(new Label { 
			CustomMinimumSize = new Vector2(150, 0),
			Text = text 
		});
		hbox.AddChild(child2);
		return hbox;
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

	public class Settings
	{
		public FNL.NoiseTypeEnum NoiseType { get; set; } = FNL.NoiseTypeEnum.SimplexSmooth;
		public int               Seed      { get; set; }
		public float             Frequency { get; set; } = 0.05f;
		public Vector2           Offset    { get; set; }
	}

	public class FractalSettings
	{
		public FNLFT Type             { get; set; } = FNLFT.None;
		public int   Octaves          { get; set; } = 5;
		public float Lacunarity       { get; set; } = 2;
		public float Gain             { get; set; } = 0.5f;
		public float WeightedStrength { get; set; }
	}

	public class DomainWarpSettings
	{
		public bool    Enabled           { get; set; }
		public FNLDWT  Type              { get; set; } = FNLDWT.Simplex;
		public float   Amplitude         { get; set; } = 30;
		public float   Frequency         { get; set; } = 0.05f;
		public FNLDWFT FractalType       { get; set; } = FNLDWFT.Progressive;
		public int     FractalOctaves    { get; set; } = 5;
		public float   FractalLacunarity { get; set; } = 6;
		public float   FractalGain       { get; set; }
	}

	public class CellularSettings
	{
		public FNLCDF DistanceFunction   { get; set; } = FNLCDF.Euclidean;
		public float Jitter              { get; set; } = 0.45f;
		public FNLCRT ReturnTypeFunction { get; set; } = FNLCRT.Distance;
	}
}
