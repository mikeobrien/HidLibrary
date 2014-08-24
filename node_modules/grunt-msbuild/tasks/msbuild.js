'use strict';
module.exports = function (grunt) {

    var exec = require('child_process').exec,
        path = require('path'),
        async = require('async'),
        fs = require('fs');

    var _ = grunt.util._;

    var versions = {
        1.0: '1.0.3705',
        1.1: '1.1.4322',
        2.0: '2.0.50727',
        3.5: '3.5',
        4.0: '4.0.30319'
    };

    grunt.registerMultiTask('msbuild', 'Run MSBuild tasks', function () {

        var asyncCallback = this.async();

        var options = this.options({
            stdout: false,
            stderr: true,
            targets: ['Build'],
            buildParameters: {},
            failOnError: true,
            verbosity: 'normal',
            processor: '',
            version: 4.0
        });

        if (!options.projectConfiguration) {
            options.projectConfiguration = 'Release';
        }

        grunt.verbose.writeln('Using Options: ' + JSON.stringify(options, null, 4).cyan);

        var projectFunctions = [];

        this.files.forEach(function (filePair) {
            filePair.src.forEach(function (src) {
                projectFunctions.push(function (cb) {
                    build(src, options, cb);
                });

                projectFunctions.push(function (cb) {
                    cb();
                });
            });
        });

        async.series(projectFunctions, function () {
            asyncCallback();
        });

    });

    function build(src, options, cb) {

        grunt.log.writeln('Building ' + src.cyan);
        var cmd = buildCommand(src, options);

        if (!cmd) {
            return;
        }

        var cp = exec(cmd, options.execOptions, function (err, stdout, stderr) {
            if (_.isFunction(options.callback)) {
                options.callback.call(this, err, stdout, stderr, cb);
            } else {
                if (err) {
                    grunt.log.writeln('Build failed '.cyan + src);
                    if (options.failOnError) {
                        grunt.warn(err);
                    }
                }
                grunt.log.writeln('Build complete ' + src.cyan);
                cb();
            }
        });

        if (options.stdout || grunt.option('verbose')) {
            cp.stdout.pipe(process.stdout);
        }

        if (options.stderr || grunt.option('verbose')) {
            cp.stderr.pipe(process.stderr);
        }

    }

    function buildCommand(src, options) {

        var commandPath = path.normalize(getBuildExecutablePath(options.version, options.processor));

        var projectPath = '\"' + path.normalize(path.resolve() + '/' + src) + '\"';

        var args = ' /target:' + options.targets;
        args += ' /verbosity:' + options.verbosity;

        if (options.maxCpuCount) {
            grunt.verbose.writeln('Using maxcpucount:' + '' + options.maxCpuCount.cyan);
            args += ' /maxcpucount:' + options.maxCpuCount;
        }

        args += ' /property:Configuration=' + options.projectConfiguration;

        for (var buildArg in options.buildParameters) {

            args += ' /property:' + buildArg + '=\"' + options.buildParameters[buildArg] + '\"';
        }

        var fullCommand = commandPath + ' ' + projectPath + ' ' + args;

        grunt.verbose.writeln('Using Command:' + fullCommand.cyan);

        return fullCommand;
    }

    function getBuildExecutablePath(version, processor) {

        // temp mono xbuild hack for linux / osx - assumes xbuild is in the path, works on my machine (Ubuntu 12.04 with Mono JIT compiler version 3.2.1 (Debian 3.2.1+dfsg-1~pre2))
        if (process.platform === 'linux' || process.platform === 'darwin') {
            return 'xbuild';
        }

        processor = 'Framework' + (processor === 64 ? processor : '');

        version = versions[version];

        if (!version) {
            grunt.fatal('Unrecognised .NET framework version "' + version + '"');
        }

        var buildExecutablePath = path.join(process.env.WINDIR, 'Microsoft.Net', processor, 'v' + version, 'MSBuild.exe');

        if (!fs.existsSync(buildExecutablePath)) {
            grunt.fatal('Unable to find MSBuild executable');
        }

        return buildExecutablePath;

    }

};
