require "albacore"
require "release/filesystem"
require "release/nuget"

task :default => [:unitTests]

desc "Inits the build"
task :initBuild do
	FileSystem.EnsurePath("reports")
end

desc "Generate assembly info."
assemblyinfo :assemblyInfo => :initBuild do |asm|
    asm.version = ENV["GO_PIPELINE_LABEL"] + ".0"
    asm.company_name = "Ultraviolet Catastrophe"
    asm.product_name = "Hid Library"
    asm.title = "Hid Library"
    asm.description = "Hid communication library."
    asm.copyright = "Copyright (c) 2010 Ultraviolet Catastrophe"
    asm.output_file = "src/Current/HidLibrary/Properties/AssemblyInfo.cs"
end

desc "Builds the library."
msbuild :buildLibrary => :assemblyInfo do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Current/HidLibrary/HidLibrary.csproj"
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
#	nunit.options "/xml=reports/TestResult.xml"
#end

desc "Prep the package folder"
task :prepPackage => :buildLibrary do
	FileSystem.DeleteDirectory("deploy")
	FileSystem.EnsurePath("deploy/package/lib")
	FileSystem.CopyFiles("src/Current/HidLibrary/bin/Release/HidLibrary.dll", "deploy/package/lib")
	FileSystem.CopyFiles("src/Current/HidLibrary/bin/Release/HidLibrary.pdb", "deploy/package/lib")
end

desc "Create the nuspec"
nuspec :createSpec => :prepPackage do |nuspec|
   nuspec.id = "hidlibrary"
   nuspec.version = ENV["GO_PIPELINE_LABEL"]
   nuspec.authors = "Mike O'Brien"
   nuspec.owners = "Mike O'Brien"
   nuspec.description = "This library enables you to enumerate and communicate with Hid compatible USB devices in .NET."
   nuspec.summary = "This library enables you to enumerate and communicate with Hid compatible USB devices in .NET."
   nuspec.language = "en-US"
   nuspec.licenseUrl = "https://github.com/mikeobrien/HidLibrary/blob/master/LICENSE"
   nuspec.projectUrl = "https://github.com/mikeobrien/HidLibrary"
   nuspec.working_directory = "deploy/package"
   nuspec.output_file = "hidlibrary.nuspec"
   nuspec.tags = "usb hid"
end

desc "Create the nuget package"
nugetpack :createPackage => :createSpec do |nugetpack|
   nugetpack.nuspec = "deploy/package/hidlibrary.nuspec"
   nugetpack.base_folder = "deploy/package"
   nugetpack.output = "deploy"
end

desc "Push the nuget package"
nugetpush :pushPackage => :createPackage do |nugetpush|
   nugetpush.package = "deploy/hidlibrary.#{ENV['GO_PIPELINE_LABEL']}.nupkg"
end

desc "Tag the current release"
task :tagRelease do
	result = system("git", "tag", "-a", "v#{ENV['GO_PIPELINE_LABEL']}", "-m", "release-v#{ENV['GO_PIPELINE_LABEL']}")
	result = system("git", "push", "--tags")
end
