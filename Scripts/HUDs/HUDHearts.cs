using Godot;
using System.Collections.Generic;

public class HUDHearts : Control
{
	private const int _heartsPerRow = 10;
	private const int _pointsPerHeart = 10;

	private readonly PackedScene _sceneHeartSmall = GD.Load<PackedScene>("res://Scenes/HUDs/HeartSmall.tscn");

	private TextureProgress _heartBig;
	private List<TextureRect> _heartsSmall;
	private HBoxContainer[] _heartRows;

	private int _displayedMax = 0;
	private int _displayedFilled = 0;
	private int _displayedFraction = 0;

	public override void _Ready()
	{
		_heartBig = GetNode<TextureProgress>("HeartBig");
		_heartsSmall = new List<TextureRect>();
		_heartRows = new HBoxContainer[] {
			GetNode<HBoxContainer>("Row0"),
			GetNode<HBoxContainer>("Row1")
		};
	}

	public void ResetHearts(int maxhealth)
	{
		var hearts = maxhealth / _pointsPerHeart;
		if (_displayedFilled > 0)
			_heartsSmall[_displayedFilled - 1].RectMinSize = new Vector2(0, 0);
		if (hearts < _displayedMax)
			for (int i = _displayedMax; i > hearts; --i)
			{
				_displayedMax--;
				_heartsSmall[_displayedMax].Free();
				_heartsSmall.RemoveAt(_displayedMax);
			}
		else
			for (int i = _displayedMax; i < hearts; ++i)
			{
				var newHeart = (TextureRect)_sceneHeartSmall.Instance();
				_heartsSmall.Add(newHeart);
				_heartRows[_displayedMax / _heartsPerRow].AddChild(newHeart);
				_displayedMax++;
			}
		UpdateHearts(maxhealth);
	}

	public void UpdateHearts(int health)
	{
		_displayedFraction = (health - 1) % _pointsPerHeart + 1;
		_heartBig.Value = _displayedFraction;

		var hearts = (health - 1) / _pointsPerHeart;
		if (hearts < 0)
			hearts = 0;
		if (hearts == 0)
			_displayedFraction = 0;
		for (int i = 0; i < _displayedMax; ++i)
			_heartsSmall[i].GetChild<CanvasItem>(0).Visible = hearts > i;

		if (_displayedFilled > 1)
			_heartsSmall[_displayedFilled].RectMinSize = new Vector2(0, 0);

		TextureRect lastHeart = _heartsSmall[hearts];
		lastHeart.RectMinSize = _heartBig.RectSize;
		CallDeferred("AlignBigHeart", lastHeart);

		_displayedFilled = hearts;
	}

	private void AlignBigHeart(Control to)
	{
		_heartBig.RectGlobalPosition = to.RectGlobalPosition;

	}
}
