using SharpNoise.Modules.Buffers;

namespace SharpNoise.Modules;

/// <summary>
/// Noise module that raises the output value from a first source module
/// to the power of the output value from a second source module.
/// </summary>
/// <remarks>
/// The first source module must have an index value of 0.
/// The second source module must have an index value of 1.
///
/// This noise module requires two source modules.
/// </remarks>
public class Power : Module
{
    public override ReadOnlySpan<Module> SourceModules => buffer;
    private TwoModulesBuffer buffer;

    /// <summary>
    /// Gets or sets the first source module
    /// </summary>
    public Module Source0
    {
        get => buffer[0];
        set => buffer[0] = value;
    }

    /// <summary>
    /// Gets or sets the second source module
    /// </summary>
    public Module Source1
    {
        get => buffer[1];
        set => buffer[1] = value;
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
        return Math.Pow(buffer[0].GetValue(x, y, z), buffer[1].GetValue(x, y, z));
    }
}
