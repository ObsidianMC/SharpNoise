﻿using SharpNoise.Modules.Buffers;

namespace SharpNoise.Modules;

/// <summary>
/// Noise module that outputs a weighted blend of the output values from
/// two source modules given the output value supplied by a control module.
/// </summary>
/// <remarks>
/// <para>
/// Unlike most other noise modules, the index value assigned to a source
/// module determines its role in the blending operation:
/// <list type="bullet">
/// <item>
/// <description>
/// Source module 0 (upper left in the diagram) outputs one of the
///   values to blend.
/// </description>
/// </item>
/// <item>
/// <description>
/// Source module 1 (lower left in the diagram) outputs one of the
///   values to blend.
/// </description>
/// </item>
/// <item>
/// <description>
/// Source module 2 (bottom of the diagram) is known as the control
/// module. The control module determines the weight of the
/// blending operation. Negative values weigh the blend towards the
/// output value from the source module with an index value of 0.
/// Positive values weigh the blend towards the output value from the
/// source module with an index value of 1.
/// </description>
/// </item>
/// </list>
/// </para>
/// 
/// An application can set the control module with the <see cref="ControlModule"/>
/// property instead of the <see cref="SetSourceModule"/> method.  This may make the
/// application code easier to read.
///
/// This noise module uses linear interpolation to perform the blending
/// operation.
///
/// This noise module requires three source modules.
/// </remarks>
public class Blend : Module
{
    public override ReadOnlySpan<Module> SourceModules => buffer;
    private ThreeModulesBuffer buffer;

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
    /// Gets or sets the control module
    /// </summary>
    public Module Control
    {
        get => buffer[2];
        set => buffer[2] = value;
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
        var v0 = buffer[0].GetValue(x, y, z);
        var v1 = buffer[1].GetValue(x, y, z);
        var alpha = (buffer[2].GetValue(x, y, z) + 1) / 2;
        return NoiseMath.Linear(v0, v1, alpha);
    }
}
