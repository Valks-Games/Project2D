using FNL     = Godot.FastNoiseLite;
using FNLFT   = Godot.FastNoiseLite.FractalTypeEnum;
using FNLDWT  = Godot.FastNoiseLite.DomainWarpTypeEnum;
using FNLDWFT = Godot.FastNoiseLite.DomainWarpFractalTypeEnum;
using FNLCDF  = Godot.FastNoiseLite.CellularDistanceFunctionEnum;
using FNLCRT  = Godot.FastNoiseLite.CellularReturnTypeEnum;

namespace Project2D;

public class NoiseEventArgs
{
    public NoiseEventArgs(FNL noise) { Noise = noise; }
    public FNL Noise { get; }
}

public class UINoiseSettings
{
    public delegate void SettingsChangedEventHandler(object sender, NoiseEventArgs e);

    public event SettingsChangedEventHandler SettingsChangedEvent;

	public bool VisibilityPreview { get; set; }

	public  PanelContainer Panel { get; set; }
	public  FNL            Noise { get; set; } = new();
	private VBoxContainer  VBoxSettings  { get; set; }

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

	public void HideDomainWarp()
	{

	}

	public Control Create(string name)
	{
		var vbox = new VBoxContainer();
		vbox.Name = name;

		// Prepare PanelContainer
		var scrollContainer = new ScrollContainer
		{
			CustomMinimumSize = new Vector2(350, 400) // this was eye-balled
		};

		Panel = new PanelContainer();

		scrollContainer.AddChild(Panel);

		var marginContainer = new MarginContainer();
		VBoxSettings = new VBoxContainer();

		foreach (var direction in new string[] { "left", "right", "up", "down" })
			marginContainer.AddThemeConstantOverride($"margin_{direction}", 5);

		Panel.AddChild(marginContainer);
		marginContainer.AddChild(VBoxSettings);

		// SETTINGS
		CreateUI();

		vbox.AddChild(CreatePreview());
		vbox.AddChild(scrollContainer);

		ControlPreview.Visible = VisibilityPreview;

		return vbox;
	}

	private void CreateUI()
	{
		CreateSettings();
		CreateFractalSettings();
		CreateCellularSettings();
		CreateDomainWarpSettings();
	}

	private void CreateSettings()
	{
		CreateNoiseType();
		CreateSeed();
		CreateFrequency();
		CreateOffset();
	}

	private TextureRect CreatePreview()
	{
		// Preview
		var noiseTexure = new NoiseTexture2D();
		noiseTexure.Noise = Noise;
		noiseTexure.Width = 400;
		noiseTexure.Height = 200;

		ControlPreview = new TextureRect
		{
			Texture = noiseTexure,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered,
			IgnoreTextureSize = true,
			CustomMinimumSize = new Vector2(0, 100)
		};

		return ControlPreview;
	}

	private void CreateNoiseType()
	{
		// Noise Type
		var optionButtonNoiseType = OptionButton(Enum.GetValues(typeof(FNL.NoiseTypeEnum)), (item) => 
			Noise.NoiseType = (FNL.NoiseTypeEnum)item);

		optionButtonNoiseType.Selected = (int)Noise.NoiseType;

		ControlNoiseType = HBox("Noise Type", optionButtonNoiseType);

		VBoxSettings.AddChild(ControlNoiseType);
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

		lineEditSeed.Text = $"{Noise.Seed}";

		ControlSeed = HBox("Seed", lineEditSeed);

		VBoxSettings.AddChild(ControlSeed);
	}

	private void CreateFrequency()
	{
		// Frequency
		var hsliderFrequency = HSlider(new SettingsSlider { 
			MinValue = 0.001f,
			MaxValue = 0.05f,
			Step = 0.001f
		}, Noise.Frequency, (v) => Noise.Frequency = (float)v);

		ControlFrequency = HBox("Frequency", hsliderFrequency);

		VBoxSettings.AddChild(ControlFrequency);
	}

