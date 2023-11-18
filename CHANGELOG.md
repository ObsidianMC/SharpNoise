# SharpNoise Changelog

## 0.13.0

- Upgrade projects to .NET 8
- Update NuGet packages
- Remove obsolete BinaryFormatter usage
- Add notice to README

## 0.12.1.1

- Generate xml doc files on build and include them in NuGet packages

## 0.12.1

- Fix bug in cubic interpolation function ([Issue #18](https://github.com/rthome/SharpNoise/issues/18))
- Fix PlanetBuilderExample which was broken by the rename of the Cell module (formerly known as Voronoi)

## 0.12.0

- Upgraded the project to target .NET Standard 2.0 (at last)
- Add new white noise module (by https://github.com/SirePi)
- Generalize Voronoi to support several cell functions (by https://github.com/SirePi)

## 0.11.0

- Moved project to GitHub (from Bitbucket)
- Rewrite and clean up unit tests with xUnit instead of MSTest, making them run on Mono.
- Improve test coverage with new unit tests
- Continuous Integration with Travis CI for Linux/Mono and AppVeyor for Windows/.NET
- Some code refactoring to use C# 6 features
- Move sample applications into a separate solution
- Cache module: Works now after deserialization and implements IDisposable
- Separate SharpNoise.Utilities into a separate NuGet package

## 0.10.0

- NoiseMapBuilders now run builds in parallel, utilizing all available CPUs & cores. 
- New 'OpenGLExample' example application, which shows how to use SharpNoise in a realtime scenario, to deform a rotating sphere. 
- New 'NoiseTester' example application (still in development) which shows a hierarchical view of a tree of noise modules and their outputs. 
- New 3D NoiseCube and NoiseCubeBuilders. 
- Filtering for NoiseMaps and NoiseCubes, allowing up- and downscaling of noise data. 
- Some code cleanups and small optimizations 
- Fix some bugs, notably in the Select module
