var fs = require('fs'),
    path = require('path'),
    _ = require('underscore'),
    regex = require('./regex.js');


exports.getSolutionProjects = function(solutionPath) {
    if (!fs.existsSync(solutionPath)) throw new Error('Solution file not found: ' + path.resolve(solutionPath));
    var projectRegEx = /Project\s*\(\s*\".*?\"\s*\)\s*=\s*\".*?\"\s*,\s*\"(.*?)\"\s*,\s*\".*?\"/ig;
    var solutionDirectory = path.dirname(solutionPath);
    return regex.matchAll(projectRegEx, fs.readFileSync(solutionPath, 'utf8')).
        map(function(projectPath) { return projectPath.replace(/\\/g, path.sep); }).
        map(function(projectPath) { return path.normalize(path.join(solutionDirectory, projectPath)); });
};

exports.getProjectInfo = function(projectPath) {
    if (!fs.existsSync(projectPath)) throw new Error('Project file not found: ' + path.resolve(projectPath));
    var projectDirectory = path.dirname(projectPath);
    var project = fs.readFileSync(projectPath, 'utf8');
    var outputType = project.match(/<OutputType.*?>(.*?)<\/OutputType>/)[1];
    var assemblyName = project.match(/<AssemblyName.*?>(.*?)<\/AssemblyName>/)[1] +
        (outputType === 'Library' ? '.dll' : '.exe');

    return {
        path: projectPath,
        references: regex.matchAll(/<Reference .*?Include\s*=\s*\"(.*?)[,\"].*?>/g, project),
        output: _.uniq(regex.matchAll(/<OutputPath.*?>(.*?)<\/OutputPath>/g, project)).
            map(function(assemblyPath) { return path.normalize(path.join(projectDirectory, 
                assemblyPath.replace(/\\/g, path.sep), assemblyName)); })
    };
};

exports.getSolutionProjectInfo = function(solution) {
    return [].concat.apply([], exports.getSolutionProjects(solution).
        map(function(project) { return exports.getProjectInfo(project); }));
};