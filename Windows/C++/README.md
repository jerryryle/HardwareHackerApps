# HardwareHackerApps - C++

To build the applications, open the HardwareHackerAppsC++.sln file in [Visual Studio 2017 Community<sup>1</sup>](https://www.visualstudio.com/downloads/).

To create your own application based upon one of these applications, you should:

1. Create your own empty solution in Visual Studio
2. Copy the UtilityLibrary project folder into your solution folder and add it to your solution
3. Copy the desired application project folder into your solution folder and add it to your solution
4. Ensure that the application project contains a reference to the UtilityLibrary project


## Folder descriptions

* CaptainHook - A windowless application that hides in the notification area as an icon and demonstrates how to use a low-level hook to trap keyboard input.
* UtilityLibrary - Utility/wrapper classes used by the applications. This builds as a static library.


## References

1. https://www.visualstudio.com/downloads/
