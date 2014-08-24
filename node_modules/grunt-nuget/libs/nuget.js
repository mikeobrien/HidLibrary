/*
 * grunt-nuget
 * https://github.com/spatools/grunt-nuget
 * Copyright (c) 2013 SPA Tools
 * Code below is licensed under MIT License
 *
 * Permission is hereby granted, free of charge, to any person 
 * obtaining a copy of this software and associated documentation 
 * files (the "Software"), to deal in the Software without restriction, 
 * including without limitation the rights to use, copy, modify, merge, 
 * publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be 
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
 * ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

var path = require("path");
var nugetPath = path.join(__dirname, "NuGet.exe");

module.exports = function (grunt) {
    var _ = grunt.util._,

        createArguments = function (command, path, args) {
            var result = [command, path];

            for (var key in args) {
                var argKey = "-" + key[0].toUpperCase() + key.slice(1);
                result.push(argKey);

                if (args[key])
                    result.push(args[key]);
            }

            return result;
        },
        createSpawnCallback = function (path, callback) {
            return function (error, result, code) {
                if (error) {
                    var _error = "Error while trying to execute NuGet Command Line on file " + path + "\n" + error;
                    callback(error);
                }
                else {
                    grunt.log.ok();
                    //grunt.log.writeln(result);
                    callback();
                }
            }
        },

        isSpecFile = function (file) {
            return path.extname(file) === ".nuspec";
        },
        isPackageFile = function (file) {
            return path.extname(file) === ".nupkg";
        },

        pack = function (path, args, callback) {
            if (!isSpecFile(path)) {
                callback("File path '" + path + "' is not a NuGet specification file !");
                return;
            }

            grunt.log.write("Trying to create NuGet package from " + path + ". ");
            grunt.util.spawn({ cmd: nugetPath, args: createArguments("Pack", path, args) }, createSpawnCallback(path, callback));
        },
        push = function (path, args, callback) {
            if (!isPackageFile(path)) {
                callback("File path '" + path + "' is not a NuGet package file !");
                return;
            }

            grunt.log.write("Trying to publish NuGet package " + path + ". ");
            grunt.util.spawn({ cmd: nugetPath, args: createArguments("Push", path, args) }, createSpawnCallback(path, callback));
        },
        setapikey = function (key, args, callback) {
            grunt.util.spawn({ cmd: nugetPath, args: createArguments("SetApiKey", key, args) }, createSpawnCallback(null, callback));
        };

    return {
        isSpecFile: isSpecFile,
        isPackageFile: isPackageFile,

        pack: pack,
        push: push,
        setapikey: setapikey
    };
};
