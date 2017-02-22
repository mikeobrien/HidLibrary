<h3>NOTE: Support is VERY limited for this library. It is almost impossible to troubleshoot issues with so many devices and configurations. The community may be able to offer some assistance <i>but you will largely be on your own</i>. If you submit an issue, please include a relevant code snippet and details about your operating system, .NET version and device. Pull requests are welcome and appreciated.</h3>

Hid Library
=============

[![Nuget](http://img.shields.io/nuget/v/HidLibrary.svg?style=flat)](http://www.nuget.org/packages/HidLibrary/) [![TeamCity Build Status](https://img.shields.io/teamcity/http/build.mikeobrien.net/s/hidlibrary.svg?style=flat)](http://build.mikeobrien.net/viewType.html?buildTypeId=hidlibrary&guest=1)

This library enables you to enumerate and communicate with Hid compatible USB devices in .NET. It offers synchronous and asynchronous read and write functionality as well as notification of insertion and removal of a device. This library works on x86 and x64.

Installation
------------

    PM> Install-Package hidlibrary
	
Developers
------------

| [![Mike O'Brien](https://avatars3.githubusercontent.com/u/187817?v=3&s=144)](https://github.com/mikeobrien) |  [![Austin Mullins](https://avatars3.githubusercontent.com/u/199260?v=3&s=144)](https://github.com/amullins83) |
|:---:|:---:|
| [Mike O'Brien](https://github.com/mikeobrien) | [Austin Mullins](https://github.com/amullins83) |
	
Contributors
------------

| [![Benjamin Wegman](https://avatars0.githubusercontent.com/u/397523?v=3&s=144)](https://github.com/bwegman) |  [![jwelch222](https://raw.githubusercontent.com/mikeobrien/HidLibrary/master/misc/default-avatar.jpg)](https://github.com/jwelch222) | [![Thomas Hammer](https://avatars3.githubusercontent.com/u/1710917?v=3&s=144)](https://github.com/thammer) | [![juliansiebert](https://raw.githubusercontent.com/mikeobrien/HidLibrary/master/misc/default-avatar.jpg)](https://github.com/juliansiebert) |
|:---:|:---:|:---:|:---:|
| [Benjamin Wegman](https://github.com/bwegman) | [jwelch222](https://github.com/jwelch222) | [Thomas Hammer](https://github.com/thammer) | [juliansiebert](https://github.com/juliansiebert) |

| [![George Hahn](https://avatars3.githubusercontent.com/u/502747?v=3&s=144)](https://github.com/GeorgeHahn) |  [![Rick van Lieshout](https://avatars3.githubusercontent.com/u/139665?v=3&s=144)](https://github.com/rvlieshout) | [![Paul Trandem](https://avatars2.githubusercontent.com/u/1494011?v=3&s=144)](https://github.com/ptrandem) | [![Neil Thiessen](https://avatars1.githubusercontent.com/u/2797902?v=3&s=144)](https://github.com/neilt6) |
|:---:|:---:|:---:|:---:|
| [George Hahn](https://github.com/GeorgeHahn) | [Rick van Lieshout](https://github.com/rvlieshout) | [Paul Trandem](https://github.com/ptrandem) | [Neil Thiessen](https://github.com/neilt6) |

| [![intrueder](https://avatars0.githubusercontent.com/u/182339?v=3&s=144)](https://github.com/intrueder) |  [![Bruno Juchli](https://avatars2.githubusercontent.com/u/2090172?v=3&s=144)](https://github.com/BrunoJuchli) | [![sblakemore](https://raw.githubusercontent.com/mikeobrien/HidLibrary/master/misc/default-avatar.jpg)](https://github.com/sblakemore) | [![J.C](https://avatars3.githubusercontent.com/u/850854?v=3&s=144)](https://github.com/jcyuan) |
|:---:|:---:|:---:|:---:|
| [intrueder](https://github.com/intrueder) | [Bruno Juchli](https://github.com/BrunoJuchli) | [sblakemore](https://github.com/sblakemore) | [J.C](https://github.com/jcyuan) |

| [![Marek Roszko](https://raw.githubusercontent.com/mikeobrien/HidLibrary/master/misc/default-avatar.jpg)](https://github.com/marekr) |  [![Bill Prescott](https://avatars2.githubusercontent.com/u/3333408?v=3&s=144)](https://github.com/bprescott) | [![Ananth Racherla](https://avatars0.githubusercontent.com/u/14127511?v=3&s=144)](https://github.com/ananth-racherla) |
|:---:|:---:|:---:|:---:|
| [Marek Roszko](https://github.com/marekr) | [Bill Prescott](https://github.com/bprescott) | [Ananth Racherla](https://github.com/ananth-racherla) |

Props
------------

Thanks to JetBrains for providing OSS licenses for [R#](http://www.jetbrains.com/resharper/features/code_refactoring.html) and [dotTrace](http://www.jetbrains.com/profiler/)!
	
Resources
------------

If your interested in HID development here are a few invaluable resources:  
  
1. [Jan Axelson's USB Hid Programming Page](http://janaxelson.com/hidpage.htm) - Excellent resource for USB Hid development. Full code samples in a number of different languages demonstrate how to use the Windows setup and Hid API.  
2. [Jan Axelson's book 'USB Complete'](http://janaxelson.com/usbc.htm) - Jan Axelson's guide to USB programming. Very good covereage of Hid. Must read for anyone new to Hid programming.
