require "albacore"
require "release/filesystem"

reportsPath = "reports"
version = ENV["BUILD_NUMBER"]

desc "Inits the build"
task :initBuild do
	FileSystem.EnsurePath(reportsPath)
end

desc "Generate assembly info."
assemblyinfo :assemblyInfo => :initBuild do |asm|
    asm.version = version
    asm.company_name = "Ultraviolet Catastrophe"
    asm.product_name = "Hid Library"
    asm.title = "Hid Library"
    asm.description = "Hid communication library."
    asm.copyright = "Copyright (c) 2011 Ultraviolet Catastrophe"
    asm.output_file = "src/HidLibrary/Properties/AssemblyInfo.cs"
end

desc "Builds the library."
msbuild :buildLibrary => :assemblyInfo do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/HidLibrary/HidLibrary.csproj"
end

#desc "Builds the test project."
#msbuild :buildTestProject => :buildLibrary do |msb|
#    msb.properties :configuration => :Release
#    msb.targets :Clean, :Build
#    msb.solution = "src/Tests/Tests.csproj"
#end

#desc "NUnit Test Runner"
#nunit :unitTests => :buildTestProject do |nunit|
#	nunit.command = "src/packages/NUnit.2.5.9.10348/Tools/nunit-console.exe"
#	nunit.assemblies "src/Tests/bin/Release/Tests.dll"
#	nunit.options "/xml=#{reportsPath}/TestResult.xml"
#end

nugetApiKey = ENV["NUGET_API_KEY"]
deployPath = "deploy"

packagePath = File.join(deployPath, "package")
nuspecName = "hidlibrary.nuspec"
packageLibPath = File.join(packagePath, "lib")
binPath = "src/HidLibrary/bin/Release"

desc "Prep the package folder"
task :prepPackage => :buildLibrary do
	FileSystem.DeleteDirectory(deployPath)
	
	FileSystem.EnsurePath(packageLibPath)
	FileSystem.CopyFiles(File.join(binPath, "HidLibrary.dll"), packageLibPath)
	FileSystem.CopyFiles(File.join(binPath, "HidLibrary.pdb"), packageLibPath)
end

desc "Create the nuspec"
nuspec :createSpec => :prepPackage do |nuspec|
   nuspec.id = "hidlibrary"
   nuspec.version = version
   nuspec.authors = "Mike O'Brien"
   nuspec.owners = "Mike O'Brien"
   nuspec.title = "Hid Library"
   nuspec.description = "This library enables you to enumerate and communicate with Hid compatible USB devices in .NET."
   nuspec.summary = "This library enables you to enumerate and communicate with Hid compatible USB devices in .NET."
   nuspec.language = "en-US"
   nuspec.licenseUrl = "https://github.com/mikeobrien/HidLibrary/blob/master/LICENSE"
   nuspec.projectUrl = "https://github.com/mikeobrien/HidLibrary"
   nuspec.iconUrl = "https://github.com/mikeobrien/HidLibrary/raw/master/misc/hidlibrary.png"
   nuspec.working_directory = packagePath
   nuspec.output_file = nuspecName
   nuspec.tags = "usb hid"
end

desc "Create the nuget package"
nugetpack :createPackage => :createSpec do |nugetpack|
   nugetpack.nuspec = File.join(packagePath, nuspecName)
   nugetpack.base_folder = packagePath
   nugetpack.output = deployPath
end

desc "Push the nuget package"
nugetpush :pushPackage => :createPackage do |nuget|
	nuget.apikey = nugetApiKey
	nuget.package = File.join(deployPath, "hidlibrary.#{version}.nupkg")
end