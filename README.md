# HardwareHackerApps
A collection of application examples that might be useful for hardware hacking, demos, etc.

These are not intended to be production-ready. They're meant to provide working examples of functionalities that are useful for hardware demos.


## Folder descriptions

* Windows - This folder contains applications intended for Windows 8 or later (though they may work with earlier versions).
    * C++ - This folder contains applications written in C++. These have few dependencies and might be easier to get working with some hardware APIs. However, they're fairly low-level and can be more cumbersome to work with than C#. Refer to the README.md inside this folder for descriptions of the included applications.
    * C# - This folder contains applications written in C#. These have dependencies on .NET and might be harder to get working with some hardware APIs (you might need to create an [interop DLL<sup>1</sup>](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/interop/interoperability-overview) to work with unmanaged code). However, they're fairly high-level and can be more productive to work with than C++. Refer to the README.md inside this folder for descriptions of the included applications.


## References

1. https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/interop/interoperability-overview
