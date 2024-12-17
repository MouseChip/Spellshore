using Godot;
using System;

public partial class SetClock : Node2D
{
	[Export] private RichTextLabel _timeText;
	[Export] private RichTextLabel _ampmText;
	[Export] private RichTextLabel _dayText;
	[Export] private TextureRect _seasonIcon;
	[Export] private TextureRect _weatherIcon;

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private TestSceneData _testSceneData; // Load the player data
	private Godot.Collections.Dictionary _dayNames = new Godot.Collections.Dictionary{
		{(int)Time.Weekday.Monday, "Mon"},
		{(int)Time.Weekday.Tuesday, "Tues"},
		{(int)Time.Weekday.Wednesday, "Wed"},
		{(int)Time.Weekday.Thursday, "Thur"},
		{(int)Time.Weekday.Friday, "Fri"},
		{(int)Time.Weekday.Saturday, "Sat"},
		{(int)Time.Weekday.Sunday, "Sun"}
	};
	private String season;
	private bool _isAm;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_testSceneData = GD.Load<TestSceneData>($"res://Scenes/SceneData/{GetOwner().GetOwner().GetChild(0).Name}Data.tres");
		SetSeason();
		SetWeather();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_timeText.Text = GetTime();
		if (_isAm) _ampmText.Text = "am"; // If it is the morning, set the text value
		else _ampmText.Text = "pm"; // Otherwise, set the text value

		_dayText.Text = (String)_dayNames[Time.GetDateDictFromSystem()["weekday"]];
	}

	private String GetTime() {
		var systemTime = Time.GetTimeDictFromSystem(); // Get the system time
		String stringTime = "xx:xx"; // String to store the rewritten time value
		String stringMinute = (string)systemTime["minute"];
		if ((int)systemTime["minute"] < 10) stringMinute = $"0{systemTime["minute"]}";

		if ((int)systemTime["hour"] == 0) { // If it is the zeroth hour...
			stringTime = $"12:{stringMinute}"; // Rewrite it as a 12
			_isAm = true; // It is morning
		} else if ((int)systemTime["hour"] < 12) { // Otherwise, if it is a morning hour...
			stringTime = $"0{systemTime["hour"]}:{stringMinute}"; // Set the time
			_isAm = true; // It is morning
		} else if ((int)systemTime["hour"] >= 12) { // Otherwise, if it is an afternoon hour...
			stringTime = $"0{(int)systemTime["hour"]-12}:{stringMinute}"; // Set the time
			_isAm = false; // It is afternoon
		}

		return stringTime;
	}

	private void SetSeason() {
		switch((int)Time.GetDateDictFromSystem()["month"]) {
			case 1: // January
				if (_playerData.PlayerHemisphere == "north") season = "winter";
				if (_playerData.PlayerHemisphere == "south") season = "summer";
				break;
			
			case 2: // February
				if (_playerData.PlayerHemisphere == "north") season = "winter";
				if (_playerData.PlayerHemisphere == "south") season = "summer";
				break;
			
			case 3: // March
				if (_playerData.PlayerHemisphere == "north") season = "spring";
				if (_playerData.PlayerHemisphere == "south") season = "autumn";
				break;
			
			case 4: // April
				if (_playerData.PlayerHemisphere == "north") season = "spring";
				if (_playerData.PlayerHemisphere == "south") season = "autumn";
				break;
			
			case 5: // May
				if (_playerData.PlayerHemisphere == "north") season = "spring";
				if (_playerData.PlayerHemisphere == "south") season = "autumn";
				break;
			
			case 6: // June
				if (_playerData.PlayerHemisphere == "north") season = "summer";
				if (_playerData.PlayerHemisphere == "south") season = "winter";
				break;
			
			case 7: // July
				if (_playerData.PlayerHemisphere == "north") season = "summer";
				if (_playerData.PlayerHemisphere == "south") season = "winter";
				break;
			
			case 8: // August
				if (_playerData.PlayerHemisphere == "north") season = "summer";
				if (_playerData.PlayerHemisphere == "south") season = "winter";
				break;
			
			case 9: // September
				if (_playerData.PlayerHemisphere == "north") season = "autumn";
				if (_playerData.PlayerHemisphere == "south") season = "spring";
				break;
			
			case 10: // October
				if (_playerData.PlayerHemisphere == "north") season = "autumn";
				if (_playerData.PlayerHemisphere == "south") season = "spring";
				break;
			
			case 11: // November
				if (_playerData.PlayerHemisphere == "north") season = "autumn";
				if (_playerData.PlayerHemisphere == "south") season = "spring";
				break;
			
			case 12: // December
				if (_playerData.PlayerHemisphere == "north") season = "winter";
				if (_playerData.PlayerHemisphere == "south") season = "summer";
				break;
		}

		if (season == "summer") {
			_seasonIcon.Modulate = new Color(245f / 255f, 255f / 255f, 0f / 255f);
		} else if (season == "autumn") {
			_seasonIcon.Modulate = new Color(255f / 255f, 150f / 255f, 0f / 255f);
		} else if (season == "winter") {
			_seasonIcon.Modulate = new Color(0 / 255f, 255 / 255f, 250 / 255f);
		} else if (season == "spring") {
			_seasonIcon.Modulate = new Color(50 / 255f, 255 / 255f, 0 / 255f);
		}
	}

	private void SetWeather() {
		switch (_testSceneData.Weather)
    {
        case "sunny":
            _weatherIcon.Modulate = new Color(245f / 255f, 255f / 255f, 0f / 255f);
            break;

        case "cloudy":
            _weatherIcon.Modulate = new Color(250f / 255f, 240f / 255f, 200f / 255f);
            break;

        case "foggy":
            _weatherIcon.Modulate = new Color(215f / 255f, 215f / 255f, 210f / 255f);
            break;

        case "rain":
            _weatherIcon.Modulate = new Color(10f / 255f, 60f / 255f, 245f / 255f);
            break;

        case "storm":
            _weatherIcon.Modulate = new Color(80f / 255f, 95f / 255f, 135f / 255f);
            break;

        case "snow":
            _weatherIcon.Modulate = new Color(40f / 255f, 235f / 255f, 255f / 255f);
            break;

        case "windy":
            _weatherIcon.Modulate = new Color(160f / 255f, 200f / 255f, 205f / 255f);
            break;

        default:
            _weatherIcon.Modulate = new Color(1f, 1f, 1f);
            break;
    }
	}
}