	private void CreateOffset()
	{
		// Offset Label
		ControlLabelOffset = new Label { Text = "Offset" };

		VBoxSettings.AddChild(ControlLabelOffset);

		// Offset
		ControlOffset = new HBoxContainer();

		var offsetRange = 1000;

		// OffsetX
		var hsliderOffsetX = HSlider(new SettingsSlider { 
			MinValue = -offsetRange,
			MaxValue = offsetRange,
			Step = 0.001f
		}, Noise.Offset.x, (v) => {
			Noise.Offset = new Vector3((float)v, Noise.Offset.y, Noise.Offset.z);
		}, false);

		ControlOffset.AddChild(hsliderOffsetX);

		// OffsetY
		var hsliderOffsetY = HSlider(new SettingsSlider { 
			MinValue = -offsetRange,
			MaxValue = offsetRange,
			Step = 0.001f
		}, Noise.Offset.y, (v) => {
			Noise.Offset = new Vector3(Noise.Offset.x, (float)v, Noise.Offset.z);
		}, false);

		ControlOffset.AddChild(hsliderOffsetY);

		VBoxSettings.AddChild(ControlOffset);
	}

	private void CreateFractalSettings()
	{
		// Fractal Label
		ControlLabelFractal = new Label {
			Text = "Fractal",
			HorizontalAlignment = HorizontalAlignment.Center
		};

		VBoxSettings.AddChild(ControlLabelFractal);

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

		optionButtonFractalType.Selected = (int)Noise.FractalType;

		ControlFractalType = HBox("Type", optionButtonFractalType);

		VBoxSettings.AddChild(ControlFractalType);
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
		}, Noise.FractalOctaves, (v) =>
		{
			Noise.FractalOctaves = (int)v;
		});

		ControlFractalOctaves = HBox("Octaves", hsliderFractalOctaves);

		VBoxSettings.AddChild(ControlFractalOctaves);
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
		}, Noise.FractalLacunarity, (v) =>
		{
			Noise.FractalLacunarity = (float)v;
		});

		ControlFractalLacunarity = HBox("Lacunarity", hsliderFractalLacunarity);

		VBoxSettings.AddChild(ControlFractalLacunarity);
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
		}, Noise.FractalGain, (v) =>
		{
			Noise.FractalGain = (float)v;
		});

		ControlFractalGain = HBox("Gain", hsliderFractalGain);

		VBoxSettings.AddChild(ControlFractalGain);
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
		}, Noise.FractalWeightedStrength, (v) =>
		{
			Noise.FractalWeightedStrength = (float)v;
		});

		ControlFractalWeightedStrength = HBox("Weighted Strength", hsliderFractalWeightedStrength);

		VBoxSettings.AddChild(ControlFractalWeightedStrength);
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

		VBoxSettings.AddChild(ControlDomainWarpLabel);

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
		checkbox.ButtonPressed = Noise.DomainWarpEnabled;
		checkbox.Toggled += (v) =>
		{
			Noise.DomainWarpEnabled = v;
			SettingsChangedEvent?.Invoke(this, new NoiseEventArgs(Noise));
		};
		ControlDomainEnabled = HBox("Enabled", checkbox);

		VBoxSettings.AddChild(ControlDomainEnabled);
	}

	private void CreateDomainWarpType()
	{
		// Domain Type
		var optionButtonDomainType = OptionButton(Enum.GetValues(typeof(FNLDWT)), (item) => 
		{
			Noise.DomainWarpType = (FNLDWT)item;
		});

		optionButtonDomainType.Selected = (int)Noise.DomainWarpType;

		ControlDomainWarpType = HBox("Type", optionButtonDomainType);

		VBoxSettings.AddChild(ControlDomainWarpType);
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
		}, Noise.DomainWarpAmplitude, (v) =>
		{
			Noise.DomainWarpAmplitude = (float)v;
		});

		ControlDomainWarpAmplitude = HBox("Amplitude", hsliderDomainAmplitude);

		VBoxSettings.AddChild(ControlDomainWarpAmplitude);
	}

	private void CreateDomainWarpFrequency()
	{
		// Domain Frequency
		var hsliderDomainFrequency = HSlider(new SettingsSlider { 
			MinValue = 0,
			MaxValue = 1,
			Step = 0.001f
		}, Noise.Frequency, (v) =>
		{
			Noise.DomainWarpFrequency = (float)v;
		});

		ControlDomainWarpFrequency = HBox("Frequency", hsliderDomainFrequency);

		VBoxSettings.AddChild(ControlDomainWarpFrequency);
	}

	private void CreateDomainWarpFractalType()
	{
		// Domain Warp Fractal Type
		var optionButtonDomainWarpFractalType = OptionButton(Enum.GetValues(typeof(FNLDWFT)), (item) => 
		{
			Noise.DomainWarpFractalType = (FNLDWFT)item;
		});

		optionButtonDomainWarpFractalType.Selected = (int)Noise.DomainWarpFractalType;

		ControlDomainWarpFractalType = HBox("Fractal Type", optionButtonDomainWarpFractalType);

		VBoxSettings.AddChild(ControlDomainWarpFractalType);
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
		}, Noise.DomainWarpFractalOctaves, (v) =>
		{
			Noise.DomainWarpFractalOctaves = (int)v;
		});

		ControlDomainWarpFractalOctaves = HBox("Fractal Octaves", hsliderDomainWarpFractalOctaves);

		VBoxSettings.AddChild(ControlDomainWarpFractalOctaves);
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
		}, Noise.DomainWarpFractalLacunarity, (v) =>
		{
			Noise.DomainWarpFractalLacunarity = (float)v;
		});

		ControlDomainWarpFractalLacunarity = HBox("Fractal Lacunarity", hsliderDomainWarpFractalLacunarity);

		VBoxSettings.AddChild(ControlDomainWarpFractalLacunarity);
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
		}, Noise.DomainWarpFractalGain , (v) =>
		{
			Noise.DomainWarpFractalGain = (float)v;
		});

		ControlDomainWarpFractalGain = HBox("Fractal Gain", hsliderDomainWarpFractalGain);

		VBoxSettings.AddChild(ControlDomainWarpFractalGain);
	}

	private static HBoxContainer HBox(string text, Node child2)
	{
		var hbox = new HBoxContainer();
		hbox.AddChild(new Label { 
			CustomMinimumSize = new Vector2(150, 0),
			Text = text 
		});
		hbox.AddChild(child2);
		return hbox;
	}

	private HBoxContainer HSlider(SettingsSlider settings, double initialValue, Action<double> valueChanged, bool showLineEditValue = true)
	{
		var hbox = new HBoxContainer();
		hbox.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;

		var slider = new HSlider
		{
			MinValue = settings.MinValue,
			MaxValue = settings.MaxValue,
			Step = settings.Step,
			Value = settings.Value,
			SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
			SizeFlagsVertical = (int)Control.SizeFlags.Fill
		};

		LineEdit lineEdit = null;

		slider.Value = initialValue;
		
		hbox.AddChild(slider);

		if (showLineEditValue)
		{ 
			lineEdit = new LineEdit
			{
				Editable = false,
				Text = initialValue + ""
			};

			hbox.AddChild(lineEdit);
		}

		slider.ValueChanged += (value) => {
			if (lineEdit != null)
				lineEdit.Text = value + "";

			valueChanged(value);
			SettingsChangedEvent?.Invoke(this, new NoiseEventArgs(Noise));
		};

		return hbox;
	}

	private LineEdit LineEdit(Action<string> textChanged)
	{
		var lineEdit = new LineEdit {
			SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
		};

		lineEdit.TextChanged += (text) => 
		{
			textChanged(text);	
			SettingsChangedEvent?.Invoke(this, new NoiseEventArgs(Noise));
		};

		return lineEdit;
	}

	private OptionButton OptionButton(Array items, Action<long> itemSelected)
	{
		var optionButton = new OptionButton();
		optionButton.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;

		foreach (var item in items)
			optionButton.AddItem($"{item}".AddSpaceBeforeEachCapital());

		optionButton.ItemSelected += (item) =>
		{
			itemSelected(item);
			SettingsChangedEvent?.Invoke(this, new NoiseEventArgs(Noise));
		};

		return optionButton;
	}
}
