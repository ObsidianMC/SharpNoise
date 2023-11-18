using SharpNoise.Modules.Buffers;

namespace SharpNoise.Modules;

/// <summary>
/// Noise module that outputs the absolute value of the output value from
/// a source module.
/// </summary>
/// <remarks>
/// This noise module requires one source module.
/// </remarks>
public class Abs : Module
{
    public override ReadOnlySpan<Module> SourceModules => buffer;
    private OneModuleBuffer buffer;

    /// <summary>
    /// Gets or sets the first source module
    /// </summary>
    public Module Source0
    {
        get => buffer[0];
        set => buffer[0] = value;
    }

    /// <summary>
    /// See the documentation on the base class.
    /// <seealso cref="Module"/>
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="z">Z coordinate</param>
    /// <returns>Returns the computed value</returns>
    public override double GetValue(double x, double y, double z)
    {
        return Math.Abs(buffer[0].GetValue(x, y, z));
    }
}
