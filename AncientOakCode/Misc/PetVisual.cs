namespace AncientOak.AncientOakCode.Misc;

public struct PetVisual
{
    public string TextureResourcePath;
    public float Scale;
    public float YPosition;

    public static PetVisual Reuniclus = new PetVisual
    {
        TextureResourcePath = "res://AncientOak/Scenes/Visuals/reuniclus.png",
        Scale = 1.0f,
        YPosition = -141.0f
    };

    public static PetVisual Rotom = new PetVisual
    {
        TextureResourcePath = "res://AncientOak/Scenes/Visuals/rotom.png",
        Scale = 0.34f,
        YPosition = -165.0f
    };

    public static PetVisual RotomFan = new PetVisual
    {
        TextureResourcePath = "res://AncientOak/Scenes/Visuals/rotom_fan.png",
        Scale = 0.42f,
        YPosition = -165.0f
    };

    public static PetVisual RotomFrost = new PetVisual
    {
        TextureResourcePath = "res://AncientOak/Scenes/Visuals/rotom_frost.png",
        Scale = 0.5f,
        YPosition = -145.0f
    };

    public static PetVisual RotomMow = new PetVisual
    {
        TextureResourcePath = "res://AncientOak/Scenes/Visuals/rotom_mow.png",
        Scale = 0.42f,
        YPosition = -165.0f
    };

    public static PetVisual RotomWash = new PetVisual
    {
        TextureResourcePath = "res://AncientOak/Scenes/Visuals/rotom_wash.png",
        Scale = 0.5f,
        YPosition = -165.0f
    };

    public static PetVisual RotomHeat = new PetVisual
    {
        TextureResourcePath = "res://AncientOak/Scenes/Visuals/rotom_heat.png",
        Scale = 0.34f,
        YPosition = -165.0f
    };
}