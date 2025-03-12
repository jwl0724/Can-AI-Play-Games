using Godot;
using System;

public partial class NextFruitUI : Control
{
    // Components
    [Export] private TextureRect imageTexture;

    public override void _Ready()
    {
        GameLoop.Instance.Connect(GameLoop.SignalName.NextFruitPicked, Callable.From((int fruitType) => OnFruitPicked((FruitType) fruitType)));
    }

    private void OnFruitPicked(FruitType type)
    {
        imageTexture.Texture = GetImage(type);
    }

    private static CompressedTexture2D GetImage(FruitType type)
    {
        switch(type)
        {
            case FruitType.APPLE:
                return Images.Apple;

            case FruitType.CHERRY:
                return Images.Cherry;

            case FruitType.DEKOPON:
                return Images.Dekopon;

            case FruitType.GRAPE:
                return Images.Grape;

            case FruitType.MELON:
                return Images.Melon;

            case FruitType.ORANGE:
                return Images.Orange;

            case FruitType.PEACH:
                return Images.Peach;

            case FruitType.PEAR:
                return Images.Pear;

            case FruitType.PINEAPPLE:
                return Images.Pineapple;

            case FruitType.STRAWBERRY:
                return Images.Strawberry;

            case FruitType.WATERMELON:
                return Images.Watermelon;

            default:
                GD.PushError($"Invalid fruit type inputted: {type}");
                return null;
        }
    }
}
