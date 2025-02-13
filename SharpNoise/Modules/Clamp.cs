﻿using SharpNoise.Modules.Buffers;

namespace SharpNoise.Modules;

/// <summary>
/// Noise module that clamps the output value from a source module to a
/// range of values.
/// </summary>
/// <remarks>
/// The range of values in which to clamp the output value is called the
/// clamping range.
///
/// If the output value from the source module is less than the lower
/// bound of the clamping range, this noise module clamps that value to
/// the lower bound.  If the output value from the source module is
/// greater than the upper bound of the clamping range, this noise module
/// clamps that value to the upper bound.
///
/// To specify the upper and lower bounds of the clamping range, call the
/// <see cref="SetBounds"/> method.
///
/// This noise module requires one source module.
/// </remarks>
public class Clamp : Module
{
    public override ReadOnlySpan<Module> SourceModules => buffer;
    private OneModuleBuffer buffer;

    /// <summary>
    /// Default lower bound of the clamping range
    /// </summary>
    public const double DefaultLowerBound = -1D;

    /// <summary>
    /// Default upper bound of the clamping range
    /// </summary>
    public const double DefaultUpperBound = 1D;
    private double lowerBound = DefaultLowerBound;
    private double upperBound = DefaultUpperBound;

    /// <summary>
    /// Gets the lower bound of the clamping range.
    /// </summary>
    /// <remarks>
    /// If the output value from the source module is less than the lower
    /// bound of the clamping range, this noise module clamps that value
    /// to the lower bound.
    /// </remarks>
    public double LowerBound
    {
        get => lowerBound;
        set => SetBounds(value, upperBound);
    }

    /// <summary>
    /// Gets the upper bound of the clamping range.
    /// </summary>
    /// <remarks>
    /// If the output value from the source module is greater than the
    /// upper bound of the clamping range, this noise module clamps that
    /// value to the upper bound.
    /// </remarks>
    public double UpperBound
    {
        get => upperBound;
        set => SetBounds(lowerBound, value);
    }

    /// <summary>
    /// Gets or sets the first source module
    /// </summary>
    public Module Source0
    {
        get => buffer[0];
        set => buffer[0] = value;
    }

    /// <summary>
    /// Sets the lower and upper bounds of the clamping range.
    /// </summary>
    /// <param name="lower">The lower bound.</param>
    /// <param name="upper">The upper bound.</param>
    /// <remarks>
    /// The lower bound must be less than or equal to the
    /// upper bound.
    /// 
    /// If the output value from the source module is less than the lower
    /// bound of the clamping range, this noise module clamps that value
    /// to the lower bound.  If the output value from the source module
    /// is greater than the upper bound of the clamping range, this noise
    /// module clamps that value to the upper bound.
    /// </remarks>
    public void SetBounds(double lower, double upper)
    {
        if (lower > upper)
            throw new InvalidOperationException("lower cannot be greater than upper.");

        lowerBound = lower;
        upperBound = upper;
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
        return Math.Clamp(buffer[0].GetValue(x, y, z), lowerBound, upperBound);
    }
}
