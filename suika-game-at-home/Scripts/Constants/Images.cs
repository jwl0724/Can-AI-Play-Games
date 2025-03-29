using Godot;

public static class Images
{
    // Paths to images
    private const string APPLE = "res://Assets/Sprites/Apple.png";
    private const string CHERRY = "res://Assets/Sprites/Cherry.png";
    private const string DEKOPON = "res://Assets/Sprites/Dekopon.png";
    private const string GRAPE = "res://Assets/Sprites/Grape.png";
    private const string MELON = "res://Assets/Sprites/Melon.png";
    private const string ORANGE = "res://Assets/Sprites/Orange.png";
    private const string PEACH = "res://Assets/Sprites/Peach.png";
    private const string PEAR = "res://Assets/Sprites/Pear.png";
    private const string PINEAPPLE = "res://Assets/Sprites/Pineapple.png";
    private const string STRAWBERRY = "res://Assets/Sprites/Strawberry.png";
    private const string WATERMELON = "res://Assets/Sprites/Watermelon.png";

    // Images
    public static readonly CompressedTexture2D Apple = ResourceLoader.Load<CompressedTexture2D>(APPLE);
    public static readonly CompressedTexture2D Cherry = ResourceLoader.Load<CompressedTexture2D>(CHERRY);
    public static readonly CompressedTexture2D Dekopon = ResourceLoader.Load<CompressedTexture2D>(DEKOPON);
    public static readonly CompressedTexture2D Grape = ResourceLoader.Load<CompressedTexture2D>(GRAPE);
    public static readonly CompressedTexture2D Melon = ResourceLoader.Load<CompressedTexture2D>(MELON);
    public static readonly CompressedTexture2D Orange = ResourceLoader.Load<CompressedTexture2D>(ORANGE);
    public static readonly CompressedTexture2D Peach = ResourceLoader.Load<CompressedTexture2D>(PEACH);
    public static readonly CompressedTexture2D Pear = ResourceLoader.Load<CompressedTexture2D>(PEAR);
    public static readonly CompressedTexture2D Pineapple = ResourceLoader.Load<CompressedTexture2D>(PINEAPPLE);
    public static readonly CompressedTexture2D Strawberry = ResourceLoader.Load<CompressedTexture2D>(STRAWBERRY);
    public static readonly CompressedTexture2D Watermelon = ResourceLoader.Load<CompressedTexture2D>(WATERMELON);
}
