var fs = require('fs'),
    path = require('path'),
    regex = require('./regex.js');


exports.getSolutionProjects = function(solution) {
    if (!fs.existsSync(solution)) throw new Error('Solution file not found: ' + path.resolve(solution));
    var projectRegEx = /Project\s*\(\s*\".*?\"\s*\)\s*=\s*\".*?\"\s*,\s*\"(.*?)\"\s*,\s*\".*?\"/ig;
    var solutionDirectory = path.dirname(solution);
    return regex.matchAll(projectRegEx, fs.readFileSync(solution, 'utf8')).
        map(function(projectPath) { return projectPath.replace(/\\/g, path.sep); }).
        map(function(projectPath) { return path.normalize(path.join(solutionDirectory, projectPath)); });
};

exports.getProjectFiles = function(project, filename) {
    if (!fs.existsSync(project)) throw new Error('Project file not found: ' + path.resolve(project));
    var fileRegEx = /Include\s*=\s*\"(.*?)\"/ig;
    var projectDirectory = path.dirname(project);
    return regex.matchAll(fileRegEx, fs.readFileSync(project, 'utf8')).
        map(function(projectPath) { return projectPath.replace(/\\/g, path.sep); }).
        filter(function(filePath) { return path.basename(filePath) === filename; }).
        map(function(filePath) { return path.normalize(path.join(projectDirectory, filePath)); });
};

exports.getSolutionFiles = function(solution, filename) {
    return [].concat.apply([], exports.getSolutionProjects(solution).
        map(function(project) { return exports.getProjectFiles(project, filename); }));
};