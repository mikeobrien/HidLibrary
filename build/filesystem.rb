require "fileutils"

module FileSystem

	def FileSystem.EnsurePath(path)
		if !Dir.exists?(path) then 
			FileUtils.mkdir_p(path)
		end
	end

	def FileSystem.DeleteDirectory(path)
		if Dir.exists?(path) then 
			 FileUtils.rm_rf path
		end
	end

	def FileSystem.CopyFiles(source, target) 
		Dir.glob(source) do |name|
			FileUtils.cp(name, target)
		end	
	end
end