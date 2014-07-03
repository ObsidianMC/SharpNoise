﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharpNoise.Modules
{
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
    /// <description><see cref="Voronoi" />: Outputs a value generated by a Voronoi-cell function.</description>
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
    [Serializable]
    public abstract class Module
    {
        protected readonly Module[] sourceModules;
        private readonly int sourceModuleCount;

        /// <summary>
        /// Returns the number of source modules required by this noise
        /// module.
        /// </summary>
        public int SourceModuleCount { get { return sourceModuleCount; } }

        /// <summary>
        /// Constructor, called from derived classes
        /// </summary>
        /// <param name="sourceModuleCount">The number of required source modules.</param>
        protected Module(int sourceModuleCount)
        {
            sourceModuleCount = NoiseMath.Max(0, sourceModuleCount);
            this.sourceModuleCount = sourceModuleCount;

            if (sourceModuleCount > 0)
            {
                sourceModules = new Module[sourceModuleCount];
                for (var i = 0; i < sourceModuleCount; i++)
                    sourceModules[i] = null;
            }
            else
                sourceModules = null;
        }

        /// <summary>
        /// Returns a reference to a source module connected to this noise
        /// module.
        /// </summary>
        /// <param name="index">The index value assigned to the source module.</param>
        /// <returns>A reference to the source module.</returns>
        /// <remarks>
        /// The index value ranges from 0 to one less than the number of
        /// source modules required by this noise module.
        /// A source module with the specified index value has been added
        /// to this noise module via a call to <see cref="SetSourceModule"/>.
        ///
        /// Each noise module requires the attachment of a certain number of
        /// source modules before an application can call the <see cref="GetValue"/>
        /// method.
        /// </remarks>
        public virtual Module GetSourceModule(int index)
        {
            if (sourceModules[index] == null)
                throw new NoModuleException();

            try
            {
                return sourceModules[index];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException("Source module index is out of range", e);
            }
        }

        /// <summary>
        /// Connects a source module to this noise module.
        /// </summary>
        /// <param name="index">An index value to assign to this source module.</param>
        /// <param name="module">The source module to attach.</param>
        /// <remarks>
        /// The index value ranges from 0 to one less than the number of
        /// source modules required by this noise module.
        ///
        /// A noise module mathematically combines the output values from the
        /// source modules to generate the value returned by <see cref="GetValue"/>.
        ///
        /// The index value to assign a source module is a unique identifier
        /// for that source module.  If an index value has already been
        /// assigned to a source module, this noise module replaces the old
        /// source module with the new source module.
        ///
        /// Before an application can call the <see cref="GetValue"/>v method, it must
        /// first connect all required source modules.  To determine the
        /// number of source modules required by this noise module, call the
        /// <see cref="GetSourceModuleCount" /> method.
        ///
        /// This source module must exist throughout the lifetime of this
        /// noise module unless another source module replaces that source
        /// module.
        ///
        /// A noise module does not modify a source module; it only modifies
        /// its output values.
        /// </remarks>
        public virtual void SetSourceModule(int index, Module module)
        {
            if (module == null)
                throw new ArgumentNullException("module", "The given Module must not be null.");
            if (ReferenceEquals(this, module))
                throw new ArgumentException("You cannot set a module as it's own source module.");

            try
            {
                sourceModules[index] = module;
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException("Target index is out of range", e);
            }
        }

        /// <summary>
        /// Serializes the entire module graph to a stream.
        /// </summary>
        /// <param name="target">The stream that the serialized data will be written to.</param>
        /// <remarks>
        /// This method uses the BinaryFormatter as a default.
        /// </remarks>
        public void Serialize(Stream target)
        {
            Serialize(target, new BinaryFormatter());
        }

        /// <summary>
        /// Serializes the entire module graph to a stream.
        /// </summary>
        /// <param name="target">The stream that the serialized data will be written to.</param>
        /// <param name="formatter">The formatter to use for serialization</param>
        public void Serialize(Stream target, IFormatter formatter)
        {
            if (target == null || formatter == null)
                throw new ArgumentNullException("None of the arguments can be null.");

            try
            {
                formatter.Serialize(target, this);
            }
            catch (SerializationException ex)
            {
                throw new ModuleSerializationException("Module graph could not be serialized.", ex);
            }

        }

        /// <summary>
        /// Deserializes a module graph from a stream.
        /// </summary>
        /// <param name="source">The source stream from which the serialized data will be read</param>
        /// <param name="formatter">The formatter to be used for deserialization</param>
        /// <returns>Returns the deserialized object</returns>
        public static T Deserialize<T>(Stream source, IFormatter formatter) where T : Module
        {
            if (source == null || formatter == null)
                throw new ArgumentNullException("None of the arguments can be null.");

            try
            {
                return (T)formatter.Deserialize(source);
            }
            catch (SerializationException ex)
            {
                throw new ModuleSerializationException("An error occurred during deserialization.", ex);
            }
            catch (InvalidCastException ex)
            {
                throw new ModuleSerializationException("The deserialized object was not a module.", ex);
            }
        }

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
}
