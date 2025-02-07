﻿using SharpNoise.Modules.Buffers;

namespace SharpNoise.Modules;

/// <summary>
/// Noise module that outputs the value selected from one of two source
/// modules chosen by the output value from a control module.
/// </summary>
/// <remarks>
/// Unlike most other noise modules, the index value assigned to a source
/// module determines its role in the selection operation:
/// <list type="bullet">
/// <item>
/// <description>
/// Source module 0 (upper left in the diagram) outputs a value.
/// </description>
/// </item>
/// <item>
/// <description>
/// Source module 1 (lower left in the diagram) outputs a value.
/// </description>
/// </item>
/// <item>
/// <description>
/// Source module 2 (bottom of the diagram) is known as the control
/// module.  The control module determines the value to select.  If
/// the output value from the control module is within a range of values
/// known as the selection range, this noise module outputs the
/// value from the source module with an index value of 1.  Otherwise,
/// this noise module outputs the value from the source module with an
/// index value of 0.
/// </description>
/// </item>
/// </list>
/// 
/// To specify the bounds of the selection range, call the SetBounds()
/// method.
///
/// An application can set the control module with the <see cref="Control"/>
/// property instead of the <see cref="Module.SourceModules"/> property.  This may make the
/// application code easier to read.
///
/// By default, there is an abrupt transition between the output values
/// from the two source modules at the selection-range boundary.  To
/// smooth the transition, pass a non-zero value to the <see cref="EdgeFalloff"/>
/// method.  Higher values result in a smoother transition.
///
/// This noise module requires three source modules.
/// </remarks>
public class Select : Module
{
    public override ReadOnlySpan<Module> SourceModules => buffer;
    private ThreeModulesBuffer buffer;

    /// <summary>
    /// Default edge-falloff value
    /// </summary>
    public const double DefaultEdgeFalloff = 0D;

    /// <summary>
    /// Default lower bound of the selection range
    /// </summary>
    public const double DefaultLowerBound = -1D;

    /// <summary>
    /// Default upper bound of the selection range
    /// </summary>
    public const double DefaultUpperBound = 1D;
    private double edgeFalloff;
    private double lowerBound, upperBound;

    /// <summary>
    /// Gets or sets the falloff value at the edge transition.
    /// </summary>
    /// <remarks>
    /// The falloff value is the width of the edge transition at either
    /// edge of the selection range.
    ///
    /// By default, there is an abrupt transition between the output
    /// values from the two source modules at the selection-range
    /// boundary.
    /// 
    /// For example, if the selection range is 0.5 to 0.8, and the edge
    /// falloff value is 0.1, then the <see cref="GetValue"/> method outputs:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// the output value from the source module with an index value of 0
    /// if the output value from the control module is less than 0.4
    /// ( = 0.5 - 0.1).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// a linear blend between the two output values from the two source
    /// modules if the output value from the control module is between
    /// 0.4 ( = 0.5 - 0.1) and 0.6 ( = 0.5 + 0.1).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// the output value from the source module with an index value of 1
    /// if the output value from the control module is between 0.6
    /// ( = 0.5 + 0.1) and 0.7 ( = 0.8 - 0.1).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// a linear blend between the output values from the two source
    /// modules if the output value from the control module is between
    /// 0.7 ( = 0.8 - 0.1 ) and 0.9 ( = 0.8 + 0.1).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// the output value from the source module with an index value of 0
    /// if the output value from the control module is greater than 0.9
    /// ( = 0.8 + 0.1).
    /// </description>
    /// </item>
    /// </list>
    /// 
    /// This module requires a total of 3 source modules: 2 sources and 1 control module.
    /// </remarks>
    public double EdgeFalloff
    {
        get => edgeFalloff;
        set
        {
            // Make sure that the edge falloff curves do not overlap.
            var boundSize = UpperBound - LowerBound;
            edgeFalloff = (value + value > boundSize) ? boundSize / 2 : value;
        }
    }

    /// <summary>
    /// Gets or sets the lower bound of the selection range.
    /// </summary>
    /// <remarks>
    /// If the output value from the control module is within the
    /// selection range, the <see cref="GetValue"/> method outputs the value from the
    /// source module with an index value of 1.  Otherwise, this method
    /// outputs the value from the source module with an index value of 0.
    /// </remarks>
    public double LowerBound
    {
        get => lowerBound;
        set => SetBounds(value, upperBound);
    }

