def gallio(*args, &block)
	task = lambda { |*args|
		task = Gallio.new
		block.call(task)
		task.run
	}
	Rake::Task.define_task(*args, &task)
end

class Gallio

    attr_accessor :gallio_path, :verbosity, :no_results, :no_progress, :no_logo, :do_not_run,
                  :ignore_annotations, :runtime_limit, :runner_type, :runner_extensions,
                  :runner_properties, :filter, :report_directory, :report_name_format, 
                  :report_archive, :report_types, :report_formatter_properties, :show_reports,
                  :test_assemblies, :hint_directories, :plugin_directories, :application_base_directory,
                  :working_directory, :shadow_copy, :debug, :runtime_version, :echo_command_line
    
    def initialize()
		@gallio_path = "C:/Program Files/Gallio/bin/gallio.echo"
        @test_assemblies = Array.new
        @hint_directories = Array.new
        @plugin_directories = Array.new
        @runtime_limit = -1
        @runner_extensions = Array.new
        @runner_properties = Array.new
        @report_types = Array.new
        @report_formatter_properties = Array.new
    end
    
    def add_test_assembly(assembly)
        test_assemblies.push(assembly)
    end
    
    def add_hint_directory(directory)
        hint_directories.push(directory)
    end
    
    def add_plugin_directory(directory)
        plugin_directories.push(directory)
    end
    
    def add_runner_extension(extension)
        runner_extensions.push(extension)
    end
    
    def add_runner_property(property)
        runner_properties.push(property)
    end
    
    def add_report_type(type)
        report_types.push(type)
    end
    
    def add_report_formatter_property(property)
        report_formatter_properties.push(property)
    end
    
    def run()
        
        command = []
		
		command << "\"#{to_windows_path(@gallio_path)}\""
        
        if @test_assemblies.count > 0 then command.concat(@test_assemblies.collect{|x| "\"#{x}\"" }) end
        if @hint_directories.count > 0 then command.concat(@hint_directories.collect{|x| "\"/hd:#{x}\"" }) end
        if @plugin_directories.count > 0 then command.concat(@plugin_directories.collect{|x| "\"/pd:#{x}\"" }) end
        if @application_base_directory != nil then command << "\"/abd:#{to_windows_path(@application_base_directory)}\"" end
        if @working_directory != nil then command << "\"/wd:#{to_windows_path(@working_directory)}\"" end
        if @shadow_copy == true then command << "/sc" end
        if @debug == true then command << "/d" end
        if @runtime_version != nil then command << "\"/rv:#{@runtime_version}\"" end
        
        # Quiet, Normal, Verbose, Debug
        if @verbosity != nil then command << "/v:#{@verbosity}" end
        
        if @no_results == true then command << "/ne" end
        if @no_progress == true then command << "/np" end
        if @no_logo == true then command << "/nl" end
        if @do_not_run == true then command << "/dnr" end
        if @ignore_annotations == true then command << "/ia" end
        if @runtime_limit > -1 then command << "/rtl:#{@runtime_limit}" end
        
        # IsolatedProcess, IsolatedAppDomain, Local
        if @runner_type != nil then command << "/r:#{@runner_type}" end
        
        if @runner_extensions.count > 0 then command.concat(@runner_extensions.collect{|x| "\"/re:#{x}\"" }) end
        if @runner_properties.count > 0 then command.concat(@runner_properties.collect{|x| "\"/rp:#{x}\"" }) end
        
        if @filter != nil then command << "\"/f:#{@filter}\"" end
        
        if @report_directory != nil then command << "\"/rd:#{to_windows_path(@report_directory)}\"" end
        if @report_name_format != nil then command << "\"/rnf:#{@report_name_format}\"" end
        if @report_archive != nil then command << "/ra:#{@report_archive}" end
        
        # Xml, Xml-Inline, Text, Text-Condensed, Html, Html-Condensed, XHtml, XHtml-Condensed, MHtml, MHtml-Condensed
        if @report_types.count > 0 then command.concat(@report_types.collect{|x| "/rt:#{x}" }) end
        
        if @report_formatter_properties.count > 0 then command.concat(@report_formatter_properties.collect{|x| "\"/rfp:#{x}\"" }) end
        if @show_reports == true then command << "/sr" end

		command = command.join(" ")

        error_handler = \
            lambda do |ok, res|
                       raise "Could not find gallio.echo.exe. " \
                             "Make sure it is added to your path." \
                       if res.exitstatus == 127
                       raise "Gallio failed with exit " \
                             "code #{res.exitstatus}." \
                       if res.exitstatus > 0
                   end

        if echo_command_line == true then puts command end
        
        sh command, &error_handler 
    end
	
	private
	
	def to_windows_path(path)
		path.gsub("/", "\\")
	end
    
end

    