﻿namespace SharpNoise;

/// <summary>
/// Implements a noise cube, a 3-dimensional array of floating-point
/// values.
/// </summary>
/// <remarks>
/// A noise cube is designed to store coherent-noise values generated by a
/// noise module, although it can store values from any source.
///
/// The size (width, height and depth) of the noise map can be specified during
/// object construction or at any other time.
/// 
/// All of the values outside of the noise map are assumed to have a
/// common value known as the border value.
///
/// To set the border value, modify the BorderValue property.
/// </remarks>
public sealed class NoiseCube
{
    /// <summary>
    /// Gets or sets the border value for all positions outside the cube
    /// </summary>
    public float BorderValue { get; set; }

    /// <summary>
    /// Gets the width of the Cube
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// Gets the height of the Cube
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// Gets the depth of the Cube
    /// </summary>
    public int Depth { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the Cube is empty
    /// </summary>
    public bool IsEmpty => values is null;

    private float[] values;

    public NoiseCube()
    {
    }

    public NoiseCube(int width, int height, int depth)
    {
        SetSize(width, height, depth);
    }

    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="other">The NoiseCube to copy</param>
    public NoiseCube(NoiseCube other)
    {
        ArgumentNullException.ThrowIfNull(other);

        SetSize(other.Width, other.Height, other.Depth);
        other.values.CopyTo(values, 0);
        BorderValue = other.BorderValue;
    }

    /// <summary>
    /// Clears and resets the cube
    /// </summary>
    public void ResetCube()
    {
        values = null;
        Width = 0;
        Height = 0;
        Depth = 0;
        BorderValue = 0;
    }

    /// <summary>
    /// Clears the cube to a specified value.
    /// </summary>
    /// <param name="value">
    /// The value that all positions within the noise cube are cleared to.
    /// </param>
    public void Clear(float value)
    {
        if (values is null)
        {
            return;
        }

        for (var i = 0; i < values.Length; i++)
            values[i] = value;
    }

    /// <summary>
    /// Sets the size of the cube
    /// </summary>
    /// <param name="width">The new width of the Cube</param>
    /// <param name="height">The new height of the Cube</param>
    /// <param name="depth">The new depth of the Cube</param>
    /// <remarks>
    /// After changing the size of the Cube the contents of the cube are all zero.
    /// </remarks>
    public void SetSize(int width, int height, int depth)
    {
        if (width < 0) throw new ArgumentException($"Parameter {nameof(width)} cannot be less than 0.", nameof(width));
        if (height < 0) throw new ArgumentException($"Parameter {nameof(height)} cannot be less than 0.", nameof(height));
        if (depth < 0) throw new ArgumentException($"Parameter {nameof(depth)} cannot be less than 0.", nameof(depth));

        if (width == 0 || height == 0 || depth == 0)
        {
            ResetCube();
        }
        else
        {
            values = new float[width * height * depth];
            Width = width;
            Height = height;
            Depth = depth;
        }
    }

    /// <summary>
    /// Gets or sets a value at the specified position
    /// </summary>
    /// <param name="x">The x coordinate of the position.</param>
    /// <param name="y">The y coordinate of the position.</param>
    /// <param name="z">The z coordinate of the position.</param>
    /// <returns>The value at that position</returns>
    /// <remarks>
    /// This calls <see cref="SetValue"/> or <see cref="GetValue"/>
    /// </remarks>
    public float this[int x, int y, int z]
    {
        get => GetValue(x, y, z);
        set => SetValue(x, y, z, value);
    }

    private int GetIndex(int x, int y, int z)
    {
        return x + Width * y + Width * Height * z;
    }

    /// <summary>
    /// Returns a value from the specified position in the cube.
    /// </summary>
    /// <param name="x">The x coordinate of the position.</param>
    /// <param name="y">The y coordinate of the position.</param>
    /// <param name="z">The z coordinate of the position.</param>
    /// <returns>The value at that position.</returns>
    /// <remarks>
    /// This method returns the border value if the coordinates are
    /// outside of the cube bounds.
    /// </remarks>
    public float GetValue(int x, int y, int z)
    {
        if (values is null)
        {
            return BorderValue;
        }

        if (x >= 0 && x < Width && y >= 0 && y < Height && z >= 0 && z < Depth)
        {
            return values[GetIndex(x, y, z)];
        }

        return BorderValue;
    }

    /// <summary>
    /// Sets a value at a specified position in the cube.
    /// </summary>
    /// <param name="x">The x coordinate of the position.</param>
    /// <param name="y">The y coordinate of the position.</param>
    /// <param name="z">The z coordinate of the position.</param>
    /// <param name="value">The value to set at the given position.</param>
    /// <remarks>
    /// This method does nothing if the noise cube object is empty or the
    /// position is outside the bounds of the cube.
    /// </remarks>
    public void SetValue(int x, int y, int z, float value)
    {
        if (values is null)
        {
            return;
        }

        if (x >= 0 && x < Width && y >= 0 && y < Height && z >= 0 && z < Depth)
        {
            values[GetIndex(x, y, z)] = value;
        }
    }

    /// <summary>
    /// Create a new NoiseCube from the given source NoiseCube using trilinear filtering.
    /// 
    /// When the sample is outside the source BorderValue is used by default. If <see cref="clamp"/>/>
    /// is set clamping is used instead.
    /// </summary>
    /// <param name="src">The source NoiseCube</param>
    /// <param name="width">Width of the new NoiseCube</param>
    /// <param name="height">Height of the new NoiseCube</param>
    /// <param name="depth">Depth of the new NoiseCube</param>
    /// <param name="clamp">Use clamping when the sample is outside the source NoiseCube</param>
    /// <returns>The new NoiseCube</returns>
    public static NoiseCube TrilinearFilter(NoiseCube src, int width, int height, int depth, bool clamp = false)
    {
        var dest = new NoiseCube(width, height, depth);

        float xratio = (float)src.Width / dest.Width;
        float yratio = (float)src.Height / dest.Height;
        float zratio = (float)src.Depth / dest.Depth;

        Parallel.For(0, dest.Depth, z =>
        {
            for (int y = 0; y < dest.Height; ++y)
                for (int x = 0; x < dest.Width; ++x)
                {
                    float u = (x + 0.5f) * xratio - 0.5f;
                    float v = (y + 0.5f) * yratio - 0.5f;
                    float w = (z + 0.5f) * zratio - 0.5f;

                    int x0 = NoiseMath.FastFloor(u);
                    int y0 = NoiseMath.FastFloor(v);
                    int z0 = NoiseMath.FastFloor(w);
                    int x1 = x0 + 1;
                    int y1 = y0 + 1;
                    int z1 = z0 + 1;

                    float xf = u - x0;
                    float yf = v - y0;
                    float zf = w - z0;

                    if (clamp)
                    {
                        x0 = Math.Clamp(x0, 0, src.Width - 1);
                        x1 = Math.Clamp(x1, 0, src.Width - 1);
                        y0 = Math.Clamp(y0, 0, src.Height - 1);
                        y1 = Math.Clamp(y1, 0, src.Height - 1);
                        z0 = Math.Clamp(z0, 0, src.Depth - 1);
                        z1 = Math.Clamp(z1, 0, src.Depth - 1);
                    }

                    float c000 = src.GetValue(x0, y0, z0);
                    float c001 = src.GetValue(x0, y0, z1);
                    float c010 = src.GetValue(x0, y1, z0);
                    float c011 = src.GetValue(x0, y1, z1);
                    float c100 = src.GetValue(x1, y0, z0);
                    float c101 = src.GetValue(x1, y0, z1);
                    float c110 = src.GetValue(x1, y1, z0);
                    float c111 = src.GetValue(x1, y1, z1);

                    float val = NoiseMath.Trilinear(xf, yf, zf,
                        c000, c001, c010, c011, c100, c101, c110, c111);

                    dest.SetValue(x, y, z, val);
                }
        });

        return dest;
    }
}