    /// <summary>
    /// Gets or sets the upper bound of the selection range.
    /// </summary>
    /// <remarks>
    /// If the output value from the control module is within the
    /// selection range, the <see cref="GetValue"/> method outputs the value from the
    /// source module with an index value of 1.  Otherwise, this method
    /// outputs the value from the source module with an index value of 0.
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
    /// Gets or sets the second source module
    /// </summary>
    public Module Source1
    {
        get => buffer[1];
        set => buffer[1] = value;
    }

    /// <summary>
    /// Gets or sets the control module.
    /// </summary>
    /// <remarks>
    /// The control module determines the output value to select.  If the
    /// output value from the control module is within a range of values
    /// known as the selection range, the <see cref="GetValue"/> method outputs
    /// the value from the source module with an index value of 1.
    /// Otherwise, this method outputs the value from the source module
    /// with an index value of 0.
    /// </remarks>
    public Module Control
    {
        get => buffer[2];
        set => buffer[2] = value;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public Select()
    {
        EdgeFalloff = DefaultEdgeFalloff;
        LowerBound = DefaultLowerBound;
        UpperBound = DefaultUpperBound;
    }

    /// <summary>
    /// Sets the lower and upper bounds of the selection range.
    /// </summary>
    /// <param name="lower">The lower bound.</param>
    /// <param name="upper">The upper bound.</param>
    /// <remarks>
    /// The lower bound must be less than or equal to the upper
    /// bound.
    /// 
    /// If the output value from the control module is within the
    /// selection range, the <see cref="GetValue"/> method outputs the value from the
    /// source module with an index value of 1.  Otherwise, this method
    /// outputs the value from the source module with an index value of 0.
    /// </remarks>
    public void SetBounds(double lower, double upper)
    {
        lowerBound = lower;
        upperBound = upper;

        // Make sure that the edge falloff curves do not overlap.
        EdgeFalloff = edgeFalloff;
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
        var controlValue = buffer[2].GetValue(x, y, z);

        if (EdgeFalloff <= 0.0)
        {
            if (controlValue < LowerBound || controlValue > UpperBound)
                return buffer[0].GetValue(x, y, z);
            else
                return buffer[1].GetValue(x, y, z);
        }

        if (controlValue < (LowerBound - EdgeFalloff))
        {
            // The output value from the control module is below the selector
            // threshold; return the output value from the first source module.
            return buffer[0].GetValue(x, y, z);
        }
        else if (controlValue < (LowerBound + EdgeFalloff))
        {
            // The output value from the control module is near the lower end of the
            // selector threshold and within the smooth curve. Interpolate between
            // the output values from the first and second source modules.
            double lowerCurve = (LowerBound - EdgeFalloff);
            double upperCurve = (LowerBound + EdgeFalloff);
            double alpha = NoiseMath.SCurve3((controlValue - lowerCurve) / (upperCurve - lowerCurve));
            return NoiseMath.Linear(
                buffer[0].GetValue(x, y, z),
                buffer[1].GetValue(x, y, z),
                alpha);
        }
        else if (controlValue < (UpperBound - EdgeFalloff))
        {
            // The output value from the control module is within the selector
            // threshold; return the output value from the second source module.
            return buffer[1].GetValue(x, y, z);
        }
        else if (controlValue < (UpperBound + EdgeFalloff))
        {
            // The output value from the control module is near the upper end of the
            // selector threshold and within the smooth curve. Interpolate between
            // the output values from the first and second source modules.
            double lowerCurve = (UpperBound - EdgeFalloff);
            double upperCurve = (UpperBound + EdgeFalloff);
            double alpha = NoiseMath.SCurve3((controlValue - lowerCurve) / (upperCurve - lowerCurve));
            return NoiseMath.Linear(
                buffer[1].GetValue(x, y, z),
                buffer[0].GetValue(x, y, z),
                alpha);
        }
        else
        {
            // Output value from the control module is above the selector threshold;
            // return the output value from the first source module.
            return buffer[0].GetValue(x, y, z);
        }
    }
}
