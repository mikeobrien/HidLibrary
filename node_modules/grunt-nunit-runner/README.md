# grunt-nunit-runner [![Build Status](https://api.travis-ci.org/mikeobrien/grunt-nunit-runner.png?branch=master)](https://travis-ci.org/mikeobrien/grunt-nunit-runner) [![NPM version](https://badge.fury.io/js/grunt-nunit-runner.png)](https://npmjs.org/package/grunt-nunit-runner)
Grunt plugin for running [NUnit](http://www.nunit.org/).
NOTE: this plugin requires Grunt 0.4.x.

## Getting Started
From the same directory as your project's Gruntfile and package.json, install
this plugin with the following command:

```bash
$ npm install grunt-nunit-runner --save-dev
```

Next add this line to your project's Gruntfile:

```js
grunt.loadNpmTasks('grunt-nunit-runner');
```

## Config
Inside your `Gruntfile.js` file, add a section named `nunit`, containing
the test runner configuration:

```js
nunit: {
    options: {

        // The path to the NUnit bin folder. If not specified the bin
        // folder must be in the system path.
        path: 'c:/Program Files/NUnit/bin',

        // Runs the anycpu or x86 build of NUnit. Default is anycpu. 
        // http://www.nunit.org/index.php?p=nunit-console&r=2.6.3
        platform: 'anycpu|x86',

        // Can be solutions, projects or individual assemblies. Solutions 
        // are searched for projects referencing nunit.framework.dll.
        files: ['src/MySolution.sln', 
                'src/Tests/Tests.csproj', 
                'src/Tests/bin/Debug/Tests.dll'],

        // Integrate test output with TeamCity.
        teamcity: true|false,

        // The options below map directly to the NUnit console runner. See here
        // for more info: http://www.nunit.org/index.php?p=consoleCommandLine&r=2.6.3

        // Name of the test case(s), fixture(s) or namespace(s) to run.
        run: ['TestSuite.Unit', 'TestSuite.Integration'],

        // Name of a file containing a list of the tests to run, one per line.
        runlist: 'TestsToRun.txt',

        // Project configuration (e.g.: Debug) to load.
        config: 'Debug',

        // Name of XML result file (Default: TestResult.xml)
        result: 'TestResult.xml',

        // Suppress XML result output.
        noresult: true|false,

        // File to receive test output.
        output: 'TestOutput.txt',

        // File to receive test error output.
        err: 'TestErrors.txt',

        // Work directory for output files.
        work: 'BuildArtifacts',

        // Label each test in stdOut.
        labels: true|false,

        // Set internal trace level.
        trace: 'Off|Error|Warning|Info|Verbose',

        // List of categories to include.
        include: ['BaseLine', 'Unit'],

        // List of categories to exclude.
        exclude: ['Database', 'Network'],

        // Framework version to be used for tests.
        framework: 'net-1.1',

        // Process model for tests.
        process: 'Single|Separate|Multiple',

        // AppDomain Usage for tests.
        domain: 'None|Single|Multiple',

        // Apartment for running tests (Default is MTA).
        apartment: 'MTA|STA',

        // Disable shadow copy when running in separate domain.
        noshadow: true|false,

        // Disable use of a separate thread for tests.
        nothread: true|false,

        // Base path to be used when loading the assemblies.
        basepath: 'src',

        // Additional directories to be probed when loading assemblies.
        privatebinpath: ['lib', 'bin'],

        // Set timeout for each test case in milliseconds.
        timeout: 1000,

        // Wait for input before closing console window.
        wait: true|false,

        // Do not display the logo.
        nologo: true|false,

        // Do not display progress.
        nodots: true|false,

        // Stop after the first test failure or error.
        stoponerror: true|false,

        // Erase any leftover cache files and exit.
        cleanup: true|false

    }
}
```

## License
MIT License