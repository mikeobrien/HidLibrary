require 'rubygems'
require 'rake/gempackagetask'

task :default => [:init, :package, :clean]

task :clean do
	if FileTest.exists?("lib") then FileUtils.rm_rf("lib") end
end

task :init do
	if FileTest.exists?("lib") then FileUtils.rm_rf("lib") end
	if FileTest.exists?("pkg") then FileUtils.rm_rf("pkg") end
	
	FileUtils.mkdir_p "lib"
	
	Dir["HidLibrary/*"].each do | file |
		FileUtils.copy(file, "lib");
	end

	spec = Gem::Specification.new do |spec|
		spec.platform = Gem::Platform::RUBY
		spec.summary = "Usb Hid .NET Component"
		spec.name = "hidlib"
		spec.version = "1.0.0.0"
		spec.files = Dir["lib/**/*"]
		spec.authors = ["Mike O'Brien"]
		spec.homepage = "http://github.com/mikeobrien/HidLibrary"
		spec.description = "This library enables you to enumerate and communicate with Hid compatible USB devices in .NET."
	end

	Rake::GemPackageTask.new(spec) do |package|
	end
end