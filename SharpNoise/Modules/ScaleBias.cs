﻿using SharpNoise.Modules.Buffers;

namespace SharpNoise.Modules;

/// <summary>
/// Noise module that applies a scaling factor and a bias to the output
/// value from a source module.
/// </summary>
/// <remarks>
/// The <see cref="GetValue"/> method retrieves the output value from the source
/// module, multiplies it with a scaling factor, adds a bias to it, then
/// outputs the value.
///
/// This noise module requires one source module.
/// </remarks>
public class ScaleBias : Module
{
    public override ReadOnlySpan<Module> SourceModules => buffer;
    private OneModuleBuffer buffer;

    /// <summary>
    /// Default bias
    /// </summary>
    public const double DefaultBias = 0D;

    /// <summary>
    /// Default scale
    /// </summary>
    public const double DefaultScale = 1D;

    /// <summary>
    /// Gets or sets the first source module
    /// </summary>
    public Module Source0
    {
        get => buffer[0];
        set => buffer[0] = value;
    }

    /// <summary>
    /// Gets or sets the bias to apply to the scaled output value from the
    /// source module.
    /// </summary>
    /// <remarks>
    /// The <see cref="GetValue"/> method retrieves the output value from the source
    /// module, multiplies it with the scaling factor, adds the bias to
    /// it, then outputs the value.
    /// </remarks>
    public double Bias { get; set; } = DefaultBias;

    /// <summary>
    /// Gets or sets the scaling factor to apply to the output value from the
    /// source module.
    /// </summary>
    /// <remarks>
    /// The <see cref="GetValue"/> method retrieves the output value from the source
    /// module, multiplies it with the scaling factor, adds the bias to
    /// it, then outputs the value.
    /// </remarks>
    public double Scale { get; set; } = DefaultScale;

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
        return buffer[0].GetValue(x, y, z) * Scale + Bias;
    }
}
