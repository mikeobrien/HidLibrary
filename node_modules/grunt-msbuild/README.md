# grunt-msbuild

> Build projects with MSBuild using Grunt

## Getting Started
This plugin requires Grunt `~0.4.1`

If you haven't used [Grunt](http://gruntjs.com/) before, be sure to check out the [Getting Started](http://gruntjs.com/getting-started) guide, as it explains how to create a [Gruntfile](http://gruntjs.com/sample-gruntfile) as well as install and use Grunt plugins. Once you're familiar with that process, you may install this plugin with this command:

```shell
npm install grunt-msbuild --save-dev
```

Once the plugin has been installed, it may be enabled inside your Gruntfile with this line of JavaScript:

```js
grunt.loadNpmTasks('grunt-msbuild');
```

## The "msbuild" task

### Overview
In your project's Gruntfile, add a section named `msbuild` to the data object passed into `grunt.initConfig()`.

```js
grunt.initConfig({
    msbuild: {
        dev: {
            src: ['ConsoleApplication5.csproj'],
            options: {
                projectConfiguration: 'Debug',
                targets: ['Clean', 'Rebuild'],
                stdout: true,
                maxCpuCount: 4,
                buildParameters: {
                    WarningLevel: 2
                },
                verbosity: 'quiet'
            }
        }
    }
});
```

## Contributing
In lieu of a formal styleguide, take care to maintain the existing coding style. Add to the VS integration tests for any new or changed functionality.


