require "albacore"
require_relative "filesystem"
require_relative "gallio-task"

reportsPath = "reports"
version = ENV["BUILD_NUMBER"]

task :build => [:createPackage]
task :deploy => [:pushPackage]

task :initBuild do
	FileSystem.EnsurePath(reportsPath)
end

assemblyinfo :assemblyInfo => :initBuild do |asm|
    asm.version = version
    asm.company_name = "Ultraviolet Catastrophe"
    asm.product_name = "Hid Library"
    asm.title = "Hid Library"
    asm.description = "Hid communication library."
    asm.copyright = "Copyright (c) 2011 Ultraviolet Catastrophe"
    asm.output_file = "src/HidLibrary/Properties/AssemblyInfo.cs"
end

msbuild :buildLibrary => :assemblyInfo do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/HidLibrary/HidLibrary.csproj"
end

msbuild :buildTestProject => :buildLibrary do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Tests/Tests.csproj"
end

gallio :unitTests => :buildTestProject do |runner|
	runner.echo_command_line = true
	runner.add_test_assembly("src/Tests/bin/Release/Tests.dll")
	runner.verbosity = 'Normal'
	runner.report_directory = reportsPath
	runner.report_name_format = 'tests'
	runner.add_report_type('Html')
end

nugetApiKey = ENV["NUGET_API_KEY"]
deployPath = "deploy"

packagePath = File.join(deployPath, "package")
nuspecName = "hidlibrary.nuspec"
packageLibPath = File.join(packagePath, "lib")
binPath = "src/HidLibrary/bin/Release"

task :prepPackage => :unitTests do
	FileSystem.DeleteDirectory(deployPath)
	
	FileSystem.EnsurePath(packageLibPath)
	FileSystem.CopyFiles(File.join(binPath, "HidLibrary.dll"), packageLibPath)
	FileSystem.CopyFiles(File.join(binPath, "HidLibrary.pdb"), packageLibPath)
end

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

nugetpack :createPackage => :createSpec do |nugetpack|
   nugetpack.nuspec = File.join(packagePath, nuspecName)
   nugetpack.base_folder = packagePath
   nugetpack.output = deployPath
end

nugetpush :pushPackage => :createPackage do |nuget|
	nuget.apikey = nugetApiKey
	nuget.package = File.join(deployPath, "hidlibrary.#{version}.nupkg").gsub('/', '\\')
end