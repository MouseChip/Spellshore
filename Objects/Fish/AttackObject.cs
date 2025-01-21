using Godot;
using System;

public partial class AttackObject : Node2D
{
	[Export] private RichTextLabel _runeLabel;
	[Export] private MouseCanvas _mouseCanvas;
	[Export] private CatchingFish _catchingFish;

	public string AttackName;
	public int RuneId;
	public bool CanDraw = false;
	public bool IsAttack {
		get { return _isAttack; }
		set { 
			_isAttack = value; 

			if (value) {
				LaunchAttack();
			}
		}
	}

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/PlayerData.tres");
	private AttackData _attackData = GD.Load<AttackData>("res://Data/AttackData.tres");
	private RuneData _runeData = GD.Load<RuneData>("res://Data/RuneData.tres");
	private Timer _attackTimer;
	private bool _isAttack = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_attackTimer = GetNode<Timer>("AttackTimer"); // Get the timer for when the bobber lands in water
		_attackTimer.Timeout += OnAttackTimeout;

		_mouseCanvas.RuneDrawn += OnRuneDrawn;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void LaunchAttack() {
		_attackTimer.WaitTime = (double)((Godot.Collections.Dictionary)_attackData.AttackDict[AttackName])["time"];
		_attackTimer.Start();

		CanDraw = true;

		_runeLabel.Text = (string)((Godot.Collections.Dictionary)_runeData.RunesDict[RuneId])["name"];
	}

	private void OnRuneDrawn(int runeId) {
		if (RuneId == runeId) {
			_catchingFish.secCount += MapFloat((int)((Godot.Collections.Dictionary)_attackData.AttackDict[AttackName])["damage"], 0, 100, 0.5f, 3);
			EndAttack();
		}
	}

	private void EndAttack() {
		_attackTimer.Stop();

		AttackName = null;
		RuneId = -1;
		CanDraw = false;
		IsAttack = false;
		_catchingFish.MoveTimer.Start();  // Restart the timer
	}

	private void OnAttackTimeout() {
		GD.Print("Hit!");
		_playerData.PlayerHealth -= (int)((Godot.Collections.Dictionary)_attackData.AttackDict[AttackName])["damage"];
		_catchingFish.secCount -= MapFloat((int)((Godot.Collections.Dictionary)_attackData.AttackDict[AttackName])["damage"], 0, 100, 0.5f, 2);
		EndAttack();
	}

	public static float MapFloat(float value, float fromLow, float fromHigh, float toLow, float toHigh) {
		return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
	}
}
