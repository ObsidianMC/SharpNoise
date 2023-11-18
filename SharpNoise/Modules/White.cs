namespace SharpNoise.Modules;

/// <summary>
/// Noise module that outputs 3-dimensional White noise.
/// </summary>
/// References &amp; acknowledgments
/// http://www.dspguru.com/dsp/howtos/how-to-generate-white-gaussian-noise
public class White : Module
{
    public override ReadOnlySpan<Module> SourceModules => [];

    public int Scale { get; set; } = 256;
    public int Seed { get; set; }

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
        return NoiseGenerator.ValueNoise3D((int)(x * Scale), (int)(y * Scale), (int)(z * Scale), Seed);
    }
}
