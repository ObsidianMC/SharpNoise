﻿using SharpNoise.Utilities.Imaging;
using Xunit;

namespace SharpNoise.Tests.Utilities;

public class ColorTests
{
    [Fact]
    public void Color_CopyConstructor_Test()
    {
        var expected = new Color(120, 5, 80, 255);
        var copy = new Color(expected);

        Assert.Equal(expected, copy);
    }

    [Fact]
    public void Color_LinearInterp_Middle_Test()
    {
        var color1 = new Color(100, 100, 100, 100);
        var color2 = new Color(0, 0, 0, 0);

        var expected = new Color(50, 50, 50, 50);
        var actual = Color.LinearInterpColor(color1, color2, 0.5f);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Color_LinearInterp_Upper_Test()
    {
        var color1 = new Color(100, 100, 100, 100);
        var color2 = new Color(0, 0, 0, 0);

        var expected = new Color(75, 75, 75, 75);
        var actual = Color.LinearInterpColor(color1, color2, 0.25f);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Color_LinearInterp_Middle2_Test()
    {
        var color1 = new Color(100, 100, 100, 100);
        var color2 = new Color(200, 200, 200, 200);

        var expected = new Color(150, 150, 150, 150);
        var actual = Color.LinearInterpColor(color1, color2, 0.5f);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Color_LinearInterp_Same_Test()
    {
        var color1 = new Color(100, 100, 100, 100);
        var color2 = new Color(color1);

        var expected = color1;
        var actual = Color.LinearInterpColor(color1, color2, 0.5f);

        Assert.Equal(expected, actual);
    }
}
