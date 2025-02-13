# SharpNoise � Obsidian fork

## :warning: Notice :warning:

This is a custom fork with changes specific to Obsidian. Current stage of development is experimental. See LICENSE for original license and CHANGELOG for list of changes.

---

A library for generating coherent noise. It can be used to procedurally create natural-looking textures (such as wood, stone, or clouds), terrain heightmaps, normal maps, and a lot more.

SharpNoise is a loose port of Jason Bevins' [libNoise](http://libnoise.sourceforge.net/) to C#. SharpNoise is published unter the terms of the Gnu LGPL 3.

Available on NuGet: https://www.nuget.org/packages/SharpNoise

## Usage

SharpNoise has [modules](https://github.com/rthome/SharpNoise/tree/master/SharpNoise/Modules) as building blocks for noise generators. There are two kinds of modules:

* Generator modules, which produce noise values and have no sources
* Modifier and Combiner modules, which take one or more source modules and produce a new output from the values of their sources.

These modules can be joined together in a tree of noise modules to generate very complex noise patterns.

### Example Applications

To see how to use SharpNoise, check out the example applications:

* [BeginnerSample](https://github.com/rthome/SharpNoise/tree/master/Examples/BeginnerSample) is a very simple first sample that just renders an image from a noisemap and saves it as a PNG.
* [ComplexPlanetExample](https://github.com/rthome/SharpNoise/tree/master/Examples/ComplexPlanetExample) generates a complex planetary surface, using [over 100 connected noise modules](https://github.com/rthome/SharpNoise/blob/master/Examples/ComplexPlanetExample/PlanetGenerator.cs).
* [OpenGLExample](https://github.com/rthome/SharpNoise/tree/master/Examples/OpenGLExample) renders ever-changing data from a simpler noise generator in real time, using OpenGL 3.3.
* [NoiseTester](https://github.com/rthome/SharpNoise/tree/master/Examples/NoiseTester) is a rough visualization tool that shows how the output of different modules is combined.

There is a separate solution with the example applications in the Examples/ folder.

### Development

I think SharpNoise is in a pretty OK state and am not doing a lot of work on it any more. However, I would like to fix any bugs or other issues that crop up. So if anything's not right, please let me know.
