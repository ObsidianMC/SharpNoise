﻿namespace SharpNoise.Modules;

/// <summary>
/// Abstract base class for noise modules.
/// </summary>
/// <remarks>
/// <para>
/// A noise module is an object that calculates and outputs a value
/// given a three-dimensional input value.
///
/// Each type of noise module uses a specific method to calculate an
/// output value.  Some of these methods include:
/// <list type="bullet">
/// <item>
/// <description>
/// Calculating a value using a coherent-noise function or some other
/// mathematical function.
/// </description>
/// </item>
/// <item>
/// <description>
/// Mathematically changing the output value from another noise module
/// in various ways.
/// </description>
/// </item>
/// <item>
/// <description>Combining the output values from two noise modules in various ways.</description>
/// </item>
/// </list>
///
/// An application can use the output values from these noise modules in
/// the following ways:
/// <list type="bullet">
/// <item>
/// <description>It can be used as an elevation value for a terrain height map</description>
/// </item>
/// <item>
/// <description>It can be used as a grayscale (or an RGB-channel) value for a procedural texture</description>
/// </item>
/// <item>
/// <description>It can be used as a position value for controlling the movement of a simulated lifeform.</description>
/// </item>
/// </list>
/// 
/// </para>
/// 
/// <para>
/// A noise module defines a near-infinite 3-dimensional texture.  Each
/// position in this "texture" has a specific value.
/// 
/// Combining noise modules
///
/// Noise modules can be combined with other noise modules to generate
/// complex output values.  A noise module that is used as a source of
/// output values for another noise module is called a source module.
/// Each of these source modules may be connected to other
/// source modules, and so on.
///
/// There is no limit to the number of noise modules that can be connected
/// together in this way.  However, each connected noise module increases
/// the time required to calculate an output value.
/// </para>
/// 
/// <para>
/// Noise-module categories
///
/// The noise module classes that are included in libnoise can be roughly
/// divided into five categories.
/// </para>
///
/// <para>
/// Generator Modules
/// 
/// A generator module outputs a value generated by a coherent-noise
/// function or some other mathematical function.
///
/// Examples of generator modules include:
/// <list type="bullet">
/// <item>
/// <description><see cref="Const" />: Outputs a constant value.</description>
/// </item>
/// <item>
/// <description><see cref="Perlin" />: Outputs a value generated by a Perlin-noise function.</description>
/// </item>
/// <item>
/// <description><see cref="Cell" />: Outputs a value generated by a Voronoi-cell function.</description>
/// </item>
/// </list>
/// 
/// </para>
///
/// <para>
/// Modifier Modules
///
/// A modifer module mathematically modifies the output value from a
/// source module.
///
/// Examples of modifier modules include:
/// <list type="bullet">
/// <item>
/// <description><see cref="Curve" />: Maps the output value from the source module onto an arbitrary function curve.</description>
/// </item>
/// <item>
/// <description><see cref="Invert" />: Inverts the output value from the source module.</description>
/// </item>
/// </list>
/// 
/// </para>
///
/// <para>
/// Combiner Modules
///
/// A combiner module mathematically combines the output values from two
/// or more source modules together.
///
/// Examples of combiner modules include:
/// 
/// <list type="bullet">
/// <item>
/// <description><see cref="Add" />: Adds the two output values from two source modules.</description>
/// </item>
/// <item>
/// <description><see cref="Max" />: Outputs the larger of the two output values from two source modules.</description>
/// </item>
/// </list>
/// 
/// </para>
///
/// <para>
/// Selector Modules
///
/// A selector module uses the output value from a control module
/// to specify how to combine the output values from its source modules.
///
/// Examples of selector modules include:
/// 
/// <list type="bullet">
/// <item>
/// <description>
/// <see cref="Blend" />: Outputs a value that is linearly interpolated
/// between the output values from two source modules; the interpolation
/// weight is determined by the output value from the control module.
/// </description>
/// </item>
/// <item>
/// <description>
/// <see cref="Select" />: Outputs the value selected from one of two
/// source modules chosen by the output value from a control module.
/// </description>
/// </item>
/// </list>
/// 
/// </para>
///
/// <para>
/// Transformer Modules
///
/// A transformer module applies a transformation to the coordinates of
/// the input value before retrieving the output value from the source
/// module.  A transformer module does not modify the output value.
///
/// Examples of transformer modules include:
/// 
/// <list type="bullet">
/// <item>
/// <description>
/// <see cref="RotatePoint"/>: Rotates the coordinates of the input value around the
/// origin before retrieving the output value from the source module.
/// </description>
/// </item>
/// <item>
/// <description>
/// <see cref="ScalePoint"/>: Multiplies each coordinate of the input value by a
/// constant value before retrieving the output value from the source
/// module.
/// </description>
/// </item>
/// </list>
/// 
/// </para>
///
/// <para>
/// Connecting source modules to a noise module
///
/// An application connects a source module to a noise module by passing
/// the source module to the <see cref="SetSourceModule" /> method.
///
/// The application must also pass an index value to
/// <see cref="SetSourceModule" /> as well.  An index value is a numeric identifier for
/// that source module.  Index values are consecutively numbered starting
/// at zero.
///
/// To retrieve a reference to a source module, pass its index value to
/// the <see cref="GetSourceModule" /> method.
///
/// Each noise module requires the attachment of a certain number of
/// source modules before it can output a value.  For example, the
/// <see cref="Add" /> module requires two source modules, while the
/// <see cref="Perlin" /> module requires none.  Call the
/// <see cref="SourceModuleCount" /> method to retrieve the number of source modules
/// required by that module.
///
/// For non-selector modules, it usually does not matter which index value
/// an application assigns to a particular source module, but for selector
/// modules, the purpose of a source module is defined by its index value.
/// For example, consider the <see cref="Select" /> noise module, which
/// requires three source modules.  The control module is the source
/// module assigned an index value of 2.  The control module determines
/// whether the noise module will output the value from the source module
/// assigned an index value of 0 or the output value from the source
/// module assigned an index value of 1.
/// </para>
///
/// <para>
/// Generating output values with a noise module
///
/// Once an application has connected all required source modules to a
/// noise module, the application can now begin to generate output values
/// with that noise module.
///
/// To generate an output value, pass the ( x, y, z ) coordinates
/// of an input value to the <see cref="GetValue" /> method.
/// </para>
///
/// <para>
/// Using a noise module to generate terrain height maps or textures
///
/// One way to generate a terrain height map or a texture is to first
/// allocate a 2-dimensional array of floating-point values.  For each
/// array element, pass the array subscripts as x and y coordinates
/// to the <see cref="GetValue" /> method (leaving the z coordinate set to zero) and
/// place the resulting output value into the array element.
/// </para>
///
/// <para>
/// Creating your own noise modules
///
/// Create a class that publicly derives from <see cref="Module" />.
///
/// In the constructor, call the base class' constructor while passing the
/// number of required source modules.
///
/// Override the <see cref="GetValue" /> pure virtual method.  For generator modules,
/// calculate and output a value given the coordinates of the input value.
/// For other modules, retrieve the output values from each source module
/// referenced in the protected source modules array, mathematically
/// combine those values, and return the combined value.
///
/// When developing a noise module, you must ensure that your noise module
/// does not modify any source module or control module connected to it; a
/// noise module can only modify the output value from those source
/// modules.  You must also ensure that if an application fails to connect
/// all required source modules via the <see cref="SetSourceModule" /> method and then
/// attempts to call the <see cref="GetValue" /> method, your module will raise an
/// assertion.
///
/// It shouldn't be too difficult to create your own noise module.  If you
/// still have some problems, take a look at the source code for
/// <see cref="Add" />, which is a very simple noise module.
/// </para>
/// </remarks>
public abstract class Module
{
    /// <summary>
    /// Returns the number of source modules required by this noise
    /// module.
    /// </summary>
    public int SourceModuleCount => SourceModules.Length;

    public virtual ReadOnlySpan<Module> SourceModules => [];

    /// <summary>
    /// Generates an output value given the coordinates of the specified
    /// input value.
    /// </summary>
    /// <param name="x">The x coordinate of the input value.</param>
    /// <param name="y">The y coordinate of the input value.</param>
    /// <param name="z">The z coordinate of the input value.</param>
    /// <returns>The output value.</returns>
    /// <remarks>
    /// All source modules required by this noise module have been
    /// passed to the <see cref="SetSourceModule"/> method.
    ///
    /// Before an application can call this method, it must first connect
    /// all required source modules via the <see cref="SetSourceModule"/> method.  If
    /// these source modules are not connected to this noise module, this
    /// method raises a debug assertion.
    ///
    /// To determine the number of source modules required by this noise
    /// module, call the <see cref="GetSourceModuleCount"/> method.
    /// </remarks>
    public abstract double GetValue(double x, double y, double z);
}
