module.exports = function(grunt) {
    grunt.loadNpmTasks('grunt-msbuild');
    grunt.loadNpmTasks('grunt-dotnet-assembly-info');
    grunt.loadNpmTasks('grunt-nunit-runner');
    grunt.loadNpmTasks('grunt-nuget');

    grunt.registerTask('default', ['msbuild', 'nunit']);
    grunt.registerTask('ci', ['assemblyinfo', 'msbuild', 'nunit', 'nugetpack']);
    grunt.registerTask('deploy', ['assemblyinfo', 'msbuild', 'nunit', 'nugetpack', 'nugetpush']);

    grunt.initConfig({
        assemblyinfo: {
            options: {
                files: ['src/HidLibrary.sln'],
                info: {
                    version: process.env.BUILD_NUMBER,
                    fileVersion: process.env.BUILD_NUMBER
                }
            }
        },
        msbuild: {
            src: ['src/HidLibrary.sln'],
            options: {
                projectConfiguration: 'Release',
                targets: ['Clean', 'Rebuild'],
                version: 4.0,
                stdout: true
            }
        },
        nunit: {
            files: ['src/HidLibrary.sln'],
            options: {
                teamcity: true
            }
        },
        nugetpack: {
            hidlibrary: {
                src: 'HidLibrary.nuspec',
                dest: './'
            },
            options: {
                version: process.env.BUILD_NUMBER
            }
        },
        nugetpush: {
            hidlibrary: {
                src: '*.nupkg'
            },
            options: {
                apiKey: process.env.NUGET_API_KEY
            }
        }
    });
}
