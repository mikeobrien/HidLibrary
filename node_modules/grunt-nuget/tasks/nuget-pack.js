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

module.exports = function (grunt) {
    var _ = grunt.util._,
        async = grunt.util.async,
        nuget = require("../libs/nuget")(grunt);

    grunt.registerMultiTask('nugetpack', "NuGet Pack - Create NuGet package", function () {
        var params = this.options(),
            done = this.async();

        async.forEach(
            this.files,
            function (file, callback) {
                var dest = file.dest || "";

                async.forEach(
                    file.src,
                    function (src, complete) {
                        nuget.pack(src, _.extend(params, { outputDirectory: dest }), complete);
                    },
                    callback
                );
            },
            function (err) {
                if (err) {
                    grunt.log.error().error(err);
                    done(false);
                    return;
                }

                grunt.log.ok("NuGet Packages created !");
                done();
            }
        );
    });
};
