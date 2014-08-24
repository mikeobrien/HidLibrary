module.exports = function (grunt) {
    grunt.initConfig({
        nugetpack: {
            dist: {
                src: 'tests/Package.nuspec',
                dest: 'tests/'
            }
        }
    });

    grunt.loadTasks('tasks');

    grunt.registerTask('default', ['nugetpack']);
};