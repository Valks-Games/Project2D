namespace Project2D;

// Everything in this class will eventually be deleted as UINoiseSettings is the new kid on the block
public static class UIElements
{
	public static HBoxContainer HSlider(SettingsSlider settings, Action<Slider, SettingsSlider> action)
	{
		var hbox = HBox(settings.Name);
		var slider = new HSlider {
			MinValue = settings.MinValue,
			MaxValue = settings.MaxValue,
			Step = settings.Step,
			Value = settings.Value,
			SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
			SizeFlagsVertical = (int)Control.SizeFlags.Fill
		};

		action(slider, settings);

		hbox.AddChild(slider);

		return hbox;
	}

	public static HBoxContainer HLineEdit(SettingsLineEdit settings, Action<LineEdit, SettingsLineEdit> action)
	{
		var hbox = HBox(settings.Name);
		var lineEdit = new LineEdit
		{
			Text = settings.Value,
			SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
		};

		action(lineEdit, settings);

		hbox.AddChild(lineEdit);

		return hbox;
	}

	public static HBoxContainer HCheckbox(SettingsCheckBox settings, Action<CheckBox, SettingsCheckBox> action)
	{
		var hbox = HBox(settings.Name);
		var checkbox = new CheckBox {
			ButtonPressed = settings.Pressed
		};

		action(checkbox, settings);

		hbox.AddChild(checkbox);

		return hbox;
	}

	public static HBoxContainer HBox(string name)
	{
		var hbox = new HBoxContainer();

		hbox.AddChild(new Label
		{
			CustomMinimumSize = new Vector2(90, 0),
			Text = name.AddSpaceBeforeEachCapital()
		});

		return hbox;
	}

	public static Button CreateButton(string name, Action action)
	{
		var button = new Button();
		button.Text = name;
		button.Pressed += () => action();

		return button;
	}
}
