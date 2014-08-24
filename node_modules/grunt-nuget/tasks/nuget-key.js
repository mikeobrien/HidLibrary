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

    grunt.registerTask('nugetkey', "NuGet Set API Key - Set globally API Key for specified server or NuGet Gallery", function () {
        var done = this.async(),
            key = grunt.option("key"),
            source = grunt.option("source"),
            args = {};

        if (!key) {
            grunt.log.error().error("No key provided, please provide a key by adding --key arguments to console");
            done(false);
            return;
        }

        if (source) {
            args.source = source;
        }

        nuget.setapikey(key, args, function (err) {
            if (err) {
                grunt.log.error().error(err);
                done(false);
                return;
            }

            grunt.log.ok("NuGet API Key successfully stored !");
            done();
        });
    });
};